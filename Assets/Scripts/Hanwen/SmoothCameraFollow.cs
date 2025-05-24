using UnityEngine;

public class SmoothCameraFollow : MonoBehaviour
{
    [Header("目标对象")]
    [Tooltip("摄像机需要跟随的目标对象 (通常是玩家)")]
    public Transform target; 

    [Header("摄像机设置")]
    [Tooltip("摄像机相对于目标的偏移量。如果目标对象已设置，此值会在Start()时根据场景初始位置自动计算。")]
    public Vector3 offset = new Vector3(0f, 10f, -5f); 

    [Tooltip("摄像机跟随的平滑度，值越小越平滑，但延迟越高")]
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