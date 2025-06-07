using NUnit.Framework;
using UnityEngine;
using System.Collections;
using TMPro;
using System.Collections.Generic;
using DG.Tweening.Core.Easing;
using UnityEngine.SceneManagement;


public class FlashText : MonoBehaviour
{
    [SerializeField] List<string> pleaseLeaveEmpty;
    [SerializeField] List<string> NPC1;
    [SerializeField] List<string> NPC2;
    [SerializeField] List<string> NPC3;
    [SerializeField] List<string> NPC4;
    [SerializeField] List<string> unimportantNPC;
    List<string> workingContent;
    TMP_Text textBox;
    bool canChange = false;
    int countText = 0;
    float timeCounter;
    Color color;
    [SerializeField] float changeDuration;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        workingContent = pleaseLeaveEmpty;
        timeCounter = changeDuration;
        textBox = GetComponent<TMP_Text>();
        color = GetComponent<TextMeshProUGUI>().color;
        textBox.text = workingContent[countText];
    }

    // Update is called once per frame
    void Update()
    {
        timeCounter -= Time.deltaTime;
        if (timeCounter < 0 && countText < workingContent.Count - 1)
        {
            color.a -= Time.deltaTime;
            GetComponent<TextMeshProUGUI>().color = color;
            if (color.a < 0.001)
            {
                countText++;
                textBox.text = workingContent[countText];
                timeCounter = changeDuration;
            }
        }
        else
        {
            color.a += Time.deltaTime;
            GetComponent<TextMeshProUGUI>().color = color;

        }

        if (SceneManager.GetActiveScene().buildIndex == 0 && Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        if ((SceneManager.GetActiveScene().buildIndex == 3 || SceneManager.GetActiveScene().buildIndex == 2) && Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }
    }


    public void LoadNewText(int index)
    {
        if (index == 0) { workingContent = pleaseLeaveEmpty; }
        if (index == 1) { workingContent = NPC1; }
        if (index == 2) { workingContent = NPC2; }
        if (index == 3) { workingContent = NPC3; }
        if (index == 4) { workingContent = NPC4; }
        if (index == 5) { workingContent = unimportantNPC; }
        countText = 0;
        textBox.text = workingContent[countText];
        timeCounter = changeDuration;
        color.a = 0;
        GetComponent<TextMeshProUGUI>().color = color;
    }
}
