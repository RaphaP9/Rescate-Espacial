using UnityEngine;
using UnityEngine.UI;
using System;

public class ScrollRectBottomDetector : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private ScrollRect scrollRect;

    [Header("Runtime Filled")]
    [SerializeField] private bool hasReachedBottom;

    private const float BOTTOM_POSITION_THRESHOLD = 0.01f;

    public event EventHandler OnBottomReached;

    private void Awake()
    {
        InitializeListeners();
    }

    private void Start()
    {
        InitializeVariables();
    }

    private void InitializeListeners()
    {
        scrollRect.onValueChanged.AddListener(OnScrollValueChanged);
    }

    private void InitializeVariables()
    {
        hasReachedBottom = false;
    }

    private void OnScrollValueChanged(Vector2 value)
    {
        HandleBottomReached();
    }

    private void HandleBottomReached()
    {
        if (hasReachedBottom) return;
        if (scrollRect.verticalNormalizedPosition > BOTTOM_POSITION_THRESHOLD) return;

        hasReachedBottom = true;

        OnBottomReached?.Invoke(this, EventArgs.Empty); 
    }
}
