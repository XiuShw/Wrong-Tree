using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void LateUpdate()
    {
        transform.position = Vector3.SmoothDamp(transform.position, player.position, ref velocity, smoothSpeed);

        if (!isRotating)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                targetRotation = transform.rotation * Quaternion.Euler(0, 0, 90);
                isRotating = true; // ��ǿ�ʼ��ת
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                targetRotation = transform.rotation * Quaternion.Euler(0, 0, -90);
                isRotating = true; // ��ǿ�ʼ��ת
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

        //Rotate();
    }

    //void Rotate()
    //{
    //    if (Input.GetKey(KeyCode.Q) && !isRotating)
    //    {
    //        StartCoroutine(RotateAround(-10, rotateTime));
    //    }
    //    if (Input.GetKey(KeyCode.E) && !isRotating)
    //    {
    //        StartCoroutine(RotateAround(10, rotateTime));
    //    }
    //}

    //IEnumerator RotateAround(float angel, float time)
    //{
    //    float number = 60 * time;
    //    float nextAngel = angel / number;
    //    isRotating = true;

    //    for (int i = 0; i < number; i++)
    //    {
    //        transform.Rotate(new Vector3(0, 0, nextAngel));
    //        yield return new WaitForFixedUpdate();
    //    }

    //    isRotating = false;
    //}
}
