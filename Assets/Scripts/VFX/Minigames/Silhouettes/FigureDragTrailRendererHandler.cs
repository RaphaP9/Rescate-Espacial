using UnityEngine;

public class FigureDragTrailRendererHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private FigureHandler figureHandler;

    [Header("Settings")]
    [SerializeField, Range(0f, 5f)] private float colorIntensity;

    private Material trailMaterial;

    private const string COLOR_PROPERTY_NAME = "_Color";

    private void OnEnable()
    {
        figureHandler.OnThisFigureDragStart += FigureHandler_OnThisFigureDragStart;
        figureHandler.OnThisFigureDragEnd += FigureHandler_OnThisFigureDragEnd;
    }

    private void OnDisable()
    {
        figureHandler.OnThisFigureDragStart -= FigureHandler_OnThisFigureDragStart;
        figureHandler.OnThisFigureDragEnd -= FigureHandler_OnThisFigureDragEnd;
    }

    private void Awake()
    {
        trailMaterial = trailRenderer.material;
    }

    private void Start()
    {
        StopTrail();
        SetGradientColorIntensity();
    }

    private void PlayTrail() => trailRenderer.emitting = true;
    private void StopTrail() => trailRenderer.emitting = false;

    private void SetGradientColorIntensity()
    {
        if (!trailMaterial.HasColor(COLOR_PROPERTY_NAME)) return;

        Color baseColor = trailMaterial.GetColor(COLOR_PROPERTY_NAME);
        Color normalizedColor = new Color(
            baseColor.r / Mathf.Max(baseColor.r, baseColor.g, baseColor.b, 1f),
            baseColor.g / Mathf.Max(baseColor.r, baseColor.g, baseColor.b, 1f),
            baseColor.b / Mathf.Max(baseColor.r, baseColor.g, baseColor.b, 1f),
            baseColor.a
        );

        trailMaterial.SetColor(COLOR_PROPERTY_NAME, normalizedColor * colorIntensity);
    }

    private void FigureHandler_OnThisFigureDragStart(object sender, System.EventArgs e)
    {
        PlayTrail();
    }

    private void FigureHandler_OnThisFigureDragEnd(object sender, System.EventArgs e)
    {
        StopTrail();
    }
}
