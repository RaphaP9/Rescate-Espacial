using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CutscenePanelUIHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Image panelImage;
    [SerializeField] private TextMeshProUGUI sentenceText;
    [Space]
    [SerializeField] private CutscenePanelAudioHandler audioHandler;
    [SerializeField] private CutscenePanelSFXHandler SFXHandler;

    [Header("General Settings")]
    [SerializeField, Range(0f, 30f)] private float timeToSkipPanel;
    [SerializeField] private bool autoSkip;

    [Header("Runtime Filled")]
    [SerializeField] private CutscenePanel cutscenePanel;
    [SerializeField] private bool canSkipPanel;

    #region Properties
    public bool CanSkipPanel => canSkipPanel;
    public bool AutoSkip => autoSkip;

    #endregion

    public event EventHandler<OnCanSkipEventArgs> OnCanSkipPanel;
    public event EventHandler OnAutoSkip;

    public class OnCanSkipEventArgs : EventArgs
    {
        public bool autoSkip;
    }

    public void SetPanel(CutscenePanel cutscenePanel)
    {
        this.cutscenePanel = cutscenePanel;
        canSkipPanel = false;

        StartCoroutine(HandleCanSkipPanelCoroutine());
    }

    public void DisposePanel()
    {
        audioHandler.TerminateAudioHandler();
        SFXHandler.TerminateSFXHandler();
    }

    private IEnumerator HandleCanSkipPanelCoroutine()
    {
        yield return new WaitForSeconds(timeToSkipPanel);
        canSkipPanel = true;

        OnCanSkipPanel?.Invoke(this, new OnCanSkipEventArgs { autoSkip = autoSkip});
        if(autoSkip) OnAutoSkip?.Invoke(this, EventArgs.Empty);
    }
}
