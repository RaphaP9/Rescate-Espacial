using System;
using UnityEngine;

public class Collectable8CollectionHandler : CollectableCollectionHandler
{
    //Collectable that checks if you won the Memory Minigame (Complete the fifth round)
    [Header("Settings")]
    [SerializeField, Range(2, 10)] private int targetRoundNumber;

    private void OnEnable()
    {
        MemoryMinigameManager.OnMemoryRoundEnd += MemoryMinigameManager_OnMemoryRoundEnd;
    }

    private void OnDisable()
    {
        MemoryMinigameManager.OnMemoryRoundEnd -= MemoryMinigameManager_OnMemoryRoundEnd;
    }

    private void MemoryMinigameManager_OnMemoryRoundEnd(object sender, MemoryMinigameManager.OnMemoryRoundEventArgs e)
    {
        if (e.roundIndex + 1 < targetRoundNumber) return; //Round index is 1 less than round number
        CollectCollectable(false);
    }
}
