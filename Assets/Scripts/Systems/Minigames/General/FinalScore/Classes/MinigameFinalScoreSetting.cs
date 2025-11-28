using UnityEngine;
using System.Collections.Generic;

[System.Serializable]   
public class MinigameFinalScoreSetting
{
    [Range(0, 1000)] public int minimunScore;
    [Space]
    public Sprite sprite;
    [Space]
    public string messageLocalizationTable;
    public string messageLocalizationBinding;
    [Space]
    public List<FinalScoreLocalizedAudioClipSetting> finalScoreLocalizedAudioclipSettings;
}

[System.Serializable]
public class FinalScoreLocalizedAudioClipSetting
{
    public string localizationTable;
    public string localizationBinding;
}
