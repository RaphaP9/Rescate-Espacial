using System;
using UnityEngine;

public class LandmarkUI : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Minigame minigame;
    [SerializeField] private MinigameLandmark minigameLandmark;
    [SerializeField] private MinigameLandmarkSettingsSO landmarkSettings;
    [Space]
    [SerializeField] private CutsceneSO unlockedCutscene;

    [Header("RuntimeFilled")]
    [SerializeField] private LandmarkState landmarkState;
    [Space]
    [SerializeField] private int currentMinigameScore;
    [SerializeField] private int targetScore;

    public MinigameLandmark MinigameLandmark => minigameLandmark;
    public int TargetScore => targetScore;  
    public LandmarkState LandmarkState => landmarkState;

    public event EventHandler<OnLandmarkStateEventArgs> OnLandmarkInitialized;
    public event EventHandler OnLandmarkClaimed;

    public class OnLandmarkStateEventArgs : EventArgs
    {
        public LandmarkState landmarkState;
    }

    private void Start()
    {
        SetLandmark();
        CalculateMinigameScore();

        CheckLandmarkUnlockedStart();

        InitializeLandmark();
    }

    #region Start Methods

    private void SetLandmark()
    {
        landmarkState = DataContainer.Instance.GetLandmarkState(minigame, minigameLandmark);

        switch (minigameLandmark)
        {
            case MinigameLandmark.FirstLandmark:
            default:
                targetScore = landmarkSettings.landmarkScore1;
                break;
            case MinigameLandmark.SecondLandmark:
                targetScore = landmarkSettings.landmarkScore2;
                break;
            case MinigameLandmark.ThirdLandmark:
                targetScore = landmarkSettings.landmarkScore3;
                break;
        }        
    }

    private void CalculateMinigameScore()
    {
        currentMinigameScore = DataContainer.Instance.GetMinigameTotalScoreByMinigame(minigame);
    }

    //Check if score is met and Not Unlocked
    private void CheckLandmarkUnlockedStart()
    {
        if (landmarkState != LandmarkState.NotUnlocked) return;
        if (currentMinigameScore < targetScore) return;

        SetLandmarkState(LandmarkState.Unlocked);

        //NOTE:Using async is risky if two Landmarks are set in same frame (Using Async Serialization on same frame and on same file)
        //Edge case is two landmarks Unlocking on start
        //This will mathematically never happen in game, but might on testing
        //Thats why we use Sychronous Save in this case
    }

    private void InitializeLandmark()
    {
        OnLandmarkInitialized?.Invoke(this, new OnLandmarkStateEventArgs { landmarkState = landmarkState });
    }
    #endregion

    #region Utility Methods
    private void SetLandmarkState(LandmarkState landmarkState)
    {
        this.landmarkState = landmarkState;
        DataContainer.Instance.SetLandmarkState(minigame, minigameLandmark, landmarkState);

        GeneralDataManager.Instance.SaveJSONData();
    }
    #endregion

    #region Public Methods
    public void ClaimLandmark()
    {
        CutsceneUnlockHandler.Instance.UnlockCutscene(unlockedCutscene);

        SetLandmarkState(LandmarkState.Claimed);
        OnLandmarkClaimed?.Invoke(this, EventArgs.Empty);
    }
    #endregion
}
