using System.Diagnostics.Contracts;
using UnityEngine;
using TMPro;
using UnityEngine.Windows;

public class ThoughtsSpawner : MonoBehaviour
{
    float timer;
    [SerializeField] GameObject thoughts;
    public int circumstance = 0;

    [SerializeField] int delay1up;
    [SerializeField] int delay1bottom;

    [SerializeField] int delay2up;
    [SerializeField] int delay2bottom;

    [SerializeField] int delay3up;
    [SerializeField] int delay3bottom;

    [SerializeField] int delay4up;
    [SerializeField] int delay4bottom;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timer = Random.Range(5, 10);
    }

    // Update is called once per frame
    void Update()
    {
        if (LevelManager.minigameStart)
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                timer = Random.Range(7, 12);
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
            minigameEnd(-5, -3, -1, 1);
            AudioManager.Instance.PlaySFX("goodResult");
            if (LevelManager.countImportantNPC >= 4 && LevelManager.globalReputation >= 4)
            {
                AudioManager.Instance.PlayBGM("goodEnding");
                LevelManager.gameFinished = true;

            }
            circumstance = 0;
        }
        else if (circumstance <= -2)
        {
            LevelManager.previousMinigameSucceed = -1;
            minigameEnd(5f, 3f, 1, -1);
            AudioManager.Instance.PlaySFX("badResult");
            circumstance = 0;
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
