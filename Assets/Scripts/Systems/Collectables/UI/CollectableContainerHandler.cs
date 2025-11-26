using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public class CollectableContainerHandler : MonoBehaviour
{
    [Header("Lists")]
    [SerializeField] private List<CollectableSO> collectableList;
    [Space]
    [SerializeField] private List<CollectableUI> collectableUIList;

    [Header("Debug")]
    [SerializeField] private bool debug;

    public static event EventHandler<OnCollectablesPopulatedEventArgs> OnCollectablesPopulated;

    public class OnCollectablesPopulatedEventArgs : EventArgs
    {
        public List<CollectableUI> collectableUIList;
    }

    private void Start()
    {
        SetCollectablesUI();
    }

    private void SetCollectablesUI()
    {
        //Count of CollectableList and CollectableUIList must match
        for(int i = 0; i < collectableList.Count; i++)
        {
            SetCollectableUI(collectableList[i], collectableUIList[i]);
        }

        OnCollectablesPopulated?.Invoke(this, new OnCollectablesPopulatedEventArgs { collectableUIList = collectableUIList });
    }

    private void SetCollectableUI(CollectableSO collectableSO, CollectableUI collectableUI)
    {
        collectableUI.SetCollectable(collectableSO);
    }
}
