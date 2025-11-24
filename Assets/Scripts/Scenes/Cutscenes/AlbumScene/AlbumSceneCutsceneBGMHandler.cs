using UnityEngine;

public class AlbumSceneCutsceneBGMHandler : MonoBehaviour
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
        AlbumSceneCutsceneUIHandler.OnCutscenePlay += AlbumSceneCutsceneUIHandler_OnCutscenePlay;
        AlbumSceneCutsceneUIHandler.OnCutsceneConclude += AlbumSceneCutsceneUIHandler_OnCutsceneConclude;
    }

    private void OnDisable()
    {
        AlbumSceneCutsceneUIHandler.OnCutscenePlay -= AlbumSceneCutsceneUIHandler_OnCutscenePlay;
        AlbumSceneCutsceneUIHandler.OnCutsceneConclude -= AlbumSceneCutsceneUIHandler_OnCutsceneConclude;
    }

    private void PlayCutsceneBGM(AudioClip cutsceneBGM)
    {
        cutsceneBGMAudioSource.Stop();

        if (cutsceneBGM == null) return;

        cutsceneBGMAudioSource.clip = cutsceneBGM;
        cutsceneBGMAudioSource.Play();
    }

    #region Subscriptions
    private void AlbumSceneCutsceneUIHandler_OnCutscenePlay(object sender, AlbumSceneCutsceneUIHandler.OnCutsceneEventArgs e)
    {
        CutscenesVolumeFadeManager.Instance.FadeInVolume(cutscenesVolumeFadeInTime);
        PlayCutsceneBGM(e.cutsceneSO.cutsceneBGM);
    }

    private void AlbumSceneCutsceneUIHandler_OnCutsceneConclude(object sender, AlbumSceneCutsceneUIHandler.OnCutsceneEventArgs e)
    {
        CutscenesVolumeFadeManager.Instance.FadeOutVolume(cutscenesVolumeFadeOutTime);
    }
    #endregion
}
