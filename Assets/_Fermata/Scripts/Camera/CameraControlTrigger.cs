using UnityEngine;
using Unity.Cinemachine;

public class CameraControlTrigger : MonoBehaviour
{
    public bool swapCameras = true;
    public CinemachineCamera cameraOnLeft;  
    public CinemachineCamera cameraOnRight;
    public CinemachineCamera cameraOnTop;
    public CinemachineCamera cameraOnBottom;
    private BoxCollider2D coll;

    void Start() 
    { 
        coll = GetComponent<BoxCollider2D>(); // Added <BoxCollider2D> here
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && swapCameras)
        {
            // Calculates if the player exited the left or right side of the doorway
            Vector2 exitDir = (other.transform.position - coll.bounds.center).normalized;
            
            if (exitDir.x > 0 && cameraOnRight != null) CameraManager.instance.SwapCamera(cameraOnRight);
            else if (exitDir.x < 0 && cameraOnLeft != null) CameraManager.instance.SwapCamera(cameraOnLeft);
            else if (exitDir.y > 0 && cameraOnTop != null) CameraManager.instance.SwapCamera(cameraOnTop);
            else if (exitDir.y < 0 && cameraOnBottom != null) CameraManager.instance.SwapCamera(cameraOnBottom);
        }
    }
}