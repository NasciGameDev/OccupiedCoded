using System.Collections;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    
    [SerializeField] 
    private float cameraSpeed = 0.06f;
    [SerializeField] 
    Vector3 offset;
    
    private Transform playerTransform;
    private bool isMovementDeactivated = false;

    // Update is called once per frame
    void FixedUpdate()
    {
        if(isMovementDeactivated) return;

        if (playerTransform == null)
        {
            Player player = FindFirstObjectByType<Player>();
            if (player == null) return;
            playerTransform = player.GetCameraTarget();
        }
        
            Vector3 targetPos = playerTransform.position + offset;
            targetPos.z = transform.position.z;



        if (Vector3.Distance(playerTransform.position, transform.position) <= 300)
        {
            Vector3 toDestination = targetPos - transform.position;
            transform.position += toDestination * cameraSpeed;
        }
        else
        {
            transform.position = targetPos;
        }
    }

    public void DeactivateMovement() => isMovementDeactivated = true;
    public void ActivateMovement() => isMovementDeactivated = false;
}
