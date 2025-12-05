using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class NotUnlockedCutsceneCoverHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator animator;
    [SerializeField] private Button button;

    [Header("Settings")]
    [SerializeField, Range(3f,10f)] private float conditionShowTime;

    private bool showingUnlockDescription = false;

    private const string SHOW_CONDITION_TRIGGER = "ShowCondition";
    private const string HIDE_CONDITION_TRIGGER = "HideCondition";

    private void Awake()
    {
        InitializeButtonsListeners();
    }

    private void InitializeButtonsListeners()
    {
        button.onClick.AddListener(ShowUnlockDescription);
    }

    private void ShowUnlockDescription()
    {
        if (showingUnlockDescription) return;

        StartCoroutine(ShowUnlockDescriptionCoroutine());
    }

    private IEnumerator ShowUnlockDescriptionCoroutine()
    {
        animator.SetTrigger(SHOW_CONDITION_TRIGGER);

        yield return new WaitForSeconds(conditionShowTime);

        animator.SetTrigger(HIDE_CONDITION_TRIGGER);
    }

    public void SetShowingUnlockDescriptionTrue() => showingUnlockDescription = true;
    public void SetShowingUnlockDescriptionFalse() => showingUnlockDescription = false;
}
