using System.Collections;
using UnityEngine;

public class AlbumStarGroupAnimationController : MonoBehaviour
{
    [Header("Animator")]
    [SerializeField] private Animator animator;

    [Header("Settings")]
    [SerializeField] private bool enableShake;
    [Space]
    [SerializeField, Range(2f, 10f)] private float minStartingTime;
    [SerializeField, Range(2f, 10f)] private float maxStartingTime;
    [Space]
    [SerializeField, Range(2f, 10f)] private float minInterval;
    [SerializeField, Range(2f, 10f)] private float maxInterval;

    private const string SHAKE_TRIGGER = "Shake";

    private void Start()
    {
        StartCoroutine(ShakeCoroutine());
    }

    private IEnumerator ShakeCoroutine()
    {
        float startingTime = GeneralUtilities.GetRandomBetweenTwoFloats(minStartingTime, maxStartingTime);

        yield return new WaitForSeconds(startingTime);

        Shake();

        while (true)
        {
            float interval = GeneralUtilities.GetRandomBetweenTwoFloats(minInterval, maxInterval);
            yield return new WaitForSeconds(interval);

            Shake();
        }
    }

    private void Shake()
    {
        if (!enableShake) return;
        animator.SetTrigger(SHAKE_TRIGGER);
    }
}
