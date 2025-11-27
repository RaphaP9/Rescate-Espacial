using System;
using UnityEngine;

public class RectTransformHeightFactorChange : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private RectTransform rectTransformRefference;
    [SerializeField] private RectTransform affectedRectTransform;

    [Header("Settings")]
    [SerializeField, Range(0.01f, 2f)] private float heightFactor;

    private void Start()
    {
        ChangeRectHeight();
    }

    private void ChangeRectHeight()
    {
        float height = GetNewHeight();
        SetRectTransformHeight(height);
    }

    private float GetNewHeight() => rectTransformRefference.rect.height * heightFactor;

    private void SetRectTransformHeight(float height)
    {
        Vector2 size = affectedRectTransform.sizeDelta;
        size.y = height;
        affectedRectTransform.sizeDelta = size;
    }
}
