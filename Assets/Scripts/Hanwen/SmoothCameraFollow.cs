using UnityEngine;

public class SmoothCameraFollow : MonoBehaviour
{
    [Header("Target Settings")]
    [Tooltip("The target the camera will follow (usually the player)")]
    public Transform target; 

    [Header("Camera Settings")]
    [Tooltip("Offset from the target. If left as default, will be calculated automatically in Start() based on initial positions.")]
    public Vector3 offset = new Vector3(0f, 10f, -5f); 

    [Tooltip("Smoothing speed for camera movement. Lower values mean smoother and slower following.")]
    [Range(0.01f, 1.0f)]
    public float smoothSpeed = 0.125f; 

    private Vector3 velocity = Vector3.zero; 

    void Awake()
    {
        if (target != null)
        {
            offset = transform.position - target.position;
        }
    }

    void LateUpdate()
    {
        if (target == null)
        {
            return;
        }

        Vector3 desiredPosition = target.position + offset;

        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed);
    }
}