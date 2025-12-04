using System;
using System.Collections;
using UnityEngine;

public class DisplacementArrowMovementAnimationController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator animator;

    [Header("Settings")]
    [SerializeField] private bool enableMovement;
    [Space]
    [SerializeField] private string animationTrigger;
    [Space]
    [SerializeField, Range(2f, 10f)] private float minStartingTime;
    [SerializeField, Range(2f, 10f)] private float maxStartingTime;
    [Space]
    [SerializeField, Range(2f, 10f)] private float minInterval;
    [SerializeField, Range(2f, 10f)] private float maxInterval;

    private void Start()
    {
        StartCoroutine(MovementCoroutine());
    }

    private IEnumerator MovementCoroutine()
    {
        float startingTime = GeneralUtilities.GetRandomBetweenTwoFloats(minStartingTime, maxStartingTime);

        yield return new WaitForSeconds(startingTime);

        Move();

        while (true)
        {
            float interval = GeneralUtilities.GetRandomBetweenTwoFloats(minInterval, maxInterval);

            yield return new WaitForSeconds(interval);

            Move();
        }
    }

    private void Move()
    {
        if (!enableMovement) return;
        animator.SetTrigger(animationTrigger);
    }
}
