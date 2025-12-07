using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class UIPointerDetector : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler,  IBeginDragHandler, IEndDragHandler
{
    public event EventHandler OnPointerPressed;
    public event EventHandler OnPointerReleased;
    public event EventHandler OnPointerClicked;
    public event EventHandler OnDragStart;
    public event EventHandler OnDragEnd;

    public void OnPointerDown(PointerEventData eventData)
    {
        OnPointerPressed?.Invoke(this, EventArgs.Empty);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        OnPointerReleased?.Invoke(this, EventArgs.Empty);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnPointerClicked?.Invoke(this, EventArgs.Empty);  
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        OnDragStart?.Invoke(this, EventArgs.Empty);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        OnDragEnd?.Invoke(this, EventArgs.Empty);
    }
}
