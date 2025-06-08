using UnityEngine;

public class EnableNPC : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {
            Debug.Log(111);
            GameManager.Instance.simulationEnabled = true;
        }
    }
}
