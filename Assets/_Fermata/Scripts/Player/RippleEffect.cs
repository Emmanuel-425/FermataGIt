using System.Collections;
using UnityEngine;

public class RippleEffect : MonoBehaviour
{
    [Header("Ripple Settings")]
    [SerializeField] private float duration = 0.6f;
    [SerializeField] private float maxDistance = 0.9f;

    private static readonly int RingDistanceProp = Shader.PropertyToID("_RingDistanceFromCenter");

    [Header("Safety")]
    [Tooltip("Only enable this on the ripple prefab, not on persistent objects.")]
    [SerializeField] private bool destroyOnComplete = true;

    private void Start()
    {
        StartCoroutine(Animate());
    }

    private IEnumerator Animate()
    {
        Material mat = GetComponent<SpriteRenderer>().material;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            mat.SetFloat(RingDistanceProp, Mathf.Lerp(-0.1f, maxDistance, t));
            elapsed += Time.deltaTime;
            yield return null;
        }

        if (destroyOnComplete)
            Destroy(gameObject);
    }
}
