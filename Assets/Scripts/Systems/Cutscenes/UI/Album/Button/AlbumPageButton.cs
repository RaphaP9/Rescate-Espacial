using System;
using UnityEngine;
using UnityEngine.UI;

public class AlbumPageButton : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator animator;
    [SerializeField] private Button pageButton;

    private const string SELECT_TRIGGER = "Select";
    private const string DESELECT_TRIGGER = "Deselect";

    private const string SELECTED_ANIMATION_NAME = "Selected";
    private const string DESELECTED_ANIMATION_NAME = "Deselected";

    public static event EventHandler<OnButtonClickedEventArgs> OnButtonClicked;

    public class OnButtonClickedEventArgs : EventArgs
    {
        public AlbumPageButton albumPageButton;
    }

    private void Awake()
    {
        InitializeButtonsListeners();
    }

    private void InitializeButtonsListeners()
    {
        pageButton.onClick.AddListener(OnButtonClickedMethod);
    }

    private void OnButtonClickedMethod()
    {
        OnButtonClicked?.Invoke(this, new OnButtonClickedEventArgs { albumPageButton = this});
    }

    public void Select()
    {
        animator.ResetTrigger(DESELECT_TRIGGER);
        animator.SetTrigger(SELECT_TRIGGER);
    }
    public void Deselect()
    {
        animator.ResetTrigger(SELECT_TRIGGER);
        animator.SetTrigger(DESELECT_TRIGGER);
    }

    public void SelectInstant()
    {
        animator.ResetTrigger(SELECT_TRIGGER);
        animator.ResetTrigger(DESELECT_TRIGGER);

        animator.Play(SELECTED_ANIMATION_NAME);
    }

    public void DeselectInstant()
    {
        animator.ResetTrigger(SELECT_TRIGGER);
        animator.ResetTrigger(DESELECT_TRIGGER);

        animator.Play(DESELECTED_ANIMATION_NAME);
    }
}
