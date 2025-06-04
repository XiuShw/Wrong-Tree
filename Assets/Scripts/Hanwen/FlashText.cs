using NUnit.Framework;
using UnityEngine;
using System.Collections;
using TMPro;
using System.Collections.Generic;
using DG.Tweening.Core.Easing;
using UnityEngine.SceneManagement;


public class FlashText : MonoBehaviour
{
    [SerializeField] List<string> content0;
    [SerializeField] List<string> content1;
    [SerializeField] List<string> content2;
    [SerializeField] List<string> content3;
    [SerializeField] List<string> content4;
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
        workingContent = content0;
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

        if (SceneManager.GetActiveScene().buildIndex == 3 && Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
    }


    public void LoadNewText(int index)
    {
        if (index == 0) { workingContent = content0; }
        if (index == 1) { workingContent = content1; }
        if (index == 2) { workingContent = content2; }
        if (index == 3) { workingContent = content3; }
        if (index == 4) { workingContent = content4; }
        countText = 0;
        textBox.text = workingContent[countText];
        timeCounter = changeDuration;
        color.a = 0;
        GetComponent<TextMeshProUGUI>().color = color;
    }
}
