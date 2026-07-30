using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform cameraTransform;

    [SerializeField] private Transform leftSprite;
    [SerializeField] private Transform centerSprite;
    [SerializeField] private Transform rightSprite;

    [Header("Parallax")]
    [Range(0f, 1f)]
    [SerializeField] private float parallaxSpeed = 0.3f;

    private float spriteWidth;
    private float lastCameraX;

    private void Start()
    {
        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;

        SpriteRenderer sr = centerSprite.GetComponent<SpriteRenderer>();

        if (sr == null)
        {
            Debug.LogError("Center sprite needs a SpriteRenderer.");
            enabled = false;
            return;
        }

        spriteWidth = sr.bounds.size.x;

        // Automatically place the sprites
        centerSprite.localPosition = Vector3.zero;
        leftSprite.localPosition = Vector3.left * spriteWidth;
        rightSprite.localPosition = Vector3.right * spriteWidth;

        lastCameraX = cameraTransform.position.x;
    }

    private void LateUpdate()
    {
        // -----------------------
        // PARALLAX
        // -----------------------

        float deltaX = cameraTransform.position.x - lastCameraX;

        transform.position += new Vector3(deltaX * parallaxSpeed, 0f, 0f);

        lastCameraX = cameraTransform.position.x;

        // -----------------------
        // LOOPING
        // -----------------------

        Transform[] sprites =
        {
            leftSprite,
            centerSprite,
            rightSprite
        };

        float leftMost = sprites[0].position.x;
        float rightMost = sprites[0].position.x;

        Transform left = sprites[0];
        Transform right = sprites[0];

        foreach (Transform t in sprites)
        {
            if (t.position.x < leftMost)
            {
                leftMost = t.position.x;
                left = t;
            }

            if (t.position.x > rightMost)
            {
                rightMost = t.position.x;
                right = t;
            }
        }

        float cameraX = cameraTransform.position.x;

        // Moving Right
        if (cameraX > centerSprite.position.x + spriteWidth * 0.5f)
        {
            left.position = new Vector3(
                right.position.x + spriteWidth,
                left.position.y,
                left.position.z);

            RotateRight();
        }

        // Moving Left
        if (cameraX < centerSprite.position.x - spriteWidth * 0.5f)
        {
            right.position = new Vector3(
                left.position.x - spriteWidth,
                right.position.y,
                right.position.z);

            RotateLeft();
        }
    }

    private void RotateRight()
    {
        Transform oldLeft = leftSprite;

        leftSprite = centerSprite;
        centerSprite = rightSprite;
        rightSprite = oldLeft;
    }

    private void RotateLeft()
    {
        Transform oldRight = rightSprite;

        rightSprite = centerSprite;
        centerSprite = leftSprite;
        leftSprite = oldRight;
    }
}