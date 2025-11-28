using UnityEngine;

public class RegularSceneCutsceneBGMHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private AudioSource cutsceneBGMAudioSource;

    [Header("Settings")]
    [SerializeField, Range(0.25f, 1f)] private float cutscenesVolumeFadeInTime = 0.3f;
    [SerializeField, Range(0.25f, 1f)] private float cutscenesVolumeFadeOutTime = 0.3f;

    [Header("Audio Settings")]
    [SerializeField] private bool hasAudio;
    [SerializeField] private string audioLocalizationTable;
    [SerializeField] private string audioLocalizationBinding;
    [Space]
    [SerializeField, Range(0f, 5f)] public float timeToPlayAudio;

    private void OnEnable()
    {
        RegularSceneCutsceneUIHandler.OnCutscenePlay += RegularSceneCutsceneUIHandler_OnCutscenePlay;
        RegularSceneCutsceneUIHandler.OnCutsceneConclude += RegularSceneCutsceneUIHandler_OnCutsceneConclude;
    }

    private void OnDisable()
    {
        RegularSceneCutsceneUIHandler.OnCutscenePlay -= RegularSceneCutsceneUIHandler_OnCutscenePlay;
        RegularSceneCutsceneUIHandler.OnCutsceneConclude -= RegularSceneCutsceneUIHandler_OnCutsceneConclude;
    }

    private void PlayCutsceneBGM(AudioClip cutsceneBGM)
    {
        cutsceneBGMAudioSource.Stop();

        if (cutsceneBGM == null) return;

        cutsceneBGMAudioSource.clip = cutsceneBGM;
        cutsceneBGMAudioSource.Play();
    }

    #region Subscriptions
    private void RegularSceneCutsceneUIHandler_OnCutscenePlay(object sender, RegularSceneCutsceneUIHandler.OnCutsceneEventArgs e)
    {
        CutscenesVolumeFadeManager.Instance.FadeInVolume(cutscenesVolumeFadeInTime);
        PlayCutsceneBGM(e.cutsceneSO.cutsceneBGM);
    }

    private void RegularSceneCutsceneUIHandler_OnCutsceneConclude(object sender, RegularSceneCutsceneUIHandler.OnCutsceneEventArgs e)
    {
        CutscenesVolumeFadeManager.Instance.FadeOutVolume(cutscenesVolumeFadeOutTime);
    }
    #endregion
}
