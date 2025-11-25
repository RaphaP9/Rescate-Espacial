using UnityEngine;
using System.Collections.Generic;

public class InstancedFeedbackHandler : MonoBehaviour
{
    [Header("Lists")]
    [SerializeField] private List<Transform> feedbackPrefabList;

    [Header("Components")]
    [SerializeField] private Transform feedbackContainer;

    [Header("Runtime Filled")]
    [SerializeField] private InstancedFeedbackUI currentFeedbackUI;

    [Header("Debug")]
    [SerializeField] private bool debug;

    private void OnDisable()
    {
        ClearCurrentFeedbackUI();
    }

    protected virtual void CreateFeedback()
    {
        if (currentFeedbackUI != null)
        {
            if (debug) Debug.Log("Can not instantiate feedback. Current one exists.");
            return;
        }

        Transform chosenPrefab = GeneralUtilities.ChooseRandomElementFromList(feedbackPrefabList);

        if (chosenPrefab == null) return; //Chosen prefab is null when list has 0 elements

        Transform feedbackTransform = Instantiate(chosenPrefab, feedbackContainer);
        InstancedFeedbackUI instancedFeedbackUI = feedbackTransform.GetComponentInChildren<InstancedFeedbackUI>();

        if(instancedFeedbackUI == null)
        {
            if (debug) Debug.Log("Instantiated prefab does not contain a InstancedFeedbackUI component");
            return;
        }

        SetCurrentFeedbackUI(instancedFeedbackUI);    
    }

    private void SetCurrentFeedbackUI(InstancedFeedbackUI instancedFeedbackUI)
    {
        currentFeedbackUI = instancedFeedbackUI;
        currentFeedbackUI.OnFeedbackEnd += CurrentFeedbackUI_OnFeedbackEnd;
    }

    private void ClearCurrentFeedbackUI()
    {
        if (currentFeedbackUI == null) return;

        currentFeedbackUI.OnFeedbackEnd -= CurrentFeedbackUI_OnFeedbackEnd;
        currentFeedbackUI = null;
    }

    private void CurrentFeedbackUI_OnFeedbackEnd(object sender, System.EventArgs e)
    {
        ClearCurrentFeedbackUI();
    }

}
