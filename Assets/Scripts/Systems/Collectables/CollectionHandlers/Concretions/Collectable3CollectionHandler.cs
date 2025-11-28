using UnityEngine;

public class Collectable3CollectionHandler : CollectableCollectionHandler
{
    //This collectable checks the first time user plays a cutscene in the Album
    private void OnEnable()
    {
        RegularSceneCutsceneUIHandler.OnCutscenePlay += AlbumSceneCutsceneUIHandler_OnCutscenePlay;
    }

    private void OnDisable()
    {
        RegularSceneCutsceneUIHandler.OnCutscenePlay -= AlbumSceneCutsceneUIHandler_OnCutscenePlay;
    }


    private void AlbumSceneCutsceneUIHandler_OnCutscenePlay(object sender, RegularSceneCutsceneUIHandler.OnCutsceneEventArgs e)
    {
        CollectCollectable(false);
    }
}
