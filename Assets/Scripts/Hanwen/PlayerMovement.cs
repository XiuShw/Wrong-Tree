using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float speed;
    new private Rigidbody2D rigidbody;
    private Animator animator;
    private float inputX, inputY;
    private float stopX, stopY;

    [SerializeField] Light playerLight;
    [SerializeField] TMP_Text canInteract;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && playerLight.intensity < 20) { playerLight.intensity += 2; }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && playerLight.intensity > 4) { playerLight.intensity -= 2; }
    }

    void FixedUpdate()
    {
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");
        Vector2 moveDirection = ((Vector2)transform.right * inputX + (Vector2)transform.up * inputY).normalized;
        rigidbody.linearVelocity = moveDirection * speed;

        if (moveDirection != Vector2.zero)
        {
            animator.SetBool("isMoving", true);
            stopX = inputX;
            stopY = inputY;
        }
        else
        {
            animator.SetBool("isMoving", false);
        }
        animator.SetFloat("InputX", stopX);
        animator.SetFloat("InputY", stopY);

    }







    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("NPC")) { canInteract.text = "'E' to interact"; }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        canInteract.text = "";
    }
}
