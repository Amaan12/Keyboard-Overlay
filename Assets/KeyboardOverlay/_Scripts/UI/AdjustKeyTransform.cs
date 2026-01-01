using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class AdjustKeyTransform : MonoBehaviour
{
    private RectTransform rectTransform;
    private BoxCollider2D boxCollider;

    private bool isDragging = false;
    private Vector2 offset;

    public static float snapValue = 10f;
    public static float scaleStep = 10f; // in pixels per scroll tick
    private static AdjustKeyTransform currentlyDragging;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if (!OverlayEditModeToggle.editMode)
            return;

        Vector2 mousePos = Input.mousePosition;

        // --- Scroll to scale ---
        if (IsMouseOver(rectTransform, mousePos))
        {
            float scroll = Input.mouseScrollDelta.y;
            if (Mathf.Abs(scroll) > 0.01f)
            {
                Vector2 size = rectTransform.sizeDelta;

                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                {
                    // vertical size
                    size.y += Mathf.Sign(scroll) * scaleStep;
                }
                else if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
                {
                    // uniform size
                    size.x += Mathf.Sign(scroll) * scaleStep;
                    size.y += Mathf.Sign(scroll) * scaleStep;
                }
                else
                {
                    // horizontal size
                    size.x += Mathf.Sign(scroll) * scaleStep;
                }

                // Clamp to avoid disappearing or huge sizes
                size.x = Mathf.Clamp(size.x, 10f, 1000f);
                size.y = Mathf.Clamp(size.y, 10f, 1000f);

                rectTransform.sizeDelta = size;

                // Match collider
                if (boxCollider)
                    boxCollider.size = size;
            }
        }

        // --- Begin drag ---
        if (Input.GetMouseButtonDown(0))
        {
            if (currentlyDragging != null && currentlyDragging != this)
                return;

            if (IsMouseOver(rectTransform, mousePos))
            {
                isDragging = true;
                currentlyDragging = this;
                offset = (Vector2)rectTransform.position - mousePos;
            }
        }

        // --- End drag ---
        if (Input.GetMouseButtonUp(0))
        {
            if (isDragging)
            {
                isDragging = false;
                currentlyDragging = null;

                Vector3 snapped = rectTransform.position;
                snapped.x = Mathf.Round(snapped.x / snapValue) * snapValue;
                snapped.y = Mathf.Round(snapped.y / snapValue) * snapValue;
                rectTransform.position = snapped;
            }
        }

        // --- While dragging ---
        if (isDragging)
        {
            rectTransform.position = mousePos + offset;
        }
    }

    private bool IsMouseOver(RectTransform rect, Vector2 screenPos)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rect,
            screenPos,
            null,
            out Vector2 localPoint
        );
        return rect.rect.Contains(localPoint);
    }
}
