using UnityEngine;

public class EnableNPC : MonoBehaviour
{
    private void Start()
    {
        GameManager.Instance.simulationEnabled = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {
            GameManager.Instance.simulationEnabled = true;
        }
    }
}
