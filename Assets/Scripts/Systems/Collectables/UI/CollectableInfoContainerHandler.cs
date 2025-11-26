using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization;

public class CollectableInfoContainerHandler : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private Image collectableImage;
    [SerializeField] private Image collectableBackground;
    [SerializeField] private TextMeshProUGUI collectableNameText;
    [SerializeField] private TextMeshProUGUI collectableDescriptionText;

    [Header("Settings")]
    [SerializeField] private bool selectFirstCollectableOnContainerPopulation; 

    [Header("Localization Settings")]
    [SerializeField] private string stringLocalizationTable;
    [SerializeField] private string notCollectedNameLocalizationBinding;

    [Header("Runtime Filled")]
    [SerializeField] private CollectableUI currentCollectableUI;

    private void OnEnable()
    {
        CollectableContainerHandler.OnCollectablesPopulated += CollectableContainerPopulatorHandler_OnCollectablesPopulated;
        CollectableUI.OnCollectableUIClicked += CollectableUI_OnCollectableUIClicked;
        LocalizationSettings.SelectedLocaleChanged += LocalizationSettings_SelectedLocaleChanged;
    }

    private void OnDisable()
    {
        CollectableContainerHandler.OnCollectablesPopulated -= CollectableContainerPopulatorHandler_OnCollectablesPopulated;
        CollectableUI.OnCollectableUIClicked -= CollectableUI_OnCollectableUIClicked;
        LocalizationSettings.SelectedLocaleChanged -= LocalizationSettings_SelectedLocaleChanged;
    }

    private void UpdateSelectedCollectable(CollectableUI collectableUI, bool instant)
    {
        if (currentCollectableUI == collectableUI) return; //AlreadySelected

        if (currentCollectableUI != null) currentCollectableUI.DeselectCollectable(instant);

        currentCollectableUI = collectableUI;

        currentCollectableUI.SelectCollectable(instant);

        SetImageByCurrentCollectable();
        SetNameTextByCurrentCollectable();
        SetDescriptionTextByCurrentCollectable();
    }

    private void SetNameTextByCurrentCollectable()
    {
        if (currentCollectableUI == null) return;

        if (currentCollectableUI.IsCollected) collectableNameText.text = LocalizationSettings.StringDatabase.GetLocalizedString(currentCollectableUI.CollectableSO.localizationTable, currentCollectableUI.CollectableSO.nameLocalizationBinding);
        else collectableNameText.text = LocalizationSettings.StringDatabase.GetLocalizedString(stringLocalizationTable, notCollectedNameLocalizationBinding);
    }

    private void SetDescriptionTextByCurrentCollectable()
    {
        if (currentCollectableUI == null) return;

        collectableDescriptionText.text = LocalizationSettings.StringDatabase.GetLocalizedString(currentCollectableUI.CollectableSO.localizationTable, currentCollectableUI.CollectableSO.descriptionLocalizationBinding);
    }

    private void SetImageByCurrentCollectable()
    {
        if (currentCollectableUI == null) return;

        collectableImage.sprite = currentCollectableUI.CollectableSO.collectableSprite;

        if (currentCollectableUI.IsCollected)
        {
            collectableImage.material = null;
            collectableBackground.color = currentCollectableUI.CollectableSO.collectedBackgroundColor;
        }
        else
        {
            collectableImage.material = currentCollectableUI.CollectableSO.notCollectedMaterial;
            collectableBackground.color = currentCollectableUI.CollectableSO.notCollectedBackgroundColor;
        }
    }

    #region Subscriptions
    private void CollectableContainerPopulatorHandler_OnCollectablesPopulated(object sender, CollectableContainerHandler.OnCollectablesPopulatedEventArgs e)
    {
        if (!selectFirstCollectableOnContainerPopulation) return;
        if (e.collectableUIList.Count <= 0) return;

        UpdateSelectedCollectable(e.collectableUIList[0], true); //Index = 0, First from list
    }

    private void CollectableUI_OnCollectableUIClicked(object sender, CollectableUI.OnCollectableUIEventArgs e)
    {
        UpdateSelectedCollectable(e.collectableUI, false);
    }

    private void LocalizationSettings_SelectedLocaleChanged(Locale locale)
    {
        SetNameTextByCurrentCollectable();
        SetDescriptionTextByCurrentCollectable();
    }

    #endregion
}
