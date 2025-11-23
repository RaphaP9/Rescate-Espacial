using UnityEngine;

public class AlbumSFXHandler : SceneSFXHandler
{
    protected override void OnEnable()
    {
        base.OnEnable();
        AlbumPagesHandler.OnAlbumRelationshipSelected += AlbumSectionsHandler_OnAlbumRelationshipSelected;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        AlbumPagesHandler.OnAlbumRelationshipSelected -= AlbumSectionsHandler_OnAlbumRelationshipSelected;
    }

    #region Subscriptions
    private void AlbumSectionsHandler_OnAlbumRelationshipSelected(object sender, AlbumPagesHandler.OnAlbumRelationshipSelectedEventArgs e)
    {
        if (e.instant) return;
        PlaySFX_Unpausable(SFXPool.genericButtonB);
    }
    #endregion
}