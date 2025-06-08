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


    public static PlayerMovement Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }



    void FixedUpdate()
    {
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");
        Vector2 moveDirection = ((Vector2)transform.right * inputX + (Vector2)transform.up * inputY).normalized;
        if (LevelManager.minigameStart)
        {
            moveDirection = new Vector2 (0, 0);
        }
        rigidbody.linearVelocity = moveDirection * speed;

        if (moveDirection != Vector2.zero)
        {
            animator.SetBool("isMoving", true);
            stopX = inputX;
            stopY = inputY;
            playerWalk.enabled = true;
        }
        else
        {
            animator.SetBool("isMoving", false);
            playerWalk.enabled = false;
        }
        animator.SetFloat("InputX", stopX);
        animator.SetFloat("InputY", stopY);
    }
}
