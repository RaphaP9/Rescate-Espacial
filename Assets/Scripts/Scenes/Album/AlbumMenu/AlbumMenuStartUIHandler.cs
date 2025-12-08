using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AlbumMenuStartUIHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private TextMeshProUGUI fractionText;
    [SerializeField] private Image starFillImage;

    [Header("Settings")]
    [SerializeField, Range(1, 10)] private int totalStories;

    [Header("Runtime Filled")]
    [SerializeField] private int uncoveredStories;

    private Material starMaterial;

    private const string NORMALIZED_VALUE_PROPERTY_NAME = "_NormalizedValue";
    private const string SLASH_CHARACTER = "/";

    private void Awake()
    {
        starMaterial = starFillImage.material;
    }

    private void Start()
    {
        GetTotalUncoveredStories();
        SetFractionText();
        SetStarMaterialFill();
    }

    private void GetTotalUncoveredStories()
    {
        uncoveredStories = DataContainer.Instance.GetCutscenesUnlockedCount();
    }

    private void SetFractionText()
    {
        fractionText.text = $"{uncoveredStories}{SLASH_CHARACTER}{totalStories}";
    }

    private void SetStarMaterialFill()
    {
        if (starMaterial == null) return;
        if (!starMaterial.HasFloat(NORMALIZED_VALUE_PROPERTY_NAME)) return;

        float normalizedFill = (float) uncoveredStories / totalStories;
        normalizedFill = Mathf.Clamp01(normalizedFill);

        starMaterial.SetFloat(NORMALIZED_VALUE_PROPERTY_NAME, normalizedFill);
    }
}
