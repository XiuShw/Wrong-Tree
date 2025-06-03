using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using TMPro;
using Yarn.Unity;


public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }
    public static bool gameStart = false;

    public static bool minigameStart;

    [SerializeField] GameObject minigameWindow;
    [SerializeField] Camera mainCamera;
    [SerializeField] TMP_Text showLight;
    [SerializeField] TMP_Text showCircumstance;
    public int circumstance = 0;

    public static float maxLight = 3f;
    public static float minLight = 1f;
    public static int lightOwn = 1;

    public static int thoughtsCount = 0;

    public static float globalReputation = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        showLight.text = "Light: " + lightOwn;

        if (minigameStart)
        { 
            minigameWindow.SetActive(true);
            mainCamera.GetUniversalAdditionalCameraData().renderPostProcessing = false;

        }
        else 
        { 
            minigameWindow.SetActive(false);
            mainCamera.GetUniversalAdditionalCameraData().renderPostProcessing = true;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            gameStart = true;
        }

    }


    [YarnCommand("ActivateMinigame")]
    public static void ActivateMinigame()
    {
        minigameStart = true;
    }
}
