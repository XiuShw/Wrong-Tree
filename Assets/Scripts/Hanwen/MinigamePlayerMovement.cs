using UnityEngine;

public class MinigamePlayerMovement : MonoBehaviour
{
    float inputX, inputY;
    new Rigidbody2D rigidbody;
    [SerializeField] float speed;
    [SerializeField] ThoughtsSpawner thoughtsSpawner;


    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");
        Vector2 moveDirection = ((Vector2)transform.right * inputX + (Vector2)transform.up * inputY).normalized;
        if (!LevelManager.minigameStart)
        {
            moveDirection = new Vector2(0, 0);
        }
        rigidbody.linearVelocity = moveDirection * speed;

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Thoughts"))
        {
            AudioManager.Instance.PlaySFX("thoughtCollision");
            if (collision.gameObject.GetComponent<ThoughtsMovement>().isPositive)
            {
                thoughtsSpawner.circumstance++;
            }
            else
            {
                thoughtsSpawner.circumstance--;
            }
            Destroy(collision.gameObject);
        }
    }
}
