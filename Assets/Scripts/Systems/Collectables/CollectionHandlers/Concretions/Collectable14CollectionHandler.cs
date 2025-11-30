using System;
using System.Numerics;
using UnityEngine;

public class Collectable14CollectionHandler : CollectableCollectionHandler
{
    //Collectable that checks if you won the Silhouettes Minigame (Complete the fifth round)
    [Header("Settings")]
    [SerializeField, Range(2, 10)] private int targetRoundNumber;

    private void OnEnable()
    {
        SilhouettesMinigameManager.OnSilhouettesRoundEnd += SilhouettesMinigameManager_OnSilhouettesRoundEnd;
    }

    private void OnDisable()
    {
        SilhouettesMinigameManager.OnSilhouettesRoundEnd -= SilhouettesMinigameManager_OnSilhouettesRoundEnd;
    }

    private void SilhouettesMinigameManager_OnSilhouettesRoundEnd(object sender, SilhouettesMinigameManager.OnSilhouettesRoundEventArgs e)
    {
        if (e.roundIndex + 1 < targetRoundNumber) return; //Round index is 1 less than round number
        CollectCollectable(false);
    }
}
