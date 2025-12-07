using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class UIVerticalDragDetector : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Settings")]
    [SerializeField, Range (0f, 1000f)] private float minVerticalDelta;

    public event EventHandler OnVerticalDragUp;
    public event EventHandler OnVerticalDragDown;

    private Vector2 startPos;
    private bool dragTriggered = false;

    public void OnBeginDrag(PointerEventData eventData)
    {
        startPos = eventData.position;
        dragTriggered = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        float deltaY = eventData.position.y - startPos.y;
        float deltaX = eventData.position.x - startPos.x;

        if (dragTriggered) return;

        if (Mathf.Abs(deltaX) >= Mathf.Abs(deltaY)) return; //Avoid diagonal drag detection

        if (Mathf.Abs(deltaY) >= minVerticalDelta)
        {
            if(deltaY > 0)
            {
                OnVerticalDragUp?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                OnVerticalDragDown?.Invoke(this, EventArgs.Empty);
            }

            dragTriggered = true;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        dragTriggered = false;
    }
}