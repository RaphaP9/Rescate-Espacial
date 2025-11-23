using System;
using UnityEngine;

public class AlbumPageUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator animator;

    private const string SHOW_TRIGGER = "Show";
    private const string HIDE_TRIGGER = "Hide";

    private const string SHOWING_ANIMATION_NAME = "Showing";
    private const string HIDDEN_ANIMATION_NAME = "Hidden";

    public void Select()
    {
        animator.ResetTrigger(HIDE_TRIGGER);
        animator.SetTrigger(SHOW_TRIGGER);
    }
    public void Deselect()
    {
        animator.ResetTrigger(SHOW_TRIGGER);
        animator.SetTrigger(HIDE_TRIGGER);
    }

    public void SelectInstant()
    {
        animator.ResetTrigger(SHOW_TRIGGER);
        animator.ResetTrigger(HIDE_TRIGGER);

        animator.Play(SHOWING_ANIMATION_NAME);
    }

    public void DeselectInstant()
    {
        animator.ResetTrigger(SHOW_TRIGGER);
        animator.ResetTrigger(HIDE_TRIGGER);

        animator.Play(HIDDEN_ANIMATION_NAME);
    }
}
