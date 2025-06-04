using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using TMPro;
using Yarn.Unity;
using UnityEngine.UI;
using UnityEngine.Experimental.GlobalIllumination;


public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    public static bool minigameStart;

    [SerializeField] GameObject minigameWindow;
    [SerializeField] Camera mainCamera;
    public static int circumstance = 0;

    public static float maxLight = 3f;
    public static float minLight = 1f;
    public static float lightOwn = 1;

    public static int thoughtsCount = 0;

    public static float globalReputation = 0;

    public static int countImportantNPC = 0;
    public static bool isImportantNPC = false;

    public static bool gameFinished = false;

    [SerializeField] FlashText argue1;

    [Header("On Game Finish")]
    [SerializeField] Image canInteract;
    [SerializeField] Image lightValue;
    [SerializeField] Image lightValueBG;
    [SerializeField] Entrance entrance;
    [SerializeField] Light globalLight;
    [SerializeField] Transform playerLight;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (minigameStart)
        { 
            minigameWindow.SetActive(true);
            mainCamera.GetUniversalAdditionalCameraData().renderPostProcessing = false;
            canInteract.enabled = false;
            lightValue.enabled = false;
            lightValueBG.enabled = false;
        }
        else 
        { 
            minigameWindow.SetActive(false);
            argue1.LoadNewText(0);
            mainCamera.GetUniversalAdditionalCameraData().renderPostProcessing = true;
            lightValue.enabled = true;
            lightValueBG.enabled = true;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameFinished = true;
        }

        if (gameFinished)
        {
            canInteract.enabled = false;
            lightValue.enabled = false;
            lightValueBG.enabled = false;
            entrance.spd = 0.05f;
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
