using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class RotatingCamera : MonoBehaviour
{
    public float rotateTime = 0.2f;
    private Transform player;

    [Tooltip("Smoothing speed for camera movement. Lower values mean smoother and slower following.")]
    [Range(0.01f, 1.0f)]
    public float smoothSpeed = 0.125f;

    [Tooltip("��ת���ٶȣ�ÿ����ת�Ķ�����")]
    public float rotationSpeed = 360.0f; // ���磬ÿ����ת360��

    [Range(0.1f, 20f)]
    public float rotationSmoothSpeed = 5f;

    private Quaternion targetRotation; // Ŀ����ת����Ԫ����
    private bool isRotating = false;   // ����Ƿ�������ת

    private Vector3 velocity = Vector3.zero;


    [SerializeField] Transform gameFinishTarget;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {

    }

    void LateUpdate()
    {
        if (!LevelManager.gameFinished)
        {
            if (smoothSpeed > 0.125f)
            {
                smoothSpeed -= 0.3f * Time.deltaTime;
            }

            transform.position = Vector3.SmoothDamp(transform.position, player.position, ref velocity, smoothSpeed);

            if (!isRotating)
            {
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    targetRotation = transform.rotation * Quaternion.Euler(0, 0, 90);
                    isRotating = true; // ��ǿ�ʼ��ת
                    AudioManager.Instance.PlaySFX("placeholder");

                }
                else if (Input.GetKeyDown(KeyCode.E))
                {
                    targetRotation = transform.rotation * Quaternion.Euler(0, 0, -90);
                    isRotating = true; // ��ǿ�ʼ��ת
                    AudioManager.Instance.PlaySFX("placeholder");

                }
            }

            if (isRotating)
            {
                // Quaternion.RotateTowards(��ǰ��ת, Ŀ����ת, ��󲽳�)
                // ������ rotationSpeed * Time.deltaTime ���ٶȣ�����/�룩��Ŀ����ת����
                float step = rotationSpeed * Time.deltaTime; // ����ÿ֡Ӧ����ת�Ķ���

                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSmoothSpeed * Time.deltaTime);

                // ����Ƿ��Ѿ��ǳ��ӽ�Ŀ����ת
                // Quaternion.Angle ����������Ԫ��֮��ĽǶȲ0��180�ȣ�
                if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f) // ����һ����С����ֵ�����⸡��������Զ�޷���ȷ����
                {
                    transform.rotation = targetRotation; // ��ȷ���õ�Ŀ����ת������΢С���
                    isRotating = false; // �����ת���
                }

            }
        }
        else
        {
            //smoothSpeed = 0.03f;
            transform.DOMove(gameFinishTarget.position, 0.7f).SetEase(Ease.InCirc);
        }
    }
}
