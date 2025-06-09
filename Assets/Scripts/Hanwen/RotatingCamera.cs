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

    [Tooltip("旋转的速度（每秒旋转的度数）")]
    public float rotationSpeed = 360.0f; // 例如，每秒旋转360度

    [Range(0.1f, 20f)]
    public float rotationSmoothSpeed = 5f;

    private Quaternion targetRotation; // 目标旋转（四元数）
    private bool isRotating = false;   // 标记是否正在旋转

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
                    isRotating = true; // 标记开始旋转
                    AudioManager.Instance.PlaySFX("placeholder");

                }
                else if (Input.GetKeyDown(KeyCode.E))
                {
                    targetRotation = transform.rotation * Quaternion.Euler(0, 0, -90);
                    isRotating = true; // 标记开始旋转
                    AudioManager.Instance.PlaySFX("placeholder");

                }
            }

            if (isRotating)
            {
                // Quaternion.RotateTowards(当前旋转, 目标旋转, 最大步长)
                // 它会以 rotationSpeed * Time.deltaTime 的速度（度数/秒）向目标旋转靠近
                float step = rotationSpeed * Time.deltaTime; // 计算每帧应该旋转的度数

                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSmoothSpeed * Time.deltaTime);

                // 检查是否已经非常接近目标旋转
                // Quaternion.Angle 返回两个四元数之间的角度差（0到180度）
                if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f) // 设置一个很小的阈值，避免浮点误差导致永远无法精确到达
                {
                    transform.rotation = targetRotation; // 精确设置到目标旋转，消除微小误差
                    isRotating = false; // 标记旋转完成
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
