using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    public static bool minigameStart;
    [SerializeField] GameObject minigameWindow;
    [SerializeField] Camera mainCamera;

    public static int thoughtsCount = 0;


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
        }
        else 
        { 
            minigameWindow.SetActive(false);
            mainCamera.GetUniversalAdditionalCameraData().renderPostProcessing = true;
        }
    }
}
