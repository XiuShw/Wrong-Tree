using TMPro;
using UnityEngine;

public class ApearOnFirst : MonoBehaviour
{
    TMP_Text guide;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        guide = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (LevelManager.countImportantNPC == 0 && LevelManager.minigameStart)
        {
            guide.text = "Move: WASD";
        }
        else
        {
            guide.text = "";
        }
    }
}
