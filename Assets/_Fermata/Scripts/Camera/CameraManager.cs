using UnityEngine;
using Unity.Cinemachine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;
    public CinemachineCamera[] cameras; 
    private CinemachinePositionComposer composer; 
    
    public float normalYDamp = 1f;
    public float fallingYDamp = 0.2f;
    private bool isFalling;

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
    }

    public void SetFalling(bool falling) { isFalling = falling; }

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