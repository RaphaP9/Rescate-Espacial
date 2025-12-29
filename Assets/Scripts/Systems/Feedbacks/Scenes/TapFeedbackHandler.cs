using UnityEngine;

public class TapFeedbackHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform feedbackContainer;
    [SerializeField] private Transform tapFeedbackPrefab;

    [Header("Debug")]
    [SerializeField] private bool debug;

    private void Update()
    {
        HandleTap();
    }

    private void HandleTap()
    {
        if (InputUtilities.TryGetTapPosition(out Vector2 screenTapPosition))
        {
            Vector2 tapPosition = screenTapPosition - InputUtilities.GetScreenDimensions() / 2;

            CreateTapVFX(tapPosition);
        }
    }

    private void CreateTapVFX(Vector2 position)
    {
        Transform tapVFXTransform = Instantiate(tapFeedbackPrefab, feedbackContainer);
        RectTransform tapVFXRectTransform = tapVFXTransform.GetComponent<RectTransform>();

        if (tapVFXRectTransform == null)
        {
            if (debug) Debug.Log("TapVFX does not contain a RectTransform");
            return;
        }

        tapVFXRectTransform.anchoredPosition = position;
    }
}
