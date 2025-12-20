using System;
using UnityEngine;
using UnityEngine.UI;

public class SnapScrollIndexButton : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private SnappingScrollMenuUI snappingScrollMenuUI;
    [SerializeField] private Button button;

    [Header("Settings")]
    [SerializeField] private int index;

    private void Awake()
    {
        InitializeButtonsListeners();
    }

    private void InitializeButtonsListeners()
    {
        button.onClick.AddListener(SnapToIndex);
    }

    private void SnapToIndex()
    {
        snappingScrollMenuUI.SnapToIndex(index, false, false);
    }
}
