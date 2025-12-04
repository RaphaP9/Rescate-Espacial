using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransitionRandomImagePicker : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private SceneTransitionUIHandler sceneTransitionUIHandler;
    [SerializeField] private Image image;

    [Header("Lists")]
    [SerializeField] private List<Sprite> spritesPool;

    private void OnEnable()
    {
        sceneTransitionUIHandler.OnTransitionOutTrigger += SceneTransitionUIHandler_OnTransitionOutTrigger;
    }

    private void OnDisable()
    {
        sceneTransitionUIHandler.OnTransitionOutTrigger -= SceneTransitionUIHandler_OnTransitionOutTrigger;
    }

    private void SetRandomImage()
    {
        Sprite randomSprite = GeneralUtilities.ChooseRandomElementFromList(spritesPool);
        image.sprite = randomSprite;
    }

    private void SceneTransitionUIHandler_OnTransitionOutTrigger(object sender, SceneTransitionUIHandler.OnTransitionUIEventArgs e)
    {
        SetRandomImage();
    }
}
