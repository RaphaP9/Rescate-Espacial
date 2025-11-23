using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class AlbumSpecificSectionsButtonsHandler : MonoBehaviour
{
    [Header("Lists")]
    [SerializeField] private List<AlbumSceneButtonRelationship> albumSceneButtonRelationships;

    [System.Serializable]
    public class AlbumSceneButtonRelationship
    {
        public Button button;
        public string sceneName;
        public TransitionType sceneTransitionType;
    }

    private void Awake()
    {
        InitializeButtonsListeners();
    }

    private void InitializeButtonsListeners()
    {
        foreach(AlbumSceneButtonRelationship relationship in albumSceneButtonRelationships)
        {
            relationship.button.onClick.AddListener(() => LoadAlbumSpecificScene(relationship.sceneName, relationship.sceneTransitionType));
        }
    }

    private void LoadAlbumSpecificScene(string sceneName, TransitionType sceneTransitionType)
    {
        ScenesManager.Instance.TransitionLoadTargetScene(sceneName, sceneTransitionType);
    }
}
