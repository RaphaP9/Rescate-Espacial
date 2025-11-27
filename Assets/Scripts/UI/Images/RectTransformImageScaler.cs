using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RectTransformImageScaler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private RectDimensionsChangeDetector rectDimensionsChangeDetector;

    [SerializeField] private RectTransform rectTransformRefference;
    [SerializeField] private Image image;
    [Space]
    [SerializeField] private RectTransform affectedRectTransform;

    private void OnEnable()
    {
        rectDimensionsChangeDetector.OnRectDimensionsChanged += RectDimensionsChangeDetector_OnRectDimensionsChanged;
    }

    private void OnDisable()
    {
        rectDimensionsChangeDetector.OnRectDimensionsChanged -= RectDimensionsChangeDetector_OnRectDimensionsChanged;
    }

    private void Start()
    {
        HandleRectTransformScaling();
    }

    private void HandleRectTransformScaling()
    {
        if (image.sprite == null) return;

        Vector2 rectTransformDimensions = GetRectTransformDimensions();
        Vector2 imageDimensions = GetImageDimensions();

        if (rectTransformDimensions.x <= 0 || rectTransformDimensions.y <= 0) return;

        Vector2 neededProportions = new Vector2(imageDimensions.x / rectTransformDimensions.x, imageDimensions.y / rectTransformDimensions.y);

        Vector2 scalingProportions = GeneralUtilities.ScaleVectorTilMinComponentIsOne(neededProportions);

        affectedRectTransform.localScale = scalingProportions;
    }

    private Vector2 GetRectTransformDimensions() => new Vector2(rectTransformRefference.rect.width, rectTransformRefference.rect.height);
    private Vector2 GetImageDimensions() => new Vector2(image.sprite.rect.width, image.sprite.rect.height);

    #region Subscriptions
    private void RectDimensionsChangeDetector_OnRectDimensionsChanged(object sender, System.EventArgs e)
    {
        HandleRectTransformScaling();
    }
    #endregion
}
