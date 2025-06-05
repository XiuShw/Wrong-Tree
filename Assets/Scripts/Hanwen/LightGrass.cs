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
            LevelManager.maxLight += 3f;
            LevelManager.minLight += 1.5f;
            LevelManager.lightOwn += 1;
            pointLight.GetComponent<Light>().enabled = false;
            AudioManager.Instance.PlaySFX("grass");
        }
    }
}
