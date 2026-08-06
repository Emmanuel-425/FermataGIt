using System.Collections;
using UnityEngine;

public class BreakingPlatform : MonoBehaviour
{
    [Header("Break Settings")]
    [SerializeField] private float breakDelay = 1.5f;

    [SerializeField] private float fadeDuration = 0.5f;

    private SpriteRenderer spriteRenderer;
    private Collider2D platformCollider;

    private bool triggered;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        platformCollider = GetComponent<Collider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (triggered)
            return;

        if (!collision.gameObject.CompareTag("Player"))
            return;

        triggered = true;

        StartCoroutine(BreakRoutine());
    }

    private IEnumerator BreakRoutine()
    {
        yield return new WaitForSeconds(breakDelay);

        if (platformCollider != null)
        {
            platformCollider.enabled = false;
        }

        Color color = spriteRenderer.color;

        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;

            color.a = Mathf.Lerp(
                1f,
                0f,
                timer / fadeDuration
            );

            spriteRenderer.color = color;

            yield return null;
        }

        Destroy(gameObject);
    }
}