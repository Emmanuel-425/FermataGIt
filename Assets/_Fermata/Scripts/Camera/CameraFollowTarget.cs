using UnityEngine;

public class CameraFollowTarget : MonoBehaviour
{
    public Transform player;
    public float smoothSpeed = 5f;

    void Update()
    {
        if (player == null) return;

        // Smoothly rotate to match the player's facing direction
        transform.rotation = Quaternion.Slerp(transform.rotation, player.rotation, smoothSpeed * Time.deltaTime);
        
        // Lock exact position to the player
        transform.position = player.position;
    }
}