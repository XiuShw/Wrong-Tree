using UnityEngine;
using UnityEngine.UI;

public class LightUI : MonoBehaviour
{
    Image lightUI;
    Color color;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lightUI = GetComponent<Image>();
        color = GetComponent<Image>().color;
    }

    // Update is called once per frame
    void Update()
    {
        lightUI.fillAmount = Mathf.MoveTowards(lightUI.fillAmount, LevelManager.lightOwn/10, 0.5f * Time.deltaTime);
        color.g = 0.3f + LevelManager.lightOwn / 10;
        color.b = 0.3f + LevelManager.lightOwn / 10;
        color.r = 0.3f + LevelManager.lightOwn / 10;
        GetComponent<Image>().color = color;
    }
}
