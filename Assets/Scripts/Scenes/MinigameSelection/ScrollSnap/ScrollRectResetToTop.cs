using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScrollRectResetToTop : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private ScrollRect scrollRect;

    private const float TOP_SCROLL_RECT_VERTICAL_NORMALIZED_POSITION = 1f;

    private void Start()
    {
        StartCoroutine(ResetScrollRectToTopCoroutine());
    }

    private IEnumerator ResetScrollRectToTopCoroutine()
    {
        yield return null;

        ResetScrollRectToTop();
    }

    private void ResetScrollRectToTop()
    {
        scrollRect.verticalNormalizedPosition = TOP_SCROLL_RECT_VERTICAL_NORMALIZED_POSITION;
    }
}
