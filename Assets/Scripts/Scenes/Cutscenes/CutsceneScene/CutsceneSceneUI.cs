using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutsceneSceneUI : MonoBehaviour
{
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
    }

    private void IntializeButtonsListeners()
    {
        skipCutsceneButton.onClick.AddListener(SkipCutscenePanel);
    }

    private void SkipCutscenePanel() => CutsceneSceneUIHandler.Instance.SkipCutscene();

    #region Subscriptions
    private void SkipCutscenePanelDetector_OnPointerPressed(object sender, EventArgs e)
    {
        CutsceneSceneUIHandler.Instance.TouchSkipCutscenePanel();   
    }
    #endregion
}
