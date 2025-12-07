using UnityEngine;
using UnityEngine.UI;

public class UIRawImageScroller : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Vector2 speed; // units/sec for U and V

    private RawImage rawImage;
    private Rect uvRect;

    private void Awake()
    {
        rawImage = GetComponent<RawImage>();
        uvRect = rawImage.uvRect;
    }

    private void Update()
    {
        HandleScrolling();
    }

    private void HandleScrolling()
    {
        uvRect.x += speed.x * Time.deltaTime;
        uvRect.y += speed.y * Time.deltaTime;

        // Keep values small with Repeat
        uvRect.x = Mathf.Repeat(uvRect.x, 1f);
        uvRect.y = Mathf.Repeat(uvRect.y, 1f);

        rawImage.uvRect = uvRect;
    }
}