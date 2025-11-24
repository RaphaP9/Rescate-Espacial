using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using System.Collections.Generic;
using System.Collections;

public class CutscenePanelSentenceHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator animator;
    [SerializeField] private TextMeshProUGUI sentenceText;

    [Header("Settings")]
    [SerializeField] private bool hasSentence;
    [SerializeField] private string sentenceLocalizationTable;
    [Space]
    [SerializeField] private bool hideLastSentence;

    [Header("Lists")]
    [SerializeField] private List<CutscenePanelSentence> sentences;

    private const string SHOW_TRIGGER = "Show";
    private const string HIDE_TRIGGER = "Hide";

    private bool sentenceShowConcludedFlag = false;
    private bool sentenceHideConcludedFlag = false;

    private CutscenePanelSentence currentSentence;

    [System.Serializable]
    public class CutscenePanelSentence
    {
        public string sentenceLocalizationBinding;
        [Range(0f, 15f)] public float timeToShowSentence;
        [Range(0f, 15f)] public float timeShowingSentence;
        [ColorUsage(true, true)] public Color sentenceColor;
    }


    private void OnEnable()
    {
        LocalizationSettings.SelectedLocaleChanged += LocalizationSettings_SelectedLocaleChanged;
    }

    private void OnDisable()
    {
        LocalizationSettings.SelectedLocaleChanged -= LocalizationSettings_SelectedLocaleChanged;
    }

    private void Start()
    {
        StartCoroutine(SentencesCoroutine());
    }

    private IEnumerator SentencesCoroutine()
    {
        if(!hasSentence) yield break;

        foreach(CutscenePanelSentence sentence in sentences)
        {
            currentSentence = sentence;

            SetLocalizedSentence(sentence.sentenceLocalizationBinding);
            SetSentenceColor(sentence.sentenceColor);

            yield return new WaitForSeconds(sentence.timeToShowSentence);

            ShowSentence();

            if (IsLastSentence(sentence) && !hideLastSentence) yield break;

            yield return new WaitUntil(() => sentenceShowConcludedFlag);
            sentenceShowConcludedFlag = false;

            yield return new WaitForSeconds(sentence.timeShowingSentence);

            HideSentence();

            yield return new WaitUntil(() => sentenceHideConcludedFlag);
            sentenceHideConcludedFlag = false;
        }
    }

    #region Animations
    private void ShowSentence()
    {
        animator.ResetTrigger(HIDE_TRIGGER);
        animator.SetTrigger(SHOW_TRIGGER);
    }

    private void HideSentence()
    {
        animator.ResetTrigger(SHOW_TRIGGER);
        animator.SetTrigger(HIDE_TRIGGER);
    }
    #endregion

    private void SetSentenceColor(Color color) => sentenceText.color = color;
    private void SetLocalizedSentence(string localizationBinding) => sentenceText.text = LocalizationSettings.StringDatabase.GetLocalizedString(sentenceLocalizationTable, localizationBinding);

    private bool IsLastSentence(CutscenePanelSentence sentence)
    {
        return sentence == sentences[^1];
    }

    public void SetSentenceShowConcludedFlagTrue() => sentenceShowConcludedFlag = true;
    public void SetSentenceHideConcludedFlagTrue() => sentenceHideConcludedFlag = true;

    #region Subsctiptions
    private void LocalizationSettings_SelectedLocaleChanged(Locale locale)
    {
        if (currentSentence == null) return;
        SetLocalizedSentence(currentSentence.sentenceLocalizationBinding);
    }
    #endregion
}
