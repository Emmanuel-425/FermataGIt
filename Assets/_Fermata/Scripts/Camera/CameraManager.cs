using UnityEngine;
using Unity.Cinemachine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;
    public CinemachineCamera[] cameras; 
    public CinemachineCamera defaultCamera;
    private CinemachinePositionComposer composer; 
    
    public float normalYDamp = 1f;
    public float fallingYDamp = 0.2f;
    private bool isFalling;

    [Header("Vertical Look")]
    public float verticalLookOffset = 0.25f;
    public float verticalLookSpeed = 3f;
    public float verticalLookDelay = 0.8f;
    private float targetScreenY = 0f;
    private float holdTimer = 0f;

    void Awake()
    {
        instance = this;
        if (cameras != null && cameras.Length > 0 && cameras[0] != null)
        {
            composer = cameras[0].GetComponent<CinemachinePositionComposer>();
        }
    }

    void Update()
    {
        if (composer == null) return;
        
        float target = isFalling ? fallingYDamp : normalYDamp;
        composer.Damping.y = Mathf.Lerp(composer.Damping.y, target, 2f * Time.deltaTime);

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
        {
            holdTimer += Time.deltaTime;
            if (holdTimer >= verticalLookDelay)
                targetScreenY = Input.GetKey(KeyCode.W) ? verticalLookOffset : -verticalLookOffset;
        }
        else
        {
            holdTimer = 0f;
            targetScreenY = 0f;
        }

        var comp = composer.Composition;
        comp.ScreenPosition.y = Mathf.Lerp(comp.ScreenPosition.y, targetScreenY, verticalLookSpeed * Time.deltaTime);
        composer.Composition = comp;
    }

    public void SetFalling(bool falling) { isFalling = falling; }

    public void RestoreDefaultCamera()
    {
        if (defaultCamera == null) return;
        foreach (var cam in cameras)
            if (cam != null) cam.gameObject.SetActive(false);
        defaultCamera.gameObject.SetActive(true);
        composer = defaultCamera.GetComponent<CinemachinePositionComposer>();
    }

    public void SwapCamera(CinemachineCamera newCam)
    {
        if (newCam == null) return;

        foreach (var cam in cameras)
        {
            if (cam != null) cam.gameObject.SetActive(false);
        }

        newCam.gameObject.SetActive(true);
        composer = newCam.GetComponent<CinemachinePositionComposer>();
    }
}