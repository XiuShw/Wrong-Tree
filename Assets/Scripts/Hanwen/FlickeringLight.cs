using UnityEngine;
using System.Collections;

/// <summary>
/// 控制点光源（Point Light）模拟火把等闪烁效果。
/// 将此脚本附加到包含点光源的游戏对象上。
/// </summary>
public class FlickeringLight : MonoBehaviour
{
    [Tooltip("需要控制的点光源组件。如果留空，脚本会尝试获取同物体上的Light组件。")]
    public Light pointLight;

    [Header("光照强度设置")]
    [Tooltip("光源的最小强度。")]
    float minIntensity = 3f;
    [Tooltip("光源的最大强度。")]
    float maxIntensity = 5f;

    [Header("光照范围设置 (可选)")]
    [Tooltip("是否同时影响光源的范围。")]
    public bool affectRange = true;
    [Tooltip("光源的最小范围。")]
    public float minRange = 10f;
    [Tooltip("光源的最大范围。")]
    public float maxRange = 10f;

    [Header("闪烁参数")]
    [Tooltip("强度变化的平滑速度。值越大，变化越快。")]
    public float intensityChangeSpeed = 5f;
    [Tooltip("范围变化的平滑速度（如果affectRange为true）。值越大，变化越快。")]
    public float rangeChangeSpeed = 5f;

    [Tooltip("选择新的随机目标强度/范围的最小时间间隔（秒）。")]
    public float minFlickerInterval = 0.05f;
    [Tooltip("选择新的随机目标强度/范围的最大时间间隔（秒）。")]
    public float maxFlickerInterval = 0.2f;

    private float targetIntensity; // 当前闪烁的目标强度
    private float targetRange;     // 当前闪烁的目标范围
    private Coroutine flickerCoroutine; // 用于管理更新目标值的协程

    void Awake()
    {
        pointLight = GetComponent<Light>();
        //// 如果pointLight未在Inspector中指定，尝试获取该GameObject上的Light组件
        //if (pointLight == null)
        //{
            
        //}

        //// 检查是否有Light组件以及是否为Point Light
        //if (pointLight == null)
        //{
        //    Debug.LogError("FlickeringLight: 未找到Light组件！请将此脚本附加到有Light组件的游戏对象上，或在Inspector中指定Light组件。", this);
        //    enabled = false; // 禁用此脚本
        //    return;
        //}

        //if (pointLight.type != LightType.Point)
        //{
        //    Debug.LogError("FlickeringLight: 指定的光源不是点光源 (Point Light)！此脚本仅设计用于点光源。", this);
        //    enabled = false; // 禁用此脚本
        //    return;
        //}
    }

    void OnEnable()
    {
        // 组件被激活时（包括游戏开始时，如果GameObject和组件都是激活的）
        if (pointLight != null && pointLight.type == LightType.Point) // 再次检查以防万一
        {
            // 初始化目标值为当前灯光的设置，使得开始时有一个平滑的过渡
            targetIntensity = pointLight.intensity;
            if (affectRange)
            {
                targetRange = pointLight.range;
            }

            // 停止任何可能正在运行的旧协程，然后启动新的
            if (flickerCoroutine != null)
            {
                StopCoroutine(flickerCoroutine);
            }
            flickerCoroutine = StartCoroutine(UpdateFlickerTargetsRoutine());
        }
    }

    void OnDisable()
    {
        // 组件被禁用或GameObject被销毁时停止协程
        if (flickerCoroutine != null)
        {
            StopCoroutine(flickerCoroutine);
            flickerCoroutine = null;
        }
    }

    /// <summary>
    /// 协程：周期性地更新闪烁的目标强度和范围。
    /// </summary>
    IEnumerator UpdateFlickerTargetsRoutine()
    {
        // 无限循环，直到协程被停止
        while (true)
        {
            // 在定义的最小和最大强度之间选择一个新的随机目标强度
            targetIntensity = Random.Range(minIntensity, maxIntensity);

            // 如果启用了影响范围的选项，则同样为范围选择一个新的随机目标值
            if (affectRange)
            {
                targetRange = Random.Range(minRange, maxRange);
            }

            // 等待一个随机的时间间隔，再进行下一次目标更新
            // 这使得闪烁的节奏不那么规律，更自然
            yield return new WaitForSeconds(Random.Range(minFlickerInterval, maxFlickerInterval));
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && maxIntensity < 21) 
        {
            maxIntensity += 2;
            minIntensity += 1;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && minIntensity > 3) 
        {
            maxIntensity -= 2;
            minIntensity -= 1;
        }


        // 如果脚本被禁用或没有有效的点光源，则不执行任何操作
        if (!enabled || pointLight == null)
        {
            return;
        }

        // 平滑地将当前光源强度插值到目标强度
        // Time.deltaTime 确保变化速度与帧率无关
        pointLight.intensity = Mathf.Lerp(pointLight.intensity, targetIntensity, Time.deltaTime * intensityChangeSpeed);

        // 如果启用了影响范围的选项，则同样平滑插值光源范围
        if (affectRange)
        {
            pointLight.range = Mathf.Lerp(pointLight.range, targetRange, Time.deltaTime * rangeChangeSpeed);
        }
    }
}
