using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataContainer : MonoBehaviour
{
    public static DataContainer Instance { get; private set; }

    [Header("Data")]
    [SerializeField] private Data data;

    public Data Data => data;

    #region Initialization & Settings
    private void Awake() //Remember this Awake Happens before all JSON awakes, initialization of container happens before JSON Load
    {
        SetSingleton();
        InitializeDataContainer();
    }

    private void SetSingleton()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void InitializeDataContainer()
    {
        data = new Data();
        data.Initialize();
    }

    public void SetData(Data perpetualData) => this.data = perpetualData;
    
    public void ResetData()
    {
        InitializeDataContainer();
    }
    #endregion

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public void IncreaseTimesEnteredGame() => data.timesEnteredGame +=1;
    public bool IsFirstTimeEnteringGame() => data.timesEnteredGame == 0;

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public bool HasConfiguredGame()
    {
        return data.passwordItemsIDs.Count >= PasswordUtilities.GetPasswordItemCount();
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public int GetMinigameTotalScoreByMinigame(Minigame minigame)
    {
        MinigameData minigameData = GetMinigameDataByMinigame(minigame);
        return minigameData.totalScore;
    }

    private MinigameData GetMinigameDataByMinigame(Minigame minigame)
    {
        switch (minigame)
        {
            case Minigame.MemoryMinigame:
            default:
                return data.memoryMinigameData;
            case Minigame.SilhouettesMinigame:
                return data.silhouettesMinigameData;
        }
    }

    private MinigameLandmarkData GetMinigameLandmarkData(Minigame minigame, MinigameLandmark minigameLandmark)
    {
        MinigameData minigameData = GetMinigameDataByMinigame(minigame);

        switch (minigameLandmark)
        {
            case MinigameLandmark.FirstLandmark:
            default:
                return minigameData.landmarkData1;
            case MinigameLandmark.SecondLandmark:
                return minigameData.landmarkData2;
            case MinigameLandmark.ThirdLandmark:
                return minigameData.landmarkData3;
        }
    }

    public LandmarkState GetLandmarkState(Minigame minigame, MinigameLandmark minigameLandmark)
    {
        MinigameLandmarkData minigameLandmarkData = GetMinigameLandmarkData(minigame, minigameLandmark);
        LandmarkState landmarkState = DataUtilities.TranslateLandmarkDataIntValueToState(minigameLandmarkData.landmarkState);
        return landmarkState;
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public void IncreaseTimesEnteredMinigame(Minigame minigame)
    {
        MinigameData minigameData = GetMinigameDataByMinigame(minigame);
        minigameData.timesEntered += 1;
    }

    public bool IsFirstTimeEnteringMinigame(Minigame minigame)
    {
        MinigameData minigameData = GetMinigameDataByMinigame(minigame);
        return minigameData.timesEntered == 0;
    }

    public bool HasEnteredMinigame(Minigame minigame)
    {
        MinigameData minigameData = GetMinigameDataByMinigame(minigame);
        return minigameData.timesEntered > 0;
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public void IncreaseTimesWonMinigame(Minigame minigame)
    {
        MinigameData minigameData = GetMinigameDataByMinigame(minigame);
        minigameData.timesWon += 1;
    }

    public void IncreaseTimesLostMinigame(Minigame minigame)
    {
        MinigameData minigameData = GetMinigameDataByMinigame(minigame);
        minigameData.timesLost += 1;
    }

    public void IncreaseTotalScoreMinigame(Minigame minigame, int score)
    {
        MinigameData minigameData = GetMinigameDataByMinigame(minigame);
        minigameData.totalScore += score;
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public bool HasUnlockedCutscene(CutsceneSO cutsceneSO)
    {
        if (data.cutscenesUnlockedIDs.Contains(cutsceneSO.id)) return true;
        return false;
    }

    public bool UnlockCutscene(CutsceneSO cutsceneSO) // Bool = success
    {
        if(HasUnlockedCutscene(cutsceneSO)) return false;

        data.cutscenesUnlockedIDs.Add(cutsceneSO.id);
        return true;
    }

    public int GetCutscenesUnlockedCount() => data.cutscenesUnlockedIDs.Count;

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    public bool HasCollectedCollectable(CollectableSO collectableSO)
    {
        if (data.collectablesCollectedIDs.Contains(collectableSO.id)) return true;
        return false;
    }

    public bool CollectCollectable(CollectableSO collectableSO) // Bool = success
    {
        if (HasCollectedCollectable(collectableSO)) return false;

        data.collectablesCollectedIDs.Add(collectableSO.id);
        return true;
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public void SetLandmarkState(Minigame minigame, MinigameLandmark minigameLandmark, LandmarkState landmarkState)
    {
        MinigameLandmarkData minigameLandmarkData = GetMinigameLandmarkData(minigame, minigameLandmark);
        int landmarkDataInt = DataUtilities.TranslateLandmarkStateToDataIntValue(landmarkState);

        minigameLandmarkData.landmarkState = landmarkDataInt;
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public void SetPasswordItemIDs(List<PasswordItemSO> passwordItemSOs)
    {
        data.passwordItemsIDs.Clear();

        foreach(PasswordItemSO passwordItemSO in passwordItemSOs)
        {
            data.passwordItemsIDs.Add(passwordItemSO.id);
        }
    }

    //Checks if data.dasswordItemIDs contains all item IDs from passwordItemSOs
    public bool PasswordMatches(List<PasswordItemSO> passwordItemSOs)
    {
        foreach (PasswordItemSO passwordItemSO in passwordItemSOs)
        {
            if (!data.passwordItemsIDs.Contains(passwordItemSO.id)) return false;
        }

        return true;
    }
}
