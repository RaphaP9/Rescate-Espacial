using UnityEngine;

public class RectTransformPosYFactorChange : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private RectTransform rectTransformRefference;
    [SerializeField] private RectTransform affectedRectTransform;

    [Header("Settings")]
    [SerializeField, Range(-2f, 2f)] private float posYFactor;

    private void Start()
    {
        ChangeRectPosY();
    }

    private void ChangeRectPosY()
    {
        float posY = GetNewPosY();
        SetRectTransformPosY(posY);
    }

    private float GetNewPosY() => rectTransformRefference.rect.height * posYFactor;

    private void SetRectTransformPosY(float posY)
    {
        Vector2 position = affectedRectTransform.anchoredPosition;
        position.y = posY;
        affectedRectTransform.anchoredPosition = position;
    }
}
