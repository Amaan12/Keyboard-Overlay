using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(BoxCollider2D))]
public class AdjustColliderBounds : MonoBehaviour
{
    private BoxCollider2D boxCollider;
    private RectTransform rectTransform;

    void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        rectTransform = GetComponent<RectTransform>();
    }

    void Start()
    {
        AdjustNow();
    }

    /// <summary>
    /// Adjusts the BoxCollider2D to match the RectTransform size and position.
    /// Call this manually when the UI element is resized or scaled.
    /// </summary>
    public void AdjustNow()
    {
        Vector2 size = rectTransform.rect.size;
        boxCollider.size = size;
        boxCollider.offset = rectTransform.rect.center;
    }
}
