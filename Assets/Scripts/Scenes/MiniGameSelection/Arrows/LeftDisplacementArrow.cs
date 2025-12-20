using UnityEngine;

public class LeftDisplacementArrow : DisplacementArrow
{
    private void OnEnable()
    {
        SnappingScrollMenuUI.OnFirstItemReached += SnappingScrollMenuUI_OnFirstItemReached;
        SnappingScrollMenuUI.OnFirstItemAway += SnappingScrollMenuUI_OnFirstItemAway;
    }

    private void OnDisable()
    {
        SnappingScrollMenuUI.OnFirstItemReached -= SnappingScrollMenuUI_OnFirstItemReached;
        SnappingScrollMenuUI.OnFirstItemAway -= SnappingScrollMenuUI_OnFirstItemAway;
    }

    protected override void ArrowDisplacement() => snappingScrollMenuUI.DisplaceLeftCommand();

    private void SnappingScrollMenuUI_OnFirstItemReached(object sender, SnappingScrollMenuUI.OnItemEventArgs e)
    {
        if (e.instantly) HideUIImmediately();
        else HideUI();
    }

    private void SnappingScrollMenuUI_OnFirstItemAway(object sender, SnappingScrollMenuUI.OnItemEventArgs e)
    {
        if (e.instantly) ShowUIImmediately();
        else ShowUI();
    }
}
