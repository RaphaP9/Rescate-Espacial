using UnityEngine;

public class SkipPanelIndicatorHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private CutscenePanelUIHandler cutscenePanelUIHandler;
    [SerializeField] private Animator animator;

    private const string SHOW_TRIGGER = "Show";

    private void OnEnable()
    {
        cutscenePanelUIHandler.OnCanSkipPanel += CutscenePanelUIHandler_OnCanSkipPanel;
    }

    private void OnDisable()
    {
        cutscenePanelUIHandler.OnCanSkipPanel -= CutscenePanelUIHandler_OnCanSkipPanel;
    }

    private void ShowIndicator()
    {
        animator.SetTrigger(SHOW_TRIGGER);
    }

    private void CutscenePanelUIHandler_OnCanSkipPanel(object sender, System.EventArgs e)
    {
        ShowIndicator();
    }
}
