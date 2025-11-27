using UnityEngine;

public class RectTransformTopBottomFactorChange : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private RectTransform rectTransformRefference;
    [SerializeField] private RectTransform affectedRectTransform;

    [Header("Settings")]
    [SerializeField, Range(0.01f, 2f)] private float topFactor;
    [SerializeField, Range(0.01f, 2f)] private float bottomFactor;

    private void Start()
    {
        ChangeRectTop();
        ChangeRectBottom();
    }

    #region Top
    private void ChangeRectTop()
    {
        float top = GetNewTop();
        SetRectTransformTop(top);
    }

    private float GetNewTop() => rectTransformRefference.rect.height * topFactor;

    private void SetRectTransformTop(float top)
    {
        Vector2 max = affectedRectTransform.offsetMax;
        max.y = -top;//Negative for Top
        affectedRectTransform.offsetMax = max;
    }
    #endregion

    #region Bottom
    private void ChangeRectBottom()
    {
        float bottom = GetNewBottom();
        SetRectTransformBottom(bottom);
    }

    private float GetNewBottom() => rectTransformRefference.rect.height * bottomFactor;

    private void SetRectTransformBottom(float bottom)
    {
        Vector2 min = affectedRectTransform.offsetMin;
        min.y = bottom;//Positive for Top
        affectedRectTransform.offsetMin = min;
    }
    #endregion
}
