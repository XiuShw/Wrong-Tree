using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using TMPro;
using Yarn.Unity;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Experimental.GlobalIllumination;


public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    public static bool minigameStart;

    [SerializeField] GameObject minigameWindow;
    [SerializeField] GameObject minigameBG;
    [SerializeField] Camera mainCamera;
    public static int circumstance = 0;

    public static float maxLight = 5f;
    public static float minLight = 3f;
    public static float lightOwn = 1;

    public static int thoughtsCount = 0;

    public static float globalReputation = 0;

    public static int countImportantNPC = 0;
    public static bool isImportantNPC = false;

    public static bool gameFinished = false;
    public static int previousMinigameSucceed = 0;

    [SerializeField] FlashText argue1;

    [Header("On Game Finish")]
    [SerializeField] Image canInteract;
    [SerializeField] Image lightValue;
    [SerializeField] Image lightValueBG;
    [SerializeField] Entrance entrance;
    [SerializeField] Light globalLight;
    [SerializeField] Transform playerLight;

    Color color;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        maxLight = 5f;
        minLight = 3f;
        lightOwn = 1;

        thoughtsCount = 0;

        globalReputation = 0;

        countImportantNPC = 0;
        isImportantNPC = false;

        gameFinished = false;
        previousMinigameSucceed = 0;
        minigameStart = false;
    }

    void Start()
    {
        AudioManager.Instance.PlayBGM("mainBGM");
    }

    // Update is called once per frame
    void Update()
    {

        if (minigameStart)
        { 
            if (minigameBG.GetComponent<SpriteRenderer>().color.a <= 1)
            {
                color = minigameBG.GetComponent<SpriteRenderer>().color;
                color.a += Time.deltaTime;
                minigameBG.GetComponent<SpriteRenderer>().color = color;
            }
            minigameWindow.SetActive(true);
            mainCamera.GetUniversalAdditionalCameraData().renderPostProcessing = false;
            canInteract.enabled = false;
            lightValue.enabled = false;
            lightValueBG.enabled = false;
        }
        else 
        {
            if (minigameBG.GetComponent<SpriteRenderer>().color.a >= 0)
            {
                color = minigameBG.GetComponent<SpriteRenderer>().color;
                color.a -= Time.deltaTime;
                minigameBG.GetComponent<SpriteRenderer>().color = color;
            }
            minigameWindow.SetActive(false);
            argue1.LoadNewText(0);
            mainCamera.GetUniversalAdditionalCameraData().renderPostProcessing = true;
            lightValue.enabled = true;
            lightValueBG.enabled = true;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameFinished = true;
            if (globalReputation >= 4)
            {
                entrance.endingScene = "Good End";
                AudioManager.Instance.PlayBGM("goodEnding");
            }
            if (globalReputation < 4)
            {
                Debug.Log(111);
                entrance.endingScene = "Bad End";
                AudioManager.Instance.PlayBGM("badEnding");
            }

        }

        if (gameFinished)
        {



            canInteract.enabled = false;
            lightValue.enabled = false;
            lightValueBG.enabled = false;
            entrance.spd = 0.048f;
            playerLight.position += Vector3.back * Time.deltaTime;
            //globalLight.enabled = true;
            if (globalLight.transform.rotation.y < -0.25)
            {
                globalLight.transform.Rotate(0, 10 * Time.deltaTime, 0, Space.Self);
            }
        }
    }


    [YarnCommand("ActivateMinigame")]
    public static void ActivateMinigame()
    {
        minigameStart = true;
    }
}
