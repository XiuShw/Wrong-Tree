using UnityEngine;
using System.Collections;

/// <summary>
/// ���Ƶ��Դ��Point Light��ģ���ѵ���˸Ч����
/// ���˽ű����ӵ��������Դ����Ϸ�����ϡ�
/// </summary>
public class FlickeringLight : MonoBehaviour
{
    [Tooltip("��Ҫ���Ƶĵ��Դ�����������գ��ű��᳢�Ի�ȡͬ�����ϵ�Light�����")]
    public Light pointLight;

    [Header("����ǿ������")]
    [Tooltip("��Դ����Сǿ�ȡ�")]
    float minIntensity = 3f;
    [Tooltip("��Դ�����ǿ�ȡ�")]
    float maxIntensity = 5f;

    [Header("���շ�Χ���� (��ѡ)")]
    [Tooltip("�Ƿ�ͬʱӰ���Դ�ķ�Χ��")]
    public bool affectRange = true;
    [Tooltip("��Դ����С��Χ��")]
    public float minRange = 10f;
    [Tooltip("��Դ�����Χ��")]
    public float maxRange = 10f;

    [Header("��˸����")]
    [Tooltip("ǿ�ȱ仯��ƽ���ٶȡ�ֵԽ�󣬱仯Խ�졣")]
    public float intensityChangeSpeed = 5f;
    [Tooltip("��Χ�仯��ƽ���ٶȣ����affectRangeΪtrue����ֵԽ�󣬱仯Խ�졣")]
    public float rangeChangeSpeed = 5f;

    [Tooltip("ѡ���µ����Ŀ��ǿ��/��Χ����Сʱ�������룩��")]
    public float minFlickerInterval = 0.05f;
    [Tooltip("ѡ���µ����Ŀ��ǿ��/��Χ�����ʱ�������룩��")]
    public float maxFlickerInterval = 0.2f;

    private float targetIntensity; // ��ǰ��˸��Ŀ��ǿ��
    private float targetRange;     // ��ǰ��˸��Ŀ�귶Χ
    private Coroutine flickerCoroutine; // ���ڹ������Ŀ��ֵ��Э��

    void Awake()
    {
        pointLight = GetComponent<Light>();
        //// ���pointLightδ��Inspector��ָ�������Ի�ȡ��GameObject�ϵ�Light���
        //if (pointLight == null)
        //{
            
        //}

        //// ����Ƿ���Light����Լ��Ƿ�ΪPoint Light
        //if (pointLight == null)
        //{
        //    Debug.LogError("FlickeringLight: δ�ҵ�Light������뽫�˽ű����ӵ���Light�������Ϸ�����ϣ�����Inspector��ָ��Light�����", this);
        //    enabled = false; // ���ô˽ű�
        //    return;
        //}

        //if (pointLight.type != LightType.Point)
        //{
        //    Debug.LogError("FlickeringLight: ָ���Ĺ�Դ���ǵ��Դ (Point Light)���˽ű���������ڵ��Դ��", this);
        //    enabled = false; // ���ô˽ű�
        //    return;
        //}
    }

    void OnEnable()
    {
        // ���������ʱ��������Ϸ��ʼʱ�����GameObject��������Ǽ���ģ�
        if (pointLight != null && pointLight.type == LightType.Point) // �ٴμ���Է���һ
        {
            // ��ʼ��Ŀ��ֵΪ��ǰ�ƹ�����ã�ʹ�ÿ�ʼʱ��һ��ƽ���Ĺ���
            targetIntensity = pointLight.intensity;
            if (affectRange)
            {
                targetRange = pointLight.range;
            }

            // ֹͣ�κο����������еľ�Э�̣�Ȼ�������µ�
            if (flickerCoroutine != null)
            {
                StopCoroutine(flickerCoroutine);
            }
            flickerCoroutine = StartCoroutine(UpdateFlickerTargetsRoutine());
        }
    }

    void OnDisable()
    {
        // ��������û�GameObject������ʱֹͣЭ��
        if (flickerCoroutine != null)
        {
            StopCoroutine(flickerCoroutine);
            flickerCoroutine = null;
        }
    }

    /// <summary>
    /// Э�̣������Եظ�����˸��Ŀ��ǿ�Ⱥͷ�Χ��
    /// </summary>
    IEnumerator UpdateFlickerTargetsRoutine()
    {
        // ����ѭ����ֱ��Э�̱�ֹͣ
        while (true)
        {
            // �ڶ������С�����ǿ��֮��ѡ��һ���µ����Ŀ��ǿ��
            targetIntensity = Random.Range(minIntensity, maxIntensity);

            // ���������Ӱ�췶Χ��ѡ���ͬ��Ϊ��Χѡ��һ���µ����Ŀ��ֵ
            if (affectRange)
            {
                targetRange = Random.Range(minRange, maxRange);
            }

            // �ȴ�һ�������ʱ�������ٽ�����һ��Ŀ�����
            // ��ʹ����˸�Ľ��಻��ô���ɣ�����Ȼ
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


        // ����ű������û�û����Ч�ĵ��Դ����ִ���κβ���
        if (!enabled || pointLight == null)
        {
            return;
        }

        // ƽ���ؽ���ǰ��Դǿ�Ȳ�ֵ��Ŀ��ǿ��
        // Time.deltaTime ȷ���仯�ٶ���֡���޹�
        pointLight.intensity = Mathf.Lerp(pointLight.intensity, targetIntensity, Time.deltaTime * intensityChangeSpeed);

        // ���������Ӱ�췶Χ��ѡ���ͬ��ƽ����ֵ��Դ��Χ
        if (affectRange)
        {
            pointLight.range = Mathf.Lerp(pointLight.range, targetRange, Time.deltaTime * rangeChangeSpeed);
        }
    }
}
