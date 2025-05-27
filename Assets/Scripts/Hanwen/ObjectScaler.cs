using UnityEngine;
using System.Collections;

/// <summary>
/// 控制游戏对象在最小和最大缩放值之间平滑地来回缩放。
/// 将此脚本附加到你想要缩放的游戏对象上。
/// </summary>
public class ObjectScaler : MonoBehaviour
{
    [Header("缩放设置")]
    [Tooltip("物体原始（或最小）的缩放比例。如果希望从当前大小开始，可以将其设置为 (1, 1, 1)。")]
    public Vector3 minScale = new Vector3(0.8f, 0.8f, 0.8f);

    [Tooltip("物体最大的缩放比例。")]
    public Vector3 maxScale = new Vector3(1.2f, 1.2f, 1.2f);

    [Tooltip("完成一次从最小到最大再回到最小的完整缩放周期所需的时间（秒）。")]
    public float scaleDuration = 2.0f;

    [Tooltip("如果勾选，脚本将在 Start() 时记录物体的初始缩放，并将其作为 minScale。")]
    public bool useInitialScaleAsMin = false;

    [Tooltip("如果勾选，maxScale 将会是初始缩放（或指定的minScale）乘以一个倍数，而不是绝对值。")]
    public bool useMultiplierForMaxScale = false;
    [Tooltip("当 useMultiplierForMaxScale 为 true 时，用于计算 maxScale 的倍数。例如，1.5 表示最大放大到初始大小的1.5倍。")]
    public float maxScaleMultiplier = 1.5f;


    private Vector3 initialScale; // 用于存储物体的初始缩放值
    private float journeyLength; // PingPong的长度

    void Start()
    {
        // 记录初始缩放值
        initialScale = transform.localScale;

        if (useInitialScaleAsMin)
        {
            minScale = initialScale;
        }

        if (useMultiplierForMaxScale)
        {
            // 如果使用倍数，则基于 minScale (可能是初始值，也可能是手动设置的值) 计算 maxScale
            maxScale = minScale * maxScaleMultiplier;
        }

        // PingPong 的长度是1，我们将用它来在 minScale 和 maxScale 之间插值
        // 实际上，我们不需要 journeyLength 变量，因为 Mathf.PingPong(Time.time / (scaleDuration / 2f), 1f)
        // 已经提供了一个在 0 和 1 之间摆动的值。
        // scaleDuration 是一个完整的来回周期，所以单程是 scaleDuration / 2。
    }

    void Update()
    {
        if (scaleDuration <= 0)
        {
            // 避免除以零或负数的情况
            return;
        }

        // 计算 PingPong 的 t 值，它会在 0 和 1 之间来回摆动
        // Time.time / (scaleDuration / 2.0f) 的结果会随着时间线性增长
        // Mathf.PingPong 会将这个线性增长的值限制在 0 到 1 的范围内来回摆动
        // (scaleDuration / 2.0f) 表示从最小到最大（或从最大到最小）所需的时间
        float t = Mathf.PingPong(Time.time / (scaleDuration / 2.0f), 1.0f);

        // 使用 Lerp 在 minScale 和 maxScale 之间根据 t 值进行插值
        transform.localScale = Vector3.Lerp(minScale, maxScale, t);

        // --- 另一种使用 Sin 函数的方法 (如果需要更平滑的加减速效果) ---
        // float sinT = (Mathf.Sin(Time.time * (Mathf.PI * 2 / scaleDuration) - Mathf.PI / 2f) + 1f) / 2f;
        // transform.localScale = Vector3.Lerp(minScale, maxScale, sinT);
        // 这里的 (Mathf.PI * 2 / scaleDuration) 控制频率，- Mathf.PI / 2f 是相位偏移，确保从最小值开始
    }
}
