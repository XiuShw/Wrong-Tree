using UnityEngine;

public class LightGrass : MonoBehaviour
{
    public Light pointLight;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && pointLight.GetComponent<Light>().enabled == true)
        {
            LevelManager.maxLight += 0.5f;
            LevelManager.minLight += 0.3f;
            LevelManager.lightOwn += 0.1f;
            pointLight.GetComponent<Light>().enabled = false;
            AudioManager.Instance.PlaySFX("grass");
        }
    }
}
