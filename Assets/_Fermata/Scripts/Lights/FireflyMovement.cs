using UnityEngine;

public class FireflyMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveRadius = 0.15f;
    [SerializeField] private float moveSpeed = 1.2f;

    [Header("Vertical Bobbing")]
    [SerializeField] private float bobHeight = 0.03f;
    [SerializeField] private float bobSpeed = 2f;

    private Vector3 startPosition;

    private float randomOffsetX;
    private float randomOffsetY;
    private float randomOffsetBob;

    private void Start()
    {
        startPosition = transform.localPosition;

        randomOffsetX = Random.Range(0f, 100f);
        randomOffsetY = Random.Range(0f, 100f);
        randomOffsetBob = Random.Range(0f, 100f);
    }

    private void Update()
    {
        float x =
            Mathf.Sin((Time.time + randomOffsetX) * moveSpeed)
            * moveRadius;

        float y =
            Mathf.Cos((Time.time + randomOffsetY) * moveSpeed)
            * moveRadius;

        y +=
            Mathf.Sin((Time.time + randomOffsetBob) * bobSpeed)
            * bobHeight;

        transform.localPosition =
            startPosition + new Vector3(x, y, 0f);
    }
}