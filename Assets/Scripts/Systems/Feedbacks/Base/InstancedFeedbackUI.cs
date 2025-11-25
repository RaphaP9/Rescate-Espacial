using System;
using System.Collections;
using UnityEngine;

public class InstancedFeedbackUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator animator;

    [Header("Settings")]
    [SerializeField, Range(2f,7f)] private float lifetime;

    private bool isConcluding = false;

    public event EventHandler OnFeedbackEnd;

    private const string CONCLUDE_TRIGGER = "Conclude";

    private void Start()
    {
        StartCoroutine(LifetimeCoroutine());
    }

    private IEnumerator LifetimeCoroutine()
    {
        yield return new WaitForSeconds(lifetime);
        ConcludeFeedback();
    }

    public void ConcludeFeedback()
    {
        if (isConcluding) return;

        isConcluding = true;
        StopAllCoroutines();

        OnFeedbackEnd?.Invoke(this, EventArgs.Empty);
        animator.SetTrigger(CONCLUDE_TRIGGER);
    }

    public void DestroyFeedback()
    {
        Destroy(gameObject);
    }
}
