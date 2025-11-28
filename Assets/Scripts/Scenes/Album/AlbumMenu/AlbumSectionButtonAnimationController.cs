using System.Collections;
using UnityEngine;

public class AlbumSectionButtonAnimationController : MonoBehaviour
{
    [Header("Animator")]
    [SerializeField] private Animator animator;

    [Header("Settings")]
    [SerializeField, Range(5f, 12f)] private float minInterval; 
    [SerializeField, Range(5f, 12f)] private float maxInterval;

    private const string SHAKE_TRIGGER = "Shake";

    private void Start()
    {
        StartCoroutine(ShakeCoroutine());
    }

    private IEnumerator ShakeCoroutine()
    {
        while (true)
        {
            float interval = GeneralUtilities.GetRandomBetweenTwoFloats(minInterval, maxInterval);
            yield return new WaitForSeconds(interval);

            Shake();
        }
    }

    private void Shake()
    {
        animator.SetTrigger(SHAKE_TRIGGER);
    }
}
