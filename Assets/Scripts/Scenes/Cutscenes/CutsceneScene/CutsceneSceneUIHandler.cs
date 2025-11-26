using UnityEngine;
using System;

public class CutsceneSceneUIHandler : MonoBehaviour
{
    public static CutsceneSceneUIHandler Instance { get; private set; }

    [Header("Next Scene Settings")]
    [SerializeField] private string nextScene;
    [SerializeField] private TransitionType nextSceneTransitionType;

    [Header("Components")]
    [SerializeField] private CutsceneSO cutsceneSO;

    [Header("UI Components")]
    [SerializeField] private Transform cutscenePanelsContainer;

    [Header("Settings")]
    [SerializeField,Range(3,5)] private int maxCutscenePanels;

    [Header("Runtime Filled")]
    [SerializeField] private CutscenePanelUIHandler currentCutscenePanelUI;
    [SerializeField] private CutscenePanel currentCutscenePanel;
    [SerializeField] private int currentCutscenePanelIndex;

    [Header("Debug")]
    [SerializeField] private bool debug;

    public static event EventHandler<OnCutsceneEventArgs> OnCutscenePlay;
    public static event EventHandler<OnCutsceneEventArgs> OnCutsceneConclude;
    public static event EventHandler<OnCutsceneEventArgs> OnNextCutscenePanelCreated;

    public class OnCutsceneEventArgs : EventArgs
    {
        public CutsceneSO cutsceneSO;
    }
    private void OnDisable()
    {
        DesubscribeToCurrentCutscenePanelUI();
    }

    private void Awake()
    {
        SetSingleton();
    }

    private void Start()
    {
        InitializeVariables();
        CreateCutscenePanel(currentCutscenePanelIndex);

        OnCutscenePlay?.Invoke(this, new OnCutsceneEventArgs { cutsceneSO = cutsceneSO });
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one CutsceneSceneManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    private void InitializeVariables()
    {
        currentCutscenePanelIndex = 0;
    }

    #region Panels

    private void CreateCutscenePanel(int index)
    {
        DesubscribeToCurrentCutscenePanelUI();

        Transform cutscenePanelTransform = Instantiate(cutsceneSO.cutscenePanels[index].panelPrefab, cutscenePanelsContainer);
        CutscenePanelUIHandler cutscenePanelUIHandler = cutscenePanelTransform.GetComponentInChildren<CutscenePanelUIHandler>();

        if (cutscenePanelUIHandler == null)
        {
            if (debug) Debug.Log("Instantiated CutscenePanelUI does not contain a CutscenePanelUIHandler.");
            return;
        }

        CutscenePanel cutscenePanel = cutsceneSO.cutscenePanels[index];

        cutscenePanelUIHandler.SetPanel(cutscenePanel);

        currentCutscenePanelUI = cutscenePanelUIHandler;
        currentCutscenePanel = cutscenePanel;

        SubscribeToCurrentCutscenePanelUI();
        EvaluatePanelContainerClearance();
    }

    private void CreateNextCutscenePanel()
    {
        if(currentCutscenePanelUI != null)
        {
            currentCutscenePanelUI.DisposePanel();
        }

        currentCutscenePanelIndex++;
        CreateCutscenePanel(currentCutscenePanelIndex);

        OnNextCutscenePanelCreated?.Invoke(this, new OnCutsceneEventArgs { cutsceneSO = cutsceneSO });
    }

    private void EvaluatePanelContainerClearance()
    {
        if(cutscenePanelsContainer.childCount > maxCutscenePanels)
        {
            Destroy(cutscenePanelsContainer.GetChild(0).gameObject); //Destroy the first child
        }
    }

    private void SubscribeToCurrentCutscenePanelUI()
    {
        if (currentCutscenePanel == null) return;
        currentCutscenePanelUI.OnAutoSkip += CurrentCutscenePanelUI_OnAutoSkip;
    }

    private void DesubscribeToCurrentCutscenePanelUI()
    {
        if (currentCutscenePanelUI == null) return;

        currentCutscenePanelUI.OnAutoSkip -= CurrentCutscenePanelUI_OnAutoSkip;
    }
    #endregion

    #region Public Methods
    public void TouchSkipCutscenePanel()
    {
        if (currentCutscenePanelUI == null) return;
        if (!currentCutscenePanelUI.CanSkipPanel) return;
        if (currentCutscenePanelUI.AutoSkip) return; //Touch won't work if autoskip

        SkipCutscenePanel();
    }

    private void SkipCutscenePanel()
    {
        if (currentCutscenePanelUI == null) return;

        if (cutsceneSO.IsLastCutscenePanel(currentCutscenePanel))
        {
            SkipCutscene();
        }
        else
        {
            CreateNextCutscenePanel();
        }
    }

    public void SkipCutscene()
    {
        ScenesManager.Instance.TransitionLoadTargetScene(nextScene, nextSceneTransitionType);
        OnCutsceneConclude?.Invoke(this, new OnCutsceneEventArgs { cutsceneSO = cutsceneSO });
    }
    #endregion

    #region Subscriptions
    private void CurrentCutscenePanelUI_OnAutoSkip(object sender, EventArgs e)
    {
        SkipCutscenePanel();
    }
    #endregion
}
