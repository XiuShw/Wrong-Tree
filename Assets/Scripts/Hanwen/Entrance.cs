using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Entrance : MonoBehaviour
{
    Color color;
    public float spd;
    public string endingScene;
    [SerializeField] float delay = 0;
    [SerializeField] bool isText;
    float count;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (isText)
        {
            color = GetComponent<TextMeshProUGUI>().color;
        }
        else
        {
            color = GetComponent<Image>().color;

        }
        count = delay;
    }

    // Update is called once per frame
    void Update()
    {
        if (delay != 0)
        {
            count -= Time.deltaTime;
        }

        if (count <= 0 && !(color.a <= -0.1 || color.a >= 1.1))
        {

            color.a += spd * Time.deltaTime;

            if (isText)
            {
                GetComponent<TextMeshProUGUI>().color = color;
            }
            else
            {
                GetComponent<Image>().color = color;
            }
        }

        if (color.a < 0)
        {
            color.a = 0;
        }
        if (color.a > 1.1)
        {
            color.a = 1;
        }

        if (!isText && color.a == 1 && SceneManager.GetActiveScene().buildIndex == 2)
        {
            SceneManager.LoadScene(endingScene);
        }

        if (isText && color.a == 1 && SceneManager.GetActiveScene().buildIndex == 1)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                SceneManager.LoadScene(5);
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
        }
    }
}
