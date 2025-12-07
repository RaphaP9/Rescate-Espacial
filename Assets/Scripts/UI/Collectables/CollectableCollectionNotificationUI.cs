using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Settings;
using UnityEngine.Video;
using System.Collections;

public class CollectableCollectionNotificationUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Image collectableImage;
    [SerializeField] private Image collectableBackground;
    [SerializeField] private TextMeshProUGUI collectableNameText;
    [SerializeField] private Animator animator;
    [Space]
    [SerializeField] private UIVerticalDragDetector UIVerticalDragDetector;

    [Header("Settings")]
    [SerializeField, Range(3f,10f)] private float timeShowing;
    [SerializeField] private bool useRealtime;

    [Header("Runtime Filled")]
    [SerializeField] private CollectableSO collectableSO;
    [SerializeField] private bool isShowing;

    private const string HIDE_TRIGGER = "Hide";

    private void OnEnable()
    {
        LocalizationSettings.SelectedLocaleChanged += LocalizationSettings_SelectedLocaleChanged;
        UIVerticalDragDetector.OnVerticalDragUp += UIVerticalDragDetector_OnVerticalDragUp;    
    }

    private void OnDisable()
    {
        LocalizationSettings.SelectedLocaleChanged -= LocalizationSettings_SelectedLocaleChanged;
        UIVerticalDragDetector.OnVerticalDragUp -= UIVerticalDragDetector_OnVerticalDragUp;
    }

    private void Start()
    {
        StartCoroutine(ShowCoroutine());
    }

    private IEnumerator ShowCoroutine()
    {
        if (useRealtime) yield return new WaitForSecondsRealtime(timeShowing);
        else yield return new WaitForSeconds(timeShowing);

        HideNotificationUI();
    }

    public void SetUI(CollectableSO collectableSO)
    {
        this.collectableSO = collectableSO;

        SetCollectableImage();
        SetCollectableBackground();

        LocalizeCollectableNameText();
    }

    private void SetCollectableImage()
    {
        if (collectableSO == null) return;

        collectableImage.sprite = collectableSO.collectableSprite;
    }

    private void SetCollectableBackground()
    {
        if (collectableSO == null) return;

        collectableBackground.color = collectableSO.collectedBackgroundColor;
    }


    private void LocalizeCollectableNameText()
    {
        if (collectableSO == null) return;

        collectableNameText.text = LocalizationSettings.StringDatabase.GetLocalizedString(collectableSO.localizationTable, collectableSO.nameLocalizationBinding);
    }

    private void HideNotificationUI()
    {
        if (!isShowing) return;
        animator.SetTrigger(HIDE_TRIGGER);
    }

    public void DestroyNotificationUI()
    {
        Destroy(gameObject);
    }

    public void SetIsShowingTrue() => isShowing = true;
    public void SetIsShowingFalse() => isShowing = false;

    #region Subscriptions
    private void LocalizationSettings_SelectedLocaleChanged(UnityEngine.Localization.Locale obj)
    {
        LocalizeCollectableNameText();
    }

    private void UIVerticalDragDetector_OnVerticalDragUp(object sender, System.EventArgs e)
    {
        HideNotificationUI();
    }
    #endregion
}
