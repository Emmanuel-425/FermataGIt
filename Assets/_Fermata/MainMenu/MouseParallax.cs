using UnityEngine;

public class UIParallax : MonoBehaviour
{
    public float moveAmount = 20f;
    public float smoothSpeed = 5f;

    private RectTransform rect;
    private Canvas canvas;
    private Vector2 startPos;

    void Start()
    {
        rect = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();

        startPos = rect.anchoredPosition;
    }

    void Update()
    {
        float mouseX = (Input.mousePosition.x / Screen.width - 0.5f) * 2f;
        float mouseY = (Input.mousePosition.y / Screen.height - 0.5f) * 2f;

        Vector2 offset = new Vector2(mouseX, mouseY) * (moveAmount / canvas.scaleFactor);
        Vector2 target = startPos + offset;

        rect.anchoredPosition = Vector2.Lerp(
            rect.anchoredPosition,
            target,
            smoothSpeed * Time.deltaTime
        );
    }
}