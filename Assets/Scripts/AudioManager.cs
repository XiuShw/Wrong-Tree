using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    public AudioSource bgmSource;
    //public AudioSource bgNoise;
    public AudioSource sfxSource1;
    public AudioSource sfxSource2;

    [Header("BGM Clips")]
    public AudioClip mainBGM;
    public AudioClip minigame;
    public AudioClip goodEnding;
    public AudioClip badEnding;


    [Header("Player Walk SFX")]
    public AudioClip walk;

    [Header("Other SFX")]
    public AudioClip thoughtCollision;
    public AudioClip badResult;
    public AudioClip goodResult;
    [SerializeField] AudioClip grass;
    [SerializeField] AudioClip lightGrass;
    public AudioClip placeholder;

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
            case "mainBGM": bgmSource.clip = mainBGM; bgmSource.volume = 0.1f; break;
            case "minigame": bgmSource.clip = minigame; bgmSource.volume = 0.1f; break;
            case "goodEnding": bgmSource.clip = goodEnding; bgmSource.volume = 0.1f; break;
            case "badEnding": bgmSource.clip = badEnding; bgmSource.volume = 0.1f; break;


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
            //case "walk": selectedClip1 = walk; volume1 = 1f; break;
            case "thoughtCollision": selectedClip1 = thoughtCollision; volume1 = 1f; pitch1 = Random.Range(0.5f, 2f); break;
            case "badResult": selectedClip1 = badResult; volume1 = 1f; break;
            case "goodResult": selectedClip1 = goodResult; volume1 = 1f; break;

            case "placeholder": selectedClip2 = placeholder; volume2 = 0.1f; break;
            case "grass": selectedClip2 = grass; volume2 = 1f; break;
            case "lightGrass": selectedClip2 = lightGrass; volume2 = 0.3f; break;
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
