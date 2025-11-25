using UnityEngine;

public class CutsceneUnlockedFeedbackHandler : InstancedFeedbackHandler
{
    private void OnEnable()
    {
        CutsceneUnlockHandler.OnCutsceneUnlock += CutsceneUnlockHandler_OnCutsceneUnlock;
    }

    private void OnDisable()
    {
        CutsceneUnlockHandler.OnCutsceneUnlock -= CutsceneUnlockHandler_OnCutsceneUnlock;
    }

    private void CutsceneUnlockHandler_OnCutsceneUnlock(object sender, CutsceneUnlockHandler.OnCutsceneUnlockEventArgs e)
    {
        CreateFeedback();
    }
}
