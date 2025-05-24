using UnityEngine;

public class SmoothCameraFollow : MonoBehaviour
{
    [Header("Ŀ�����")]
    [Tooltip("�������Ҫ�����Ŀ����� (ͨ�������)")]
    public Transform target; 

    [Header("���������")]
    [Tooltip("����������Ŀ���ƫ���������Ŀ����������ã���ֵ����Start()ʱ���ݳ�����ʼλ���Զ����㡣")]
    public Vector3 offset = new Vector3(0f, 10f, -5f); 

    [Tooltip("����������ƽ���ȣ�ֵԽСԽƽ�������ӳ�Խ��")]
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