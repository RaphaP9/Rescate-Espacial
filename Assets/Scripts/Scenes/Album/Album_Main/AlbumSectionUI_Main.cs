using UnityEngine;
using UnityEngine.UI;

public class AlbumSectionUI_Main : MonoBehaviour
{

    [Header("UI Components")]
    [SerializeField] private Button backButton;

    [Header("Settings")]
    [SerializeField] private string backScene;
    [SerializeField] private TransitionType backTransitionType;

    private void Awake()
    {
        IntializeButtonsListeners();
    }

    private void IntializeButtonsListeners()
    {
        backButton.onClick.AddListener(LoadBackScene);
    }

    private void LoadBackScene() => ScenesManager.Instance.TransitionLoadTargetScene(backScene, backTransitionType);
}
