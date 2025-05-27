using UnityEngine;
using System.Collections;

/// <summary>
/// ������Ϸ��������С���������ֵ֮��ƽ�����������š�
/// ���˽ű����ӵ�����Ҫ���ŵ���Ϸ�����ϡ�
/// </summary>
public class ObjectScaler : MonoBehaviour
{
    [Header("��������")]
    [Tooltip("����ԭʼ������С�������ű��������ϣ���ӵ�ǰ��С��ʼ�����Խ�������Ϊ (1, 1, 1)��")]
    public Vector3 minScale = new Vector3(0.8f, 0.8f, 0.8f);

    [Tooltip("�����������ű�����")]
    public Vector3 maxScale = new Vector3(1.2f, 1.2f, 1.2f);

    [Tooltip("���һ�δ���С������ٻص���С�������������������ʱ�䣨�룩��")]
    public float scaleDuration = 2.0f;

    [Tooltip("�����ѡ���ű����� Start() ʱ��¼����ĳ�ʼ���ţ���������Ϊ minScale��")]
    public bool useInitialScaleAsMin = false;

    [Tooltip("�����ѡ��maxScale �����ǳ�ʼ���ţ���ָ����minScale������һ�������������Ǿ���ֵ��")]
    public bool useMultiplierForMaxScale = false;
    [Tooltip("�� useMultiplierForMaxScale Ϊ true ʱ�����ڼ��� maxScale �ı��������磬1.5 ��ʾ���Ŵ󵽳�ʼ��С��1.5����")]
    public float maxScaleMultiplier = 1.5f;


    private Vector3 initialScale; // ���ڴ洢����ĳ�ʼ����ֵ
    private float journeyLength; // PingPong�ĳ���

    void Start()
    {
        // ��¼��ʼ����ֵ
        initialScale = transform.localScale;

        if (useInitialScaleAsMin)
        {
            minScale = initialScale;
        }

        if (useMultiplierForMaxScale)
        {
            // ���ʹ�ñ���������� minScale (�����ǳ�ʼֵ��Ҳ�������ֶ����õ�ֵ) ���� maxScale
            maxScale = minScale * maxScaleMultiplier;
        }

        // PingPong �ĳ�����1�����ǽ��������� minScale �� maxScale ֮���ֵ
        // ʵ���ϣ����ǲ���Ҫ journeyLength ��������Ϊ Mathf.PingPong(Time.time / (scaleDuration / 2f), 1f)
        // �Ѿ��ṩ��һ���� 0 �� 1 ֮��ڶ���ֵ��
        // scaleDuration ��һ���������������ڣ����Ե����� scaleDuration / 2��
    }

    void Update()
    {
        if (scaleDuration <= 0)
        {
            // �����������������
            return;
        }

        // ���� PingPong �� t ֵ�������� 0 �� 1 ֮�����ذڶ�
        // Time.time / (scaleDuration / 2.0f) �Ľ��������ʱ����������
        // Mathf.PingPong �Ὣ�������������ֵ������ 0 �� 1 �ķ�Χ�����ذڶ�
        // (scaleDuration / 2.0f) ��ʾ����С����󣨻�������С�������ʱ��
        float t = Mathf.PingPong(Time.time / (scaleDuration / 2.0f), 1.0f);

        // ʹ�� Lerp �� minScale �� maxScale ֮����� t ֵ���в�ֵ
        transform.localScale = Vector3.Lerp(minScale, maxScale, t);

        // --- ��һ��ʹ�� Sin �����ķ��� (�����Ҫ��ƽ���ļӼ���Ч��) ---
        // float sinT = (Mathf.Sin(Time.time * (Mathf.PI * 2 / scaleDuration) - Mathf.PI / 2f) + 1f) / 2f;
        // transform.localScale = Vector3.Lerp(minScale, maxScale, sinT);
        // ����� (Mathf.PI * 2 / scaleDuration) ����Ƶ�ʣ�- Mathf.PI / 2f ����λƫ�ƣ�ȷ������Сֵ��ʼ
    }
}
