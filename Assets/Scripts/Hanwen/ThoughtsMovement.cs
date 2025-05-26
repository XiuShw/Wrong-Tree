using UnityEngine;

public class ThoughtsMovement : MonoBehaviour
{
    [SerializeField] float initialForce;
    float currentSpeedSqr;

    Rigidbody2D rb;
    float count;
    [SerializeField] int threshold;

    public bool isPositive = false;

    Color color;

    void Start()
    {
        color = GetComponent<SpriteRenderer>().color;
        rb = GetComponent<Rigidbody2D>();

        Vector2 randomDirection = Random.insideUnitCircle.normalized;

        rb.AddForce(randomDirection * initialForce, ForceMode2D.Impulse);
    }

    void Update()
    {
        color.a += 0.7f * Time.deltaTime;
        count += Time.deltaTime;

        if (count > threshold)
        {
            isPositive = true;
            if (color.g <= 1)
            {
                color.g += 1f * Time.deltaTime;
                color.b += 1f * Time.deltaTime;
            }
        }
        GetComponent<SpriteRenderer>().color = color;

        currentSpeedSqr = rb.linearVelocity.sqrMagnitude;

        if (currentSpeedSqr != (initialForce * initialForce))
        {
            rb.linearVelocity = rb.linearVelocity.normalized * initialForce;
        }
    }


}