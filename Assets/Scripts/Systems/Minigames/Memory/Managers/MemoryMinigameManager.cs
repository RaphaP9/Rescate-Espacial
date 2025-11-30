using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class MemoryMinigameManager : MinigameManager
{
    public static MemoryMinigameManager Instance {  get; private set; }

    [Header("Components")]
    [SerializeField] private MemoryMinigameSettings settings;
    [Space]
    [SerializeField] private GridLayoutGroup gridLayoutGroup;
    [SerializeField] private Transform cardsContainer;
    [Space]
    [SerializeField] private Transform cardPrefab;


    [Header("RuntimeFilled")]
    [SerializeField] private MinigameState minigameState;
    [Space]
    [SerializeField] private List<MemoryCardHandler> currentRoundCards;
    [Space]
    [SerializeField] private List<MemoryCardHandler> currentMatchedCards;
    [Space]
    [SerializeField] private List<MemoryCardHandler> currentRevealedCards;
    [Space]
    [SerializeField] private MemoryCardHandler lastRevealedCard;

    [Header("Debug")]
    [SerializeField] private bool debug;
    [SerializeField] private bool inputLocked;

    private enum MinigameState { StartingMinigame, RevealingCards, WaitForFirstCard, WaitForSecondCard, ProcessingPairMatch, ProcessingPairFail, EndingRound, SwitchingRound, Winning, Win, Losing, Lose}

    private bool gameEnded = false;
    private int currentRoundIndex = 0;

    private bool cardRevealed = false;

    #region Events

    public static event EventHandler<OnRevealTimeEventArgs> OnRevealTimeStart;
    public static event EventHandler<OnRevealTimeEventArgs> OnRevealTimeEnd;

    public static event EventHandler<OnMemoryRoundEventArgs> OnMemoryRoundStart;
    public static event EventHandler<OnMemoryRoundEventArgs> OnMemoryRoundEnd;

    //Event for scripts that require pair match event but respond before the OnPairMatch subscriptors
    //Ex. Collectable where you check if a pair has matched when the timer was 5s or less
    //If Time is added because OnPairMatch, the seconds might be added before the 5s or less condition is checked
    public static event EventHandler OnPairMatchPreliminar; 
    public static event EventHandler OnPairMatch;
    public static event EventHandler OnPairFailed;

    public static event EventHandler OnFirstCardRevealed;
    public static event EventHandler OnSecondCardRevealed;
    #endregion

    #region Custom Classes
    public class OnMemoryRoundEventArgs : OnRoundEventArgs
    {
        public MemoryRound memoryRound;
    }

    public class OnRevealTimeEventArgs : EventArgs
    {
        public float revealTime;
    }
    #endregion

    private void OnEnable()
    {
        MemoryCardHandler.OnCardRevealed += MemoryCardHandler_OnCardRevealed;
        MinigameTimerManager.OnTimeEnd += MinigameTimerManager_OnTimeEnd;
    }

    private void OnDisable()
    {
        MemoryCardHandler.OnCardRevealed -= MemoryCardHandler_OnCardRevealed;
        MinigameTimerManager.OnTimeEnd -= MinigameTimerManager_OnTimeEnd;
    }

    private void Awake()
    {
        SetSingleton();
    }

    private void Start()
    {
        InitializeVariables();
        StartCoroutine(MemoryMinigameCoroutine());
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            //Debug.LogWarning("There is more than one MemoryMinigameManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    private void InitializeVariables()
    {
        gameEnded = false;
        currentRoundIndex = 0;
        inputLocked = false;

        OnGameInitializedMethod();
    }

    #region Coroutines
    private IEnumerator MemoryMinigameCoroutine()
    {
        ClearCardsContainer();

        SetMinigameState(MinigameState.StartingMinigame);

        yield return new WaitForSeconds(settings.startingGameTime);

        while (!gameEnded)
        {
            yield return StartCoroutine(MemoryRoundCoroutine(settings.rounds[currentRoundIndex], currentRoundIndex));

            #region Minigame Completed Evaluation
            if (currentRoundIndex >= settings.rounds.Count - 1)
            {
                gameEnded = true;
            }
            else
            {
                currentRoundIndex++;
            }
            #endregion
        }

        yield return StartCoroutine(WinMinigameCoroutine());
    }

    private IEnumerator MemoryRoundCoroutine(MemoryRound memoryRound, int roundIndex)
    {
        SetUpGridLayout(memoryRound);

        List<MemoryCardSO> chosenPairs = GeneralUtilities.ChooseNRandomDifferentItemsFromPoolFisherYates(memoryRound.cardPool, memoryRound.pairCount);
        CreateCards(chosenPairs, memoryRound);

        OnRoundStartMethod(roundIndex, settings.rounds.Count);  
        OnMemoryRoundStart?.Invoke(this, new OnMemoryRoundEventArgs { memoryRound = memoryRound, roundIndex = roundIndex, totalRounds = settings.rounds.Count }); 

        SetMinigameState(MinigameState.RevealingCards);

        float revealTime = memoryRound.revealTime;

        OnRevealTimeStart?.Invoke(this, new OnRevealTimeEventArgs { revealTime = revealTime });

        yield return new WaitForSeconds(revealTime);

        OnRevealTimeEnd?.Invoke(this, new OnRevealTimeEventArgs { revealTime = revealTime });

        CoverCards(currentRoundCards);

        bool roundEnded = false;

        while (!roundEnded)
        {
            #region FirstCard
            SetMinigameState(MinigameState.WaitForFirstCard);

            yield return new WaitUntil(() => cardRevealed);
            cardRevealed = false;

            OnFirstCardRevealed?.Invoke(this, EventArgs.Empty);

            MemoryCardHandler firstCard = lastRevealedCard;
            #endregion

            #region SecondCard
            SetMinigameState(MinigameState.WaitForSecondCard);

            yield return new WaitUntil(() => cardRevealed);
            cardRevealed = false;

            OnSecondCardRevealed?.Invoke(this, EventArgs.Empty);

            MemoryCardHandler secondCard = lastRevealedCard;
            #endregion

            #region Pair Processing


            if(PairMatches(firstCard, secondCard)) //Instant Pair Processing
            {
                currentMatchedCards.Add(firstCard);
                currentMatchedCards.Add(secondCard);

                firstCard.SetCardWillMatchTrue();
                secondCard.SetCardWillMatchTrue();

                SetMinigameState(MinigameState.ProcessingPairMatch);
            }
            else
            {
                firstCard.SetCardWillFailTrue();
                secondCard.SetCardWillFailTrue();

                SetMinigameState(MinigameState.ProcessingPairFail);
            }

            StartCoroutine(ProcessPairCoroutine(firstCard, secondCard)); //Card Pair Processing (Separate Coroutine)

            currentRevealedCards.Clear();
            #endregion

            #region Round End Evaluation
            if(AllPairMatch())
            {
                SetMinigameState(MinigameState.EndingRound);

                yield return new WaitForSeconds(settings.allPairsMatchTime);

                OnRoundEndMethod(roundIndex, settings.rounds.Count);
                OnMemoryRoundEnd?.Invoke(this, new OnMemoryRoundEventArgs { memoryRound = memoryRound, roundIndex = roundIndex, totalRounds = settings.rounds.Count });

                //DisappearCards(currentRoundCards); //Only If cards do not dissapear after match

                if (IsLastRound(roundIndex))
                {
                    yield return new WaitForSeconds(settings.endLastRoundTimer);
                }
                else
                {
                    SetMinigameState(MinigameState.SwitchingRound);
                    yield return new WaitForSeconds(settings.switchRoundTimer);
                }

                currentMatchedCards.Clear();
                currentRoundCards.Clear();
                lastRevealedCard = null;

                roundEnded = true;

                ClearCardsContainer();
            }
            else
            {
                yield return new WaitForSeconds(settings.timeBetweenPairs);
            }
            #endregion
        }
    }

    private IEnumerator WinMinigameCoroutine()
    {
        SetMinigameState(MinigameState.Winning);
        OnGameWinningMethod();

        yield return new WaitForSeconds(settings.endingGameTime);

        SetMinigameState(MinigameState.Win);
        OnGameWonMethod();
    }

    private IEnumerator LoseMinigameByTimeCoroutine()
    {
        SetMinigameState(MinigameState.Losing);
        OnGameLosingMethod();

        yield return new WaitForSeconds(settings.endingGameTime);

        SetMinigameState(MinigameState.Lose);
        OnGameLostMethod();
    }

    private IEnumerator SetInputLockCooldownCoroutine()
    {
        yield return new WaitForSeconds(settings.cardRevealInputCooldown);

        inputLocked = false;
    }
    #endregion

    #region Setters
    private void SetUpGridLayout(MemoryRound memoryRound)
    {
        int columns = memoryRound.gridColumnCount;

        gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayoutGroup.constraintCount = columns;

        gridLayoutGroup.cellSize = new Vector2 (memoryRound.cardSize, memoryRound.cardSize);
    }

    private void CreateCards(List<MemoryCardSO> chosenPairs, MemoryRound memoryRound)
    {
        List<MemoryCardSO> cardList = new List<MemoryCardSO>(chosenPairs); //Add the chosen pairs
        cardList.AddRange(chosenPairs); //Add the corresponding second copy

        cardList = GeneralUtilities.FisherYatesShuffle(cardList);

        foreach (MemoryCardSO memoryCardSO in cardList)
        {
            CreateCard(memoryCardSO, memoryRound);
        }
    }

    private void CreateCard(MemoryCardSO memoryCardSO, MemoryRound memoryRound)
    {
        Transform createdCard = Instantiate(cardPrefab, cardsContainer);
        MemoryCardHandler memoryCardHandler = createdCard.GetComponentInChildren<MemoryCardHandler>();

        if (memoryCardHandler == null)
        {
            if (debug) Debug.Log("Instantiated card does not contain a MemoryCardHandler component.");
            return;
        }

        memoryCardHandler.SetMemoryCard(memoryCardSO);
        memoryCardHandler.SetBackSprite(memoryRound.cardBackSprite);
        currentRoundCards.Add(memoryCardHandler);
    }

    private void ClearCardsContainer()
    {
        for (int i = cardsContainer.childCount - 1; i >= 0; i--)
        {
            Destroy(cardsContainer.GetChild(i).gameObject);
        }
    }

    private void SetMinigameState(MinigameState state) => minigameState = state;

    #endregion

    #region Pair Processing
    private IEnumerator ProcessPairCoroutine(MemoryCardHandler firstCard, MemoryCardHandler secondCard)
    {
        List<MemoryCardHandler> evaluatedCards = new List<MemoryCardHandler> { firstCard, secondCard};

        yield return new WaitForSeconds(settings.pairProcessingTime); //pairProcessing is the same duration as FlipRevealAnimation (or at least that value)

        if (PairMatches(firstCard, secondCard))
        {
            MatchCards(evaluatedCards);
            OnPairMatchPreliminar?.Invoke(this, EventArgs.Empty);
            OnPairMatch?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            FailCards(evaluatedCards);
            OnPairFailed?.Invoke(this, EventArgs.Empty);
        }
    }

    private bool PairMatches(MemoryCardHandler firstCard, MemoryCardHandler secondCard) => firstCard.MemoryCardSO == secondCard.MemoryCardSO;
    private bool AllPairMatch() => currentMatchedCards.Count >= currentRoundCards.Count;
    private bool IsLastRound(int roundIndex) => roundIndex + 1 >= settings.rounds.Count;
    #endregion

    #region Cards
    private void CoverCards(List<MemoryCardHandler> memoryCardHandlers)
    {
        foreach (MemoryCardHandler memoryCardHandler in memoryCardHandlers)
        {
            memoryCardHandler.CoverCard();
        }
    }

    private void MatchCards(List<MemoryCardHandler> memoryCardHandlers)
    {
        foreach (MemoryCardHandler memoryCardHandler in memoryCardHandlers)
        {
            memoryCardHandler.MatchCard();
        }
    }

    private void FailCards(List<MemoryCardHandler> memoryCardHandlers)
    {
        foreach (MemoryCardHandler memoryCardHandler in memoryCardHandlers)
        {
            memoryCardHandler.FailMatch();
        }
    }

    private void DisappearCards(List<MemoryCardHandler> memoryCardHandlers)
    {
        foreach (MemoryCardHandler memoryCardHandler in memoryCardHandlers)
        {
            memoryCardHandler.DisappearCard();
        }
    }

    private bool CardIsBeingFlippedRevealed()
    {
        foreach (MemoryCardHandler memoryCardHandler in currentRoundCards)
        {
            if (memoryCardHandler.IsBeingFlippedReveal) return true;
        }

        return false;
    }

    private bool CardIsFailing()
    {
        foreach(MemoryCardHandler memoryCardHandler in currentRoundCards)
        {
            if (memoryCardHandler.IsFailing) return true;
            if (memoryCardHandler.CardWillFail) return true;
        }

        return false;
    }

    #endregion

    #region Public Methods
    public bool CanFlipCard()
    {
        if (inputLocked) return false;
        if (settings.waitForPairFail && CardIsFailing()) return false;
        if (settings.wairForCardReveal && CardIsBeingFlippedRevealed()) return false;

        return minigameState == MinigameState.WaitForFirstCard || minigameState == MinigameState.WaitForSecondCard;
    }

    public override bool CanPassTime() => minigameState == MinigameState.WaitForFirstCard || minigameState == MinigameState.WaitForSecondCard;

    public void SetInputLockCooldown()
    {
        if(inputLocked) return;

        inputLocked = true;
        StartCoroutine(SetInputLockCooldownCoroutine());
    }

    public void LoseMinigameByTime()
    {
        StopAllCoroutines();
        StartCoroutine(LoseMinigameByTimeCoroutine());
    }
    #endregion

    #region Subscriptions
    private void MemoryCardHandler_OnCardRevealed(object sender, MemoryCardHandler.OnCardRevealedEventArgs e)
    {
        currentRevealedCards.Add(e.memoryCardHandler);
        lastRevealedCard = e.memoryCardHandler;
        cardRevealed = true;
    }

    private void MinigameTimerManager_OnTimeEnd(object sender, EventArgs e)
    {
        LoseMinigameByTime();
    }
    #endregion
}
    