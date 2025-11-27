using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class CutscenePanelSFXHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private AudioSource cutsceneSFXSource;

    [Header("Lists")]
    [SerializeField] private List<CutsceneSFXSetting> cutsceneSFXSettingList;

    [System.Serializable]
    public class CutsceneSFXSetting
    {
        public AudioClip SFXClip;
        [Range(0.1f, 10f)] public float timeToPlay;
    }

    protected virtual void OnEnable()
    {
        PauseManager.OnGamePaused += PauseManager_OnGamePaused;
        PauseManager.OnGameResumed += PauseManager_OnGameResumed;
    }

    protected virtual void OnDisable()
    {
        PauseManager.OnGamePaused -= PauseManager_OnGamePaused;
        PauseManager.OnGameResumed -= PauseManager_OnGameResumed;
    }

    private void Start()
    {
        StartSFXCoroutines();
    }

    private void StartSFXCoroutines()
    {
        foreach(CutsceneSFXSetting cutsceneSFXSetting in cutsceneSFXSettingList)
        {
            StartCoroutine(CutsceneSFXCoroutine(cutsceneSFXSetting.SFXClip, cutsceneSFXSetting.timeToPlay));
        }
    }

    public void TerminateSFXHandler()
    {
        //As all SFX are PlayOneShot, we must mute the audioSource to stop them
        MuteSFXHandler();
    }

    private void MuteSFXHandler()
    {
        cutsceneSFXSource.mute = true;
    }

    private IEnumerator CutsceneSFXCoroutine(AudioClip sfxClip, float timeToPlay)
    {
        yield return new WaitForSeconds(timeToPlay);
        cutsceneSFXSource.PlayOneShot(sfxClip);
    }

    private void PauseManager_OnGamePaused(object sender, System.EventArgs e)
    {
        cutsceneSFXSource.Pause();
    }

    private void PauseManager_OnGameResumed(object sender, System.EventArgs e)
    {
        cutsceneSFXSource.UnPause();
    }
}
