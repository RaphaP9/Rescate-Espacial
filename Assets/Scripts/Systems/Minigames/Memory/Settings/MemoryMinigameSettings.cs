using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "MemoryMinigameSettingsSO", menuName = "ScriptableObjects/Minigames/Memory/Settings")]
public class MemoryMinigameSettings : ScriptableObject
{
    [Header("Rounds Settings")]
    public List<MemoryRound> rounds;

    [Header("Score Settings")]
    [Range(1,10)] public int baseScorePerPairMatch;
    [Range(2,10)] public int minCombo;
    [Range(2,10)] public int maxCombo;
    [Range(1,10)] public int bonusScorePerCombo;

    [Header("Time Settings")]
    [SerializeField, Range(0f, 600f)] public float gameTime;
    [SerializeField, Range(0f, 10f)] public float timeBonusOnMatch;
    [SerializeField, Range(0f, 10f)] public float timePenaltyOnFail;

    [Header("Low Level Timers")]
    [Range(0f, 5f)] public float startingGameTime;
    [Range(0f, 5f)] public float cardRevealInputCooldown;
    [Range(0f, 5f)] public float pairProcessingTime; //pairProcessing is the same duration as FlipRevealAnimation (or at least that value)
    [Range(0f, 5f)] public float timeBetweenPairs;
    [Space]
    [Range(0f, 5f)] public float allPairsMatchTime;
    [Range(0f, 5f)] public float switchRoundTimer;
    [Range(0f, 5f)] public float endLastRoundTimer;
    [Range(0f, 5f)] public float endingGameTime;

    [Header("Other Settings")]
    public bool wairForCardReveal; //Makes cards can't be revealed while a card is being fliped Reveal
    public bool waitForPairFail; //Makes cards can't be revealed while a pair is failing
}


