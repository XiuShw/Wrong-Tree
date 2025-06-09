using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float speed;
    new Rigidbody2D rigidbody;
    Animator animator;
    float inputX, inputY;
    float stopX, stopY;
    public AudioSource playerWalk;
    SpriteRenderer spriteRenderer;
    bool playBGM = false;


    public static PlayerMovement Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    void FixedUpdate()
    {
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            inputY = 1f;
        }

        Vector2 moveDirection = ((Vector2)transform.right * inputX + (Vector2)transform.up * inputY).normalized;
        if (LevelManager.minigameStart)
        {
            moveDirection = new Vector2(0, 0);
        }
        rigidbody.linearVelocity = moveDirection * speed;


        if (inputX > 0.1f)
        {
            // 面向右边 (不翻转)
            spriteRenderer.flipX = false;
        }
        // 如果水平速度小于一个很小的负数（例如-0.1f），我们认为它在向左移动
        else if (inputX < -0.1f)
        {
            // 面向左边 (水平翻转)
            spriteRenderer.flipX = true;
        }

        if (!LevelManager.minigameStart && (inputX != 0 || inputY != 0))
        {
            animator.SetBool("isMoving", true);
            playerWalk.enabled = true;

        }
        else
        {
            animator.SetBool("isMoving", false);
            playerWalk.enabled = false;
        }

        if (gameObject.transform.position.x > 220)
        {
            GameManager.Instance.simulationEnabled = true;
            if (LevelManager.globalReputation >= 3 && !playBGM)
            {
                AudioManager.Instance.PlayBGM("beforeGoodEnd");
                playBGM = true;
            }
        }
    }
}