using UnityEngine;

public class AlbumCutsceneMusicAudioFadeHandler : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField, Range(0.25f, 1f)] private float musicVolumeFadeInTime = 0.5f;
    [SerializeField, Range(0.25f, 1f)] private float musicVolumeFadeOutTime = 0.5f;

    private void OnEnable()
    {
        RegularSceneCutsceneUIHandler.OnCutscenePlay += AlbumSceneCutsceneUIHandler_OnCutscenePlay;
        RegularSceneCutsceneUIHandler.OnCutsceneConclude += AlbumSceneCutsceneUIHandler_OnCutsceneConclude;
    }

    private void OnDisable()
    {
        RegularSceneCutsceneUIHandler.OnCutscenePlay -= AlbumSceneCutsceneUIHandler_OnCutscenePlay;
        RegularSceneCutsceneUIHandler.OnCutsceneConclude -= AlbumSceneCutsceneUIHandler_OnCutsceneConclude;
    }

    #region Subscriptions
    private void AlbumSceneCutsceneUIHandler_OnCutscenePlay(object sender, RegularSceneCutsceneUIHandler.OnCutsceneEventArgs e)
    {
        MusicVolumeFadeManager.Instance.FadeOutVolume(musicVolumeFadeOutTime);
    }

    private void AlbumSceneCutsceneUIHandler_OnCutsceneConclude(object sender, RegularSceneCutsceneUIHandler.OnCutsceneEventArgs e)
    {
        MusicVolumeFadeManager.Instance.FadeInVolume(musicVolumeFadeInTime);
    }
    #endregion
}
