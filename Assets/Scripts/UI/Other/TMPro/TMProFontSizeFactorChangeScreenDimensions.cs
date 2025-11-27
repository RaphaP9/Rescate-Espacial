using TMPro;
using UnityEngine;

public class TMProFontSizeFactorChangeScreenDimensions : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private TextMeshProUGUI affectedTMPro;

    [Header("Settings")]
    [SerializeField, Range(-100f, 100f)] private float sizeOffset;
    [SerializeField, Range(-100f, 100f)] private float sizeFactor;
    [Space]
    [SerializeField, Range(-100f, 100f)] private float minFontSize;

    private void Start()
    {
        ChangeRectPosY();
    }

    private void ChangeRectPosY()
    {
        float aspect = InputUtilities.GetScreenAspect();

        SetTMProTextSize(aspect);
    }

    private void SetTMProTextSize(float aspect)
    {
        float size = sizeOffset + aspect * sizeFactor;

        if(size < minFontSize) size = minFontSize;

        affectedTMPro.fontSize = size;
    }

    #region Subscriptions
    private void RectDimensionsChangeDetector_OnRectDimensionsChanged(object sender, System.EventArgs e)
    {
        ChangeRectPosY();
    }
    #endregion
}
