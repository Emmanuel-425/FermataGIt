using UnityEngine;
using Unity.Cinemachine;

public class CameraControlTrigger : MonoBehaviour
{
    public bool swapCameras = true;
    public CinemachineCamera cameraOnLeft;  
    public CinemachineCamera cameraOnRight; 
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
            
            if (exitDir.x > 0) // Exited Right
                CameraManager.instance.SwapCamera(cameraOnRight);
            else if (exitDir.x < 0) // Exited Left
                CameraManager.instance.SwapCamera(cameraOnLeft);
        }
    }
}