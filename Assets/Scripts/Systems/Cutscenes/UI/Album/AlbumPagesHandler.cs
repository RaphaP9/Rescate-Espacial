using UnityEngine;
using System;
using System.Collections.Generic;

public class AlbumPagesHandler : MonoBehaviour
{
    [Header("Lists")]
    [SerializeField] private List<AlbumPageButtonUIRelationship> albumPageButtonUIRelationships;

    [Header("Settings")]
    [SerializeField] private AlbumPageButton startingSelectedButton;

    [Header("Runtime Filled")]
    [SerializeField] private AlbumPageButtonUIRelationship currentSelectedRelationship;

    [Header("Debug")]
    [SerializeField] private bool debug;

    public static event EventHandler<OnAlbumRelationshipSelectedEventArgs> OnAlbumRelationshipSelected;
    public static event EventHandler<OnAlbumRelationshipSelectedEventArgs> OnAlbumRelationshipDeselected;

    public class OnAlbumRelationshipSelectedEventArgs : EventArgs
    {
        public AlbumPageButtonUIRelationship relationship;
        public bool instant;
    }

    [Serializable]
    public class AlbumPageButtonUIRelationship
    {
        public AlbumPageButton albumPageButton;
        public AlbumPageUI albumPageUI;
    }

    private void OnEnable()
    {
        AlbumPageButton.OnButtonClicked += AlbumPageButton_OnButtonClicked;
    }

    private void OnDisable()
    {
        AlbumPageButton.OnButtonClicked -= AlbumPageButton_OnButtonClicked;
    }

    private void Start()
    {
        SelectRelationshipByButton(startingSelectedButton, true);
    }


    private void SelectRelationshipByButton(AlbumPageButton albumPageButton, bool instant)
    {
        AlbumPageButtonUIRelationship relationship = GetRelationshipByButton(albumPageButton);

        if(relationship == null)
        {
            if (debug) Debug.Log("AlbumPageButtonUIRelationship is null.");
            return;
        }

        if (relationship == currentSelectedRelationship) return; //Already Selected

        if(IsValidRelationship(currentSelectedRelationship)) DeselectRelationship(currentSelectedRelationship, instant);

        SelectRelationship(relationship, instant);

        currentSelectedRelationship = relationship;
    }

    #region Utility Methods
    private AlbumPageButtonUIRelationship GetRelationshipByButton(AlbumPageButton albumPageButton)
    {
        foreach(AlbumPageButtonUIRelationship relationship in albumPageButtonUIRelationships)
        {
            if(relationship.albumPageButton == albumPageButton) return relationship;
        }

        return null;
    }

    private void SelectRelationship(AlbumPageButtonUIRelationship relationship, bool instant)
    {
        if (relationship == null) return;

        if (instant)
        {
            relationship.albumPageButton.SelectInstant();
            relationship.albumPageUI.SelectInstant();
        }
        else
        {
            relationship.albumPageButton.Select();
            relationship.albumPageUI.Select();
            relationship.albumPageUI.transform.SetAsLastSibling();
        }

        OnAlbumRelationshipSelected?.Invoke(this, new OnAlbumRelationshipSelectedEventArgs { relationship = relationship, instant = instant});
    }

    private void DeselectRelationship(AlbumPageButtonUIRelationship relationship, bool instant)
    {
        if (relationship == null) return;

        if (instant)
        {
            relationship.albumPageButton.DeselectInstant();
            relationship.albumPageUI.DeselectInstant();
        }
        else
        {
            relationship.albumPageButton.Deselect();
            relationship.albumPageUI.Deselect();
        }

        OnAlbumRelationshipDeselected?.Invoke(this, new OnAlbumRelationshipSelectedEventArgs { relationship = relationship, instant = instant });
    }

    private bool IsValidRelationship(AlbumPageButtonUIRelationship relationship)
    {
        if( relationship == null) return false;
        if(relationship.albumPageButton == null) return false;
        if (relationship.albumPageUI == null) return false;

        return true;
    }
    #endregion


    #region Subscriptions
    private void AlbumPageButton_OnButtonClicked(object sender, AlbumPageButton.OnButtonClickedEventArgs e)
    {
        SelectRelationshipByButton(e.albumPageButton, false);
    }
    #endregion
}
