using UnityEngine;

public class RightDisplacementArrow : DisplacementArrow
{
    private void OnEnable()
    {
        SnappingScrollMenuUI.OnLastItemReached += SnappingScrollMenuUI_OnLastItemReached;
        SnappingScrollMenuUI.OnLastItemAway += SnappingScrollMenuUI_OnLastItemAway;
    }

    private void OnDisable()
    {
        SnappingScrollMenuUI.OnLastItemReached -= SnappingScrollMenuUI_OnLastItemReached;
        SnappingScrollMenuUI.OnLastItemAway -= SnappingScrollMenuUI_OnLastItemAway;
    }

    protected override void ArrowDisplacement() => snappingScrollMenuUI.DisplaceRightCommand();

    private void SnappingScrollMenuUI_OnLastItemReached(object sender, SnappingScrollMenuUI.OnItemEventArgs e)
    {
        if (e.instantly) HideUIImmediately();
        else HideUI();
    }

    private void SnappingScrollMenuUI_OnLastItemAway(object sender, SnappingScrollMenuUI.OnItemEventArgs e)
    {
        if (e.instantly) ShowUIImmediately();
        else ShowUI();
    }
}
