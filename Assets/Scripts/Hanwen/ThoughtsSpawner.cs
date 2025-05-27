using System.Diagnostics.Contracts;
using UnityEngine;
using TMPro;
using UnityEngine.Windows;

public class ThoughtsSpawner : MonoBehaviour
{
    float timer;
    [SerializeField] GameObject thoughts;
    [SerializeField] TMP_Text showCircumstance;
    public int circumstance = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timer = Random.Range(0, 5);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(timer);
        if (LevelManager.minigameStart)
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                timer = Random.Range(5, 7);
                if (LevelManager.thoughtsCount <= 3)
                {
                    LevelManager.thoughtsCount++;
                    Instantiate(thoughts, gameObject.transform.position, Quaternion.identity);
                }
            }

            showCircumstance.text = circumstance.ToString();
        }

        if (circumstance >= 2 || circumstance <= -2)
        {
            LevelManager.minigameStart = false;
            circumstance = 0;
            showCircumstance.text = "";
        }
    }
}
