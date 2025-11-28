using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class RegularSceneCutsceneUI : MonoBehaviour
{
    public static RegularSceneCutsceneUI Instance { get; private set; }

    [Header("Components")]
    [SerializeField] private Button skipCutsceneButton;
    [SerializeField] private UIPointerDetector skipCutscenePanelDetector;

    private void OnEnable()
    {
        skipCutscenePanelDetector.OnPointerPressed += SkipCutscenePanelDetector_OnPointerPressed;
    }

    private void OnDisable()
    {
        skipCutscenePanelDetector.OnPointerPressed -= SkipCutscenePanelDetector_OnPointerPressed;
    }

    private void Awake()
    {
        IntializeButtonsListeners();
        SetSingleton();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one AlbumSceneCutsceneUI instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    private void IntializeButtonsListeners()
    {
        skipCutsceneButton.onClick.AddListener(SkipCutscenePanel);
    }

    private void SkipCutscenePanel() => RegularSceneCutsceneUIHandler.Instance.SkipCutscene();
    public void PlayCutscene(CutsceneSO cutsceneSO) => RegularSceneCutsceneUIHandler.Instance.PlayCutscene(cutsceneSO);  

    #region Subscriptions
    private void SkipCutscenePanelDetector_OnPointerPressed(object sender, EventArgs e)
    {
        RegularSceneCutsceneUIHandler.Instance.TouchSkipCutscenePanel();
    }
    #endregion
}
