using UnityEngine;

public class RectTransformPosYFactorChangeScreenAspect : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private RectTransform affectedRectTransform;

    [Header("Settings")]
    [SerializeField, Range(-1000f, 1000f)] private float posYOffset;
    [SerializeField, Range(-1000f, 1000f)] private float posYFactor;

    private void Start()
    {
        ChangeRectPosY();
    }

    private void ChangeRectPosY()
    {
        float aspect = InputUtilities.GetScreenAspect();

        SetRectTransformPosY(aspect);
    }

    private void SetRectTransformPosY(float aspect)
    {
        float posY = posYOffset + aspect * posYFactor;

        Vector2 position = affectedRectTransform.anchoredPosition;
        position.y = posY;
        affectedRectTransform.anchoredPosition = position;
    }

    #region Subscriptions
    private void RectDimensionsChangeDetector_OnRectDimensionsChanged(object sender, System.EventArgs e)
    {
        ChangeRectPosY();
    }
    #endregion
}
