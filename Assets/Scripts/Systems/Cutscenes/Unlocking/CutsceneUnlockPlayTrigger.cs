using UnityEngine;

public class CutsceneUnlockPlayTrigger : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private bool triggerCutsceneOnUnlock;

    private void OnEnable()
    {
        CutsceneUnlockHandler.OnCutsceneUnlock += CutsceneUnlockHandler_OnCutsceneUnlock;
    }

    private void OnDisable()
    {
        CutsceneUnlockHandler.OnCutsceneUnlock -= CutsceneUnlockHandler_OnCutsceneUnlock;
    }

    private void PlayCutscene(CutsceneSO cutsceneSO) => RegularSceneCutsceneUI.Instance.PlayCutscene(cutsceneSO);

    private void CutsceneUnlockHandler_OnCutsceneUnlock(object sender, CutsceneUnlockHandler.OnCutsceneUnlockEventArgs e)
    {
        if (!triggerCutsceneOnUnlock) return;
        PlayCutscene(e.cutsceneSO);
    }
}
