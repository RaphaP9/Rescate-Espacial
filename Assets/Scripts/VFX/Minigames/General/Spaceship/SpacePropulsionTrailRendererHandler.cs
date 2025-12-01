using UnityEngine;
using UnityEngine.VFX;
using System.Collections;

public class SpacePropulsionTrailRendererHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private TrailRenderer trailRenderer;

    [Header("Settings")]
    [SerializeField, Range(0.1f, 5f)] private float trailActiveTime;
    [Space]
    [SerializeField, Range(0f, 5f)] private float colorIntensity;

    private Material trailMaterial;

    private const string COLOR_PROPERTY_NAME = "_Color";

    private void OnEnable()
    {
        MinigameManager.OnRoundEnd += MinigameManager_OnRoundEnd;
    }

    private void OnDisable()
    {
        MinigameManager.OnRoundEnd -= MinigameManager_OnRoundEnd;
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

    private IEnumerator PlayTrailCoroutine()
    {
        PlayTrail();

        yield return new WaitForSeconds(trailActiveTime);

        StopTrail();
    }

    private void MinigameManager_OnRoundEnd(object sender, MinigameManager.OnRoundEventArgs e)
    {
        StopAllCoroutines();
        StartCoroutine(PlayTrailCoroutine());
    }
}
