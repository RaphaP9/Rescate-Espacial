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

    [Header("General Settings")]
    [SerializeField, Range(0f, 30f)] private float timeToSkipPanel;

    [Header("Runtime Filled")]
    [SerializeField] private CutscenePanel cutscenePanel;
    [SerializeField] private bool canSkipPanel;

    #region Properties
    public bool CanSkipPanel => canSkipPanel;

    #endregion

    public event EventHandler OnCanSkipPanel;

    public void SetPanel(CutscenePanel cutscenePanel)
    {
        this.cutscenePanel = cutscenePanel;
        canSkipPanel = false;

        StartCoroutine(HandleCanSkipPanelCoroutine());
    }

    public void DisposePanel()
    {
        audioHandler.TerminateAudioHandler();
    }

    private IEnumerator HandleCanSkipPanelCoroutine()
    {
        yield return new WaitForSeconds(timeToSkipPanel);
        canSkipPanel = true;
        OnCanSkipPanel?.Invoke(this, EventArgs.Empty);
    }
}
