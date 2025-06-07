using System.Diagnostics.Contracts;
using UnityEngine;
using TMPro;
using UnityEngine.Windows;

public class ThoughtsSpawner : MonoBehaviour
{
    float timer;
    [SerializeField] GameObject thoughts;
    public int circumstance = 0;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timer = Random.Range(0, 5);
    }

    // Update is called once per frame
    void Update()
    {
        if (LevelManager.minigameStart)
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                timer = Random.Range(5, 7);
                if (LevelManager.thoughtsCount <= 2)
                {
                    LevelManager.thoughtsCount++;
                    Instantiate(thoughts, gameObject.transform.position, Quaternion.identity);
                    AudioManager.Instance.PlaySFX("lightGrass");
                }
            }
            LevelManager.circumstance = circumstance;
        }

        if (circumstance >= 2)
        {
            LevelManager.previousMinigameSucceed = 1;
            minigameEnd(-4, -2, -2, 1);
            AudioManager.Instance.PlaySFX("goodResult");
        }
        else if (circumstance <= -2)
        {
            LevelManager.previousMinigameSucceed = -1;
            minigameEnd(5, 2.5f, 3, -1);
            AudioManager.Instance.PlaySFX("badResult");
        }
    }

    void minigameEnd(float maxLight, float minLight, int lightOwn, int globalReputation)
    {
        LevelManager.minigameStart = false;
        LevelManager.maxLight += maxLight;
        LevelManager.minLight += minLight;
        LevelManager.lightOwn += lightOwn;
        LevelManager.globalReputation += globalReputation;
        circumstance = 0;
        AudioManager.Instance.PlayBGM("mainBGM");
        if (LevelManager.isImportantNPC)
        {
            LevelManager.countImportantNPC++;
            LevelManager.isImportantNPC = false;
        }
    }
}
