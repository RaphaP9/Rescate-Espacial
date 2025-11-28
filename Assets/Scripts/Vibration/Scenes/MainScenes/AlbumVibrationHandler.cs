using UnityEngine;

public class AlbumVibrationHandler : SceneVibrationHandler
{
    [Header("Settings")]
    [SerializeField] private HapticPreset albumSectionSelectedHapticPreset;
    [SerializeField] private HapticPreset nextCutscenePanelHapticPreset;

    protected override void OnEnable()
    {
        base.OnEnable();
        AlbumPagesHandler.OnAlbumRelationshipSelected += AlbumSectionsHandler_OnAlbumRelationshipSelected;
        RegularSceneCutsceneUIHandler.OnNextCutscenePanelCreated += AlbumSceneCutsceneUIHandler_OnNextCutscenePanelCreated;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        AlbumPagesHandler.OnAlbumRelationshipSelected -= AlbumSectionsHandler_OnAlbumRelationshipSelected;
        RegularSceneCutsceneUIHandler.OnNextCutscenePanelCreated -= AlbumSceneCutsceneUIHandler_OnNextCutscenePanelCreated;
    }

    #region Subscriptions
    private void AlbumSectionsHandler_OnAlbumRelationshipSelected(object sender, AlbumPagesHandler.OnAlbumRelationshipSelectedEventArgs e)
    {
        if (e.instant) return;
        PlayHaptic_Unforced(albumSectionSelectedHapticPreset);
    }

    private void AlbumSceneCutsceneUIHandler_OnNextCutscenePanelCreated(object sender, RegularSceneCutsceneUIHandler.OnCutsceneEventArgs e)
    {
        PlayHaptic_Unforced(nextCutscenePanelHapticPreset);
    }
    #endregion
}
