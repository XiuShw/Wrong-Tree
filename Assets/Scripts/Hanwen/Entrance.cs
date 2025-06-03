using UnityEngine;
using UnityEngine.UI;

public class Entrance : MonoBehaviour
{
    Color color;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        color = GetComponent<Image>().color;
    }

    // Update is called once per frame
    void Update()
    {

        if (LevelManager.gameStart)
        {
            color.a -= 0.3f * Time.deltaTime;
            GetComponent<Image>().color = color;

            if (color.a <= 0)
            {
                Destroy(gameObject);
            }
        }

    }
}
