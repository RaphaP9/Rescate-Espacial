using System.Collections;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class FinalScoreAudioHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private FinalScoreUI finalScoreUI;
    [SerializeField] private AudioSource audioSource;

    [Header("Settings")]
    [SerializeField, Range(0f, 2f)] private float timeToPlayAudio;

    [Header("Debug")]
    [SerializeField] private bool debug;

    private void OnEnable()
    {
        finalScoreUI.OnFinalScoreUIShow += FinalScoreUI_OnFinalScoreUIShow;
    }

    private void OnDisable()
    {
        finalScoreUI.OnFinalScoreUIShow -= FinalScoreUI_OnFinalScoreUIShow;
    }

    private IEnumerator PlayAudioCoroutine(MinigameFinalScoreSetting minigameFinalScoreSetting)
    {
        if (minigameFinalScoreSetting.finalScoreLocalizedAudioclipSettings.Count <= 0) yield break;

        FinalScoreLocalizedAudioClipSetting setting = GeneralUtilities.ChooseRandomElementFromList(minigameFinalScoreSetting.finalScoreLocalizedAudioclipSettings);

        AsyncOperationHandle<AudioClip> handle = LocalizationSettings.AssetDatabase.GetLocalizedAssetAsync<AudioClip>(setting.localizationTable, setting.localizationBinding);

        yield return handle;

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            AudioClip clip = handle.Result;

            yield return new WaitForSeconds(timeToPlayAudio);

            PlayAudio(clip);
        }
        else
        {
            if (debug) Debug.Log("Async Operation Failed");
        }
    }

    private void PlayAudio(AudioClip clip) => audioSource.PlayOneShot(clip);

    #region Subscriptions
    private void FinalScoreUI_OnFinalScoreUIShow(object sender, FinalScoreUI.OnFinalScoreUIShowEventArgs e)
    {
        StartCoroutine(PlayAudioCoroutine(e.minigameFinalScoreSetting));
    }
    #endregion
}
