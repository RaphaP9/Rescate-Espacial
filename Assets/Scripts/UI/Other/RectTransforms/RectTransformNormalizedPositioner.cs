using UnityEngine;

public class RectTransformNormalizedPositioner : MonoBehaviour
{
    //NOTE: Make sure RectTransform has center anchors

    [Header("Components")]
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private RectDimensionsChangeDetector rectDimensionsChangeDetector;

    [Header("Settings")]
    [SerializeField] private Vector2 refferenceScreenDimensions;
    [SerializeField] private Vector2 refferencePositionDueToRefferenceScreenDimensions;

    private void OnEnable()
    {
        rectDimensionsChangeDetector.OnRectDimensionsChanged += RectDimensionsChangeDetector_OnRectDimensionsChanged;
    }

    private void OnDisable()
    {
        rectDimensionsChangeDetector.OnRectDimensionsChanged -= RectDimensionsChangeDetector_OnRectDimensionsChanged;
    }

    private void UpdateRectTransformPosition()
    {
        Vector2 screenDimensions = InputUtilities.GetScreenDimensions();
        rectTransform.anchoredPosition = screenDimensions * GetPositionFactor();
    }

    private Vector2 GetPositionFactor()
    {
        Vector2 positionFactor = new Vector2(refferencePositionDueToRefferenceScreenDimensions.x / refferenceScreenDimensions.x, refferencePositionDueToRefferenceScreenDimensions.y / refferenceScreenDimensions.y);
        return positionFactor;
    }

    private void RectDimensionsChangeDetector_OnRectDimensionsChanged(object sender, System.EventArgs e)
    {
        UpdateRectTransformPosition();
    }
}
