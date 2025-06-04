using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    public AudioSource bgmSource;
    public AudioSource sfxSource1;
    public AudioSource sfxSource2;

    [Header("BGM Clips")]
    public AudioClip game;
    public AudioClip gameover;
    public AudioClip gameWin;

    [Header("SFX Clips 1")]
    public AudioClip pew;
    public AudioClip explode;
    public AudioClip losehealth;

    [Header("SFX Clips 2")]
    [SerializeField] AudioClip inputCorrect;
    [SerializeField] AudioClip inputIncorrect;
    [SerializeField] AudioClip inputSuccess;
    [SerializeField] AudioClip optionOn;
    public AudioClip message;

    private string currentBGM = "";
    private bool canPlaySFX = true;
   
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        string scene = SceneManager.GetActiveScene().name;
    }

    private void Start()
    {
        bgmSource.loop = true;
        PlayBGM("game");
    }

    private void Update()
    {
        string scene = SceneManager.GetActiveScene().name;
    }

    public void PlayBGM(string clip)
    {
        switch (clip)
        {
            case "game": bgmSource.clip = game; bgmSource.volume = 0.1f; break;
            case "gameover": bgmSource.clip = gameover; bgmSource.volume = 0.2f; bgmSource.loop = false; break;
            case "gameWin": bgmSource.clip = gameWin; bgmSource.volume = 0.5f; bgmSource.loop = false; break;
            default: return;
        }

        bgmSource.Play();
    }

    public void PlaySFX(string clip)
    {
        if (!canPlaySFX) return;

        AudioClip selectedClip1 = null;
        AudioClip selectedClip2 = null;
        
        float volume1 = 1f;
        float volume2 = 1f;
        float pitch1 = 1f;
        float pitch2 = 1f;

        switch (clip)
        {
            //1: for shooter minigame
            case "pew": selectedClip1 = pew; volume1 = 0.1f; pitch1 = Random.Range(0.5f, 2f); break;
            case "explode": selectedClip1 = explode; volume1 = 0.2f; break;
            case "losehealth": selectedClip1 = losehealth; volume1 = 0.3f; break;
            //2: for messaging
            case "optionOn": selectedClip2 = optionOn; break;
            case "inputCorrect": selectedClip2 = inputCorrect; break;
            case "inputIncorrect": selectedClip2 = inputIncorrect; break;
            case "inputSuccess": selectedClip2 = inputSuccess; break;
            case "message": selectedClip2 = message; break;
            default: return;
        }

        sfxSource1.PlayOneShot(selectedClip1, volume1);
        sfxSource1.pitch = pitch1;
        sfxSource2.PlayOneShot(selectedClip2, volume2);
        sfxSource2.pitch = pitch2;
    }

    private IEnumerator EnableSFXAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        canPlaySFX = true;
    }
}
