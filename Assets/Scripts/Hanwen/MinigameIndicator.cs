using TMPro;
using UnityEngine;

public class MinigameIndicator : MonoBehaviour
{
    Color color;
    TMP_Text textBox;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        textBox = GetComponent<TMP_Text>();
        color = GetComponent<TextMeshProUGUI>().color;
    }

    // Update is called once per frame
    void Update()
    {
        if (LevelManager.minigameStart)
        {
            if (LevelManager.circumstance == 0)
            {
                if (color.a >= 0) { color.a -= Time.deltaTime; }
            }
            else if (LevelManager.circumstance != 0)
            {
                if (color.a <= 1) { color.a += Time.deltaTime; }
                if (LevelManager.circumstance < 0)
                {
                    textBox.text = "Take";
                    if (color.g >= 0)
                    {
                        color.g -= Time.deltaTime;
                        color.b -= Time.deltaTime;
                    }
                }
                if (LevelManager.circumstance > 0)
                {
                    textBox.text = "Give";
                    if (color.g <= 1)
                    {
                        color.g += Time.deltaTime;
                        color.b += Time.deltaTime;
                    }
                }
            }
        }
        else
        {
            textBox.text = "";
        }
        GetComponent<TextMeshProUGUI>().color = color;
    }
}
