using System.Collections;
using UnityEngine;

public class DissolveEffect : MonoBehaviour
{
    [Header("Renderer")]
    [SerializeField] private Renderer targetRenderer;

    [Header("Hit (boss damage)")]
    [SerializeField] private float hitDuration = 0.4f;
    [SerializeField] private AnimationCurve hitCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [SerializeField] private Color hitColor = new Color(1f, 0.4f, 0.4f);
    [SerializeField] private float hitDissolveScale = 30f;

    [Header("Death (hazard / boss)")]
    [SerializeField] private float deathDuration = 1f;
    [SerializeField] private AnimationCurve deathCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [SerializeField] private Color deathColor = Color.red;
    [SerializeField] private float deathDissolveScale = 100f;

    [Header("Correct Note")]
    [SerializeField] private float noteDuration = 0.3f;
    [SerializeField] private AnimationCurve noteCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [SerializeField] private Color noteColor = new Color(0.4f, 1f, 0.4f);
    [SerializeField] private float noteDissolveScale = 5f;

    private static readonly int AmountID = Shader.PropertyToID("_DissolveAmmount");
    private static readonly int ColorID  = Shader.PropertyToID("_OutlineColor");
    private static readonly int ScaleID  = Shader.PropertyToID("_DisolveScale");

    private Material mat;
    private Coroutine current;

    private void Awake()
    {
        if (targetRenderer == null)
            targetRenderer = GetComponentInChildren<Renderer>();

        mat = targetRenderer.material;
    }

    public void Reset()
    {
        if (current != null) StopCoroutine(current);
        current = null;
        mat.SetFloat(AmountID, 0f);
    }

    public void PlayHit()        => Play(hitDuration,   hitCurve,   hitColor,   hitDissolveScale,   pingPong: true);
    public void PlayDeath()      => Play(deathDuration, deathCurve, deathColor, deathDissolveScale, pingPong: false);
    public void PlayCorrectNote()=> Play(noteDuration,  noteCurve,  noteColor,  noteDissolveScale,  pingPong: true);

    private void Play(float duration, AnimationCurve curve, Color color, float scale, bool pingPong)
    {
        if (current != null) StopCoroutine(current);
        current = StartCoroutine(Animate(duration, curve, color, scale, pingPong));
    }

    private IEnumerator Animate(float duration, AnimationCurve curve, Color color, float scale, bool pingPong)
    {
        mat.SetColor(ColorID, color);
        mat.SetFloat(ScaleID, scale);

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            mat.SetFloat(AmountID, curve.Evaluate(Mathf.Clamp01(t)));
            yield return null;
        }

        if (pingPong)
        {
            t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime / duration;
                mat.SetFloat(AmountID, curve.Evaluate(1f - Mathf.Clamp01(t)));
                yield return null;
            }
            mat.SetFloat(AmountID, 0f);
        }

        current = null;
    }
}