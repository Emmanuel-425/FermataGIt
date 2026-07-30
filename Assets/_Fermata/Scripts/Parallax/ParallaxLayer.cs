using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform spriteA;
    [SerializeField] private Transform spriteB;

    [Header("Parallax")]
    [Range(0f, 1f)]
    [SerializeField] private float parallaxSpeed = 0.3f;

    private float spriteWidth;
    private float lastCameraX;

    private void Start()
    {
        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;

        if (spriteA == null || spriteB == null)
        {
            Debug.LogError($"{name}: Please assign Sprite A and Sprite B.");
            enabled = false;
            return;
        }

        SpriteRenderer sr = spriteA.GetComponent<SpriteRenderer>();

        if (sr == null)
        {
            Debug.LogError($"{name}: Sprite A needs a SpriteRenderer.");
            enabled = false;
            return;
        }

        // Get sprite width in world units
        spriteWidth = sr.bounds.size.x;

        // Automatically place the two sprites side by side
        spriteA.localPosition = Vector3.zero;
        spriteB.localPosition = new Vector3(spriteWidth, 0f, 0f);

        lastCameraX = cameraTransform.position.x;
    }

    private void LateUpdate()
    {
        // ==========================
        // PARALLAX (Horizontal Only)
        // ==========================

        float deltaX = cameraTransform.position.x - lastCameraX;

        transform.position += new Vector3(deltaX * parallaxSpeed, 0f, 0f);

        lastCameraX = cameraTransform.position.x;

        // ==========================
        // INFINITE SCROLL
        // ==========================

        Transform left;
        Transform right;

        if (spriteA.position.x < spriteB.position.x)
        {
            left = spriteA;
            right = spriteB;
        }
        else
        {
            left = spriteB;
            right = spriteA;
        }

        float cameraX = cameraTransform.position.x;

        // Move the left sprite to the right
        if (cameraX > right.position.x)
        {
            left.position = new Vector3(
                right.position.x + spriteWidth,
                left.position.y,
                left.position.z);
        }

        // Move the right sprite to the left
        if (cameraX < left.position.x)
        {
            right.position = new Vector3(
                left.position.x - spriteWidth,
                right.position.y,
                right.position.z);
        }
    }
}