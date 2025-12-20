using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System;

public class SnappingScrollMenuUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Canvas canvas;
    [SerializeField] private RectTransform content;
    [Space]
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private ScrollRectDragDetector scrollRectDragDetector;

    [Header("Lists")]
    [SerializeField] private List<SnappingScrollMenuItemUI> items;

    [Header("Settings")]
    [SerializeField, Range(1f,100f)] private float snapSpeed = 10f;
    [SerializeField] private int startIndex;

    [Header("Runtime Filled")]
    [SerializeField] private SnappingScrollMenuItemUI targetSnapItem;
    [SerializeField] private int currentIndex;

    private bool initializationLogicPerformed = false;

    #region Events
    public static event EventHandler<OnItemsInitializedEventArgs> OnItemsInitialized;

    public static event EventHandler<OnItemEventArgs> OnFirstItemReached;
    public static event EventHandler<OnItemEventArgs> OnLastItemReached;

    public static event EventHandler<OnItemEventArgs> OnLastItemAway;
    public static event EventHandler<OnItemEventArgs> OnFirstItemAway;

    public static event EventHandler<OnItemSnapEventArgs> OnItemSnap;
    #endregion

    #region Custom Classes
    [System.Serializable]
    public class OnItemsInitializedEventArgs : EventArgs
    {
        public List<SnappingScrollMenuItemUI> items;
    }

    public class OnItemSnapEventArgs : EventArgs
    {
        public int itemIndex;
        public bool instantly;
    }

    public class OnItemEventArgs : EventArgs
    {
        public bool instantly;
    }
    #endregion

    private void OnEnable()
    {
        SwipeManager.OnSwipeLeft += SwipeManager_OnSwipeLeft;
        SwipeManager.OnSwipeRight += SwipeManager_OnSwipeRight;
    }

    private void OnDisable()
    {
        SwipeManager.OnSwipeLeft -= SwipeManager_OnSwipeLeft;
        SwipeManager.OnSwipeRight -= SwipeManager_OnSwipeRight;
    }

    private void Start()
    {
        StartCoroutine(InitializationCoroutine());
    }

    private void Update()
    {
        HandleItemSnap();
    }

    private IEnumerator InitializationCoroutine()
    {
        //NOTE: This asumes previous scene was same resolution as this scene, so Canvas Layout will not take time to rebuild

        //Wait one frames
        yield return null;

        InitializeItems();
        SnapToIndex(startIndex, true, true);

        initializationLogicPerformed = true;
    }

    private void InitializeItems()
    {
        int index = 0;

        foreach(SnappingScrollMenuItemUI item in items)
        {
            Vector2 refferencePosition = UIUtilities.GetCanvasPosition(item.RectTransform, canvas) - UIUtilities.GetCanvasPosition(scrollRect.viewport, canvas);

            item.SetAssignedIndex(index);
            item.SetRefferencePosition(refferencePosition);

            index++;
        }

        OnItemsInitialized?.Invoke(this, new OnItemsInitializedEventArgs { items = items });
    }

    public void SnapToIndex(int index, bool instant, bool force)
    {
        if (!force && currentIndex == index) return;

        if (index < 0) return; //Invalid negative index
        if(index >= items.Count) return; //Invalid out of bounds index   

        int previousIndex = currentIndex;
        currentIndex = index;
        UpdateTargetSnapItemToIndex(currentIndex);

        if(instant) content.anchoredPosition = -targetSnapItem.RefferencePosition; //This makes Anchored Position Instant

        if (currentIndex >= items.Count - 1)
        {
            OnLastItemReached?.Invoke(this, new OnItemEventArgs { instantly = instant});
        }

        if (previousIndex <= 0)
        {
            OnFirstItemAway?.Invoke(this, new OnItemEventArgs { instantly = instant });
        }

        if (currentIndex <= 0)
        {
            OnFirstItemReached?.Invoke(this, new OnItemEventArgs { instantly = instant });
        }

        if (previousIndex >= items.Count - 1)
        {
            OnLastItemAway?.Invoke(this, new OnItemEventArgs { instantly = instant });
        }

        OnItemSnap?.Invoke(this, new OnItemSnapEventArgs { itemIndex = currentIndex, instantly = true });
    }

    private void HandleItemSnap()
    {
        if (!initializationLogicPerformed) return;
        if (scrollRectDragDetector.IsDragging) return;

        content.anchoredPosition = Vector2.Lerp(content.anchoredPosition, -targetSnapItem.RefferencePosition, Time.deltaTime * snapSpeed);
    }

    #region Displacement Commands
    public void DisplaceRightCommand()
    {
        TryIncreaseIndex(false);
        UpdateTargetSnapItemToIndex(currentIndex);

        OnItemSnap?.Invoke(this, new OnItemSnapEventArgs { itemIndex = currentIndex, instantly = false});
    }

    public void DisplaceLeftCommand()
    {
        TryDecreaseIndex(false);
        UpdateTargetSnapItemToIndex(currentIndex);

        OnItemSnap?.Invoke(this, new OnItemSnapEventArgs { itemIndex = currentIndex, instantly = false });
    }
    #endregion

    #region Increase Decrease Index
    private void TryIncreaseIndex(bool instant)
    {
        if (currentIndex >= items.Count - 1) return; //Is in last index

        int previousIndex = currentIndex;
        currentIndex++;

        if (currentIndex >= items.Count - 1)
        {
            OnLastItemReached?.Invoke(this, new OnItemEventArgs { instantly = instant });
        }

        if(previousIndex <= 0)
        {
            OnFirstItemAway?.Invoke(this, new OnItemEventArgs { instantly = instant });
        }
    }

    private void TryDecreaseIndex(bool instant)
    {
        if (currentIndex <= 0) return; //Is in first index

        int previousIndex = currentIndex;
        currentIndex--;

        if (currentIndex <= 0)
        {
            OnFirstItemReached?.Invoke(this, new OnItemEventArgs { instantly = instant });
        }

        if (previousIndex >= items.Count - 1)
        {
            OnLastItemAway?.Invoke(this, new OnItemEventArgs { instantly = instant });
        }
    }
    #endregion

    private void UpdateTargetSnapItemToIndex(int index)
    {
        foreach(SnappingScrollMenuItemUI item in items)
        {
            if (item.AssignedIndex != index) continue; //If not the same index, continue
            if (targetSnapItem == item) continue; //If already snapped, continue

            item.TriggerSnapEvents();
            targetSnapItem = item;
        }

        //targetSnapItem = items[index]; //Also can be
    }

    #region Subscriptions
    private void SwipeManager_OnSwipeRight(object sender, System.EventArgs e)
    {
        DisplaceLeftCommand();
    }

    private void SwipeManager_OnSwipeLeft(object sender, System.EventArgs e)
    {
        DisplaceRightCommand();
    }
    #endregion
}

