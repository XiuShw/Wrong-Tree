using System.Diagnostics.Contracts;
using UnityEngine;
using TMPro;

public class ThoughtsSpawner : MonoBehaviour
{
    public int count = 0;
    float timer;
    [SerializeField] GameObject thoughts;
    [SerializeField] TMP_Text showCircumstance;
    public int circumstance = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instantiate(thoughts, gameObject.transform.position, Quaternion.identity);
        timer = Random.Range(5, 7);
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0 && count < 2)
        {
            count++;
            Instantiate(thoughts, gameObject.transform.position, Quaternion.identity);
            timer = Random.Range(5, 7);
        }

        showCircumstance.text = circumstance.ToString();
    }
}
