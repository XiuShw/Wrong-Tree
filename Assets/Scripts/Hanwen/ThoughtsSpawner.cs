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
                }
            }
            LevelManager.circumstance = circumstance;
        }

        if (circumstance >= 2)
        {
            LevelManager.minigameStart = false;
            LevelManager.maxLight -= 4f;
            LevelManager.minLight -= 2f;
            LevelManager.globalReputation += 1;
            circumstance = 0;
            AudioManager.Instance.PlaySFX("goodResult");
            AudioManager.Instance.PlayBGM("mainBGM");

        }
        else if (circumstance <= -2)
        {
            LevelManager.minigameStart = false;
            LevelManager.maxLight += 5f;
            LevelManager.minLight += 2.5f;
            LevelManager.lightOwn += 3;
            LevelManager.globalReputation -= 1;
            circumstance = 0;
            AudioManager.Instance.PlaySFX("badResult");
            AudioManager.Instance.PlayBGM("mainBGM");

        }
    }
}
