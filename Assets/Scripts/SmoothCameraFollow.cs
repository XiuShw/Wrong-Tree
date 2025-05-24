using UnityEngine;

public class SmoothCameraFollow : MonoBehaviour
{
    [Header("Ŀ�����")]
    [Tooltip("�������Ҫ�����Ŀ����� (ͨ�������)")]
    public Transform target; // ��һ�������Ҫ�����Ŀ��

    [Header("���������")]
    [Tooltip("����������Ŀ���ƫ���������Ŀ����������ã���ֵ����Start()ʱ���ݳ�����ʼλ���Զ����㡣")]
    public Vector3 offset = new Vector3(0f, 10f, -5f); // �������Ŀ��֮��Ĺ̶�ƫ��

    [Tooltip("����������ƽ���ȣ�ֵԽСԽƽ�������ӳ�Խ��")]
    [Range(0.01f, 1.0f)]
    public float smoothSpeed = 0.125f; // �����ƽ����

    private Vector3 velocity = Vector3.zero; // ���� SmoothDamp �ĵ�ǰ�ٶȣ�����Ҫ�ֶ��޸�

    /// <summary>
    /// Awake �ڽű�ʵ��������ʱ���á�
    /// ���������ﳢ�Ի�ȡ��ʼ��ƫ������
    /// ʹ�� Awake ������ Start ��Ϊ��ȷ�� offset �ڵ�һ�� LateUpdate ǰ�����㣬
    /// ��ʹ�����ű��� Start ���ƶ��� target��
    /// </summary>
    void Awake()
    {
        if (target != null)
        {
            // �Զ����㲢���ó�ʼƫ����
            // ����������ĵ�ǰ����λ�ü�ȥĿ�������λ��
            offset = transform.position - target.position;
            Debug.Log($"�Զ�����������ƫ����: {offset}");
        }
        else
        {
            Debug.LogWarning("�����Ŀ�� (Target) δ�ڼ�����������á�" +
                             "�޷��Զ������ʼƫ��������ʹ�ü��������Ԥ���Offsetֵ��" +
                             "���ϣ���Զ����㣬��ȷ��������ǰ����Target��");
        }
    }

    void LateUpdate()
    {
        if (target == null)
        {
            // ����� Awake ʱ target Ϊ null������֮��Ҳû�б������ű���ֵ��
            // ��ô����������ʾ�����Կ���ֻ�� Awake ʱ��ʾһ�Ρ�
            // ����� target ����������ʱ��̬��ֵ������ļ����Ȼ�Ǳ�Ҫ�ġ�
            // Ϊ�������̨ˢ�������Լ�һ��������ǿ���ֻ����һ�Ρ�
            // Debug.LogWarning("�����δ���ø���Ŀ�꣡"); 
            return;
        }

        // ���������������λ��
        // target.position ����������ϵ��Ŀ���λ��
        // offset �������Ѿ�����õģ����ֶ����õģ��̶���������ƫ��
        Vector3 desiredPosition = target.position + offset;

        // ʹ�� Vector3.SmoothDamp ƽ�����ƶ������
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed);

        // ����ѡ���������ʼ�տ���Ŀ��
        // ���ںܶษ�ӽ���Ϸ��������й̶�����ת�Ƕȣ�����Ҫ��̬����Ŀ��
        // �����Ҫ������ȡ�������ע�ͣ���������ʼ���������ת�Ƕ�
        // transform.LookAt(target);
    }
}