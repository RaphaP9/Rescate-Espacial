using UnityEngine;

public class ComboGainedMinigameFeedbackHandler : InstancedFeedbackHandler
{
    [Header("Settings")]
    [SerializeField, Range(2, 5)] private int targetCombo;

    private void OnEnable()
    {
        MinigameScoreManager.OnComboGained += MinigameScoreManager_OnComboGained;
        MinigameScoreManager.OnComboUpdated += MinigameScoreManager_OnComboUpdated;
    }

    private void OnDisable()
    {
        MinigameScoreManager.OnComboGained -= MinigameScoreManager_OnComboGained;
        MinigameScoreManager.OnComboUpdated -= MinigameScoreManager_OnComboUpdated;
    }

    private void CheckTargetCombo(int combo)
    {
        if (combo != targetCombo) return;

        CreateFeedback();
    }

    #region Subscriptions
    private void MinigameScoreManager_OnComboGained(object sender, MinigameScoreManager.OnComboGainedEventArgs e)
    {
        CheckTargetCombo(e.comboGained);
    }

    private void MinigameScoreManager_OnComboUpdated(object sender, MinigameScoreManager.OnComboGainedEventArgs e)
    {
        CheckTargetCombo(e.comboGained);
    }
    #endregion
}
