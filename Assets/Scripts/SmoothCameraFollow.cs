using UnityEngine;

public class SmoothCameraFollow : MonoBehaviour
{
    [Header("目标对象")]
    [Tooltip("摄像机需要跟随的目标对象 (通常是玩家)")]
    public Transform target; // 玩家或其他需要跟随的目标

    [Header("摄像机设置")]
    [Tooltip("摄像机相对于目标的偏移量。如果目标对象已设置，此值会在Start()时根据场景初始位置自动计算。")]
    public Vector3 offset = new Vector3(0f, 10f, -5f); // 摄像机与目标之间的固定偏移

    [Tooltip("摄像机跟随的平滑度，值越小越平滑，但延迟越高")]
    [Range(0.01f, 1.0f)]
    public float smoothSpeed = 0.125f; // 跟随的平滑度

    private Vector3 velocity = Vector3.zero; // 用于 SmoothDamp 的当前速度，不需要手动修改

    /// <summary>
    /// Awake 在脚本实例被加载时调用。
    /// 我们在这里尝试获取初始的偏移量。
    /// 使用 Awake 而不是 Start 是为了确保 offset 在第一次 LateUpdate 前被计算，
    /// 即使其他脚本在 Start 中移动了 target。
    /// </summary>
    void Awake()
    {
        if (target != null)
        {
            // 自动计算并设置初始偏移量
            // 这是摄像机的当前世界位置减去目标的世界位置
            offset = transform.position - target.position;
            Debug.Log($"自动计算的摄像机偏移量: {offset}");
        }
        else
        {
            Debug.LogWarning("摄像机目标 (Target) 未在检视面板中设置。" +
                             "无法自动计算初始偏移量。将使用检视面板中预设的Offset值。" +
                             "如果希望自动计算，请确保在运行前分配Target。");
        }
    }

    void LateUpdate()
    {
        if (target == null)
        {
            // 如果在 Awake 时 target 为 null，并且之后也没有被其他脚本赋值，
            // 那么这里会持续提示。可以考虑只在 Awake 时提示一次。
            // 但如果 target 可能在运行时动态赋值，这里的检查仍然是必要的。
            // 为避免控制台刷屏，可以加一个布尔标记控制只警告一次。
            // Debug.LogWarning("摄像机未设置跟随目标！"); 
            return;
        }

        // 计算期望的摄像机位置
        // target.position 是世界坐标系中目标的位置
        // offset 是我们已经计算好的（或手动设置的）固定世界坐标偏移
        Vector3 desiredPosition = target.position + offset;

        // 使用 Vector3.SmoothDamp 平滑地移动摄像机
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed);

        // （可选）让摄像机始终看向目标
        // 对于很多俯视角游戏，摄像机有固定的旋转角度，不需要动态看向目标
        // 如果需要，可以取消下面的注释，并调整初始的摄像机旋转角度
        // transform.LookAt(target);
    }
}