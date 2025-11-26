using System;
using UnityEngine;

public class AlbumSceneCutsceneUIHandler : MonoBehaviour
{
    public static AlbumSceneCutsceneUIHandler Instance { get; private set; }

    [Header("Components")]
    [SerializeField] private Animator animator;

    [Header("UI Components")]
    [SerializeField] private Transform cutscenePanelsContainer;

    [Header("Settings")]
    [SerializeField, Range(3, 5)] private int maxCutscenePanels;

    [Header("Runtime Filled")]
    [SerializeField] private bool cutsceneActive; //Also manipulated via Animation Events
    [SerializeField] private CutsceneSO currentCutsceneSO;
    [SerializeField] private CutscenePanelUIHandler currentCutscenePanelUI;
    [SerializeField] private CutscenePanel currentCutscenePanel;
    [SerializeField] private int currentCutscenePanelIndex;

    [Header("Debug")]
    [SerializeField] private bool debug;

    private const string SHOW_TRIGGER = "Show";
    private const string HIDE_TRIGGER = "Hide";

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
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one AlbumSceneCutsceneUIHandler instance, proceding to destroy duplicate");
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

        Transform cutscenePanelTransform = Instantiate(currentCutsceneSO.cutscenePanels[index].panelPrefab, cutscenePanelsContainer);
        CutscenePanelUIHandler cutscenePanelUIHandler = cutscenePanelTransform.GetComponentInChildren<CutscenePanelUIHandler>();

        if (cutscenePanelUIHandler == null)
        {
            if (debug) Debug.Log("Instantiated CutscenePanelUI does not contain a CutscenePanelUIHandler.");
            return;
        }

        CutscenePanel cutscenePanel = currentCutsceneSO.cutscenePanels[index];

        cutscenePanelUIHandler.SetPanel(cutscenePanel);

        currentCutscenePanelUI = cutscenePanelUIHandler;
        currentCutscenePanel = cutscenePanel;

        SubscribeToCurrentCutscenePanelUI();

        EvaluatePanelContainerClearance();
    }

    private void CreateNextCutscenePanel()
    {
        if (currentCutscenePanelUI != null)
        {
            currentCutscenePanelUI.DisposePanel();
        }

        currentCutscenePanelIndex++;
        CreateCutscenePanel(currentCutscenePanelIndex);

        OnNextCutscenePanelCreated?.Invoke(this, new OnCutsceneEventArgs { cutsceneSO = currentCutsceneSO });
    }

    private void EvaluatePanelContainerClearance()
    {
        if (cutscenePanelsContainer.childCount > maxCutscenePanels)
        {
            Destroy(cutscenePanelsContainer.GetChild(0).gameObject); //Destroy the first child
        }
    }

    private void ClearPanelContainer()
    {
        foreach(Transform child in cutscenePanelsContainer)
        {
            Destroy(child.gameObject);
        }
    }

    private void ClearPanelContainerButLast()
    {
        if (cutscenePanelsContainer.childCount <= 1) return;

        for (int i = 0; i < cutscenePanelsContainer.childCount -1 ; i++)
        {
            Destroy(cutscenePanelsContainer.GetChild(i).gameObject);
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

    #region Animations

    private void ShowUI()
    {
        animator.ResetTrigger(HIDE_TRIGGER);
        animator.SetTrigger(SHOW_TRIGGER);
    }

    private void HideUI()
    {
        animator.ResetTrigger(SHOW_TRIGGER);
        animator.SetTrigger(HIDE_TRIGGER);
    }

    #endregion

    #region Public Methods
    public void SetCutsceneActive()
    {
        cutsceneActive = true;
    }

    public void SetCutsceneInactive()
    {
        currentCutsceneSO = null;
        currentCutscenePanel = null;
        currentCutscenePanelUI = null;
        currentCutscenePanelIndex = 0;

        ClearPanelContainer();

        cutsceneActive = false;
    }

    public void PlayCutscene(CutsceneSO cutsceneSO)
    {
        if (cutsceneActive) return;
        if (cutsceneSO.cutscenePanels.Count <= 0) return;

        ClearPanelContainer();

        currentCutsceneSO = cutsceneSO;
        currentCutscenePanelIndex = 0;

        SetCutsceneActive();
        CreateCutscenePanel(0); //0 is first index

        ShowUI();

        OnCutscenePlay?.Invoke(this, new OnCutsceneEventArgs { cutsceneSO = currentCutsceneSO });
    }

    public void TouchSkipCutscenePanel()
    {
        if (!cutsceneActive) return;
        if (currentCutscenePanelUI == null) return;
        if (!currentCutscenePanelUI.CanSkipPanel) return;
        if (currentCutscenePanelUI.AutoSkip) return; //Touch won't work if autoskip

        SkipCutscenePanel();
    }

    public void SkipCutscenePanel()
    {
        if (!cutsceneActive) return;
        if (currentCutscenePanelUI == null) return;

        if (currentCutsceneSO.IsLastCutscenePanel(currentCutscenePanel))
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
        ClearPanelContainerButLast();
        HideUI();

        OnCutsceneConclude?.Invoke(this, new OnCutsceneEventArgs { cutsceneSO = currentCutsceneSO });
    }
    #endregion

    #region Subscriptions
    private void CurrentCutscenePanelUI_OnAutoSkip(object sender, EventArgs e)
    {
        SkipCutscenePanel();
    }
    #endregion
}
