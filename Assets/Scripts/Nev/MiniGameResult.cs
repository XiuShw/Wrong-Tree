using UnityEngine;
using Yarn.Unity;
using System.Collections;

public class MiniGameResult : MonoBehaviour
{
    [SerializeField] private DialogueRunner dialogueRunner;
    [SerializeField] int NPCnumber;
    [SerializeField] private string winNode = "MinigameWinA";
    [SerializeField] private string loseNode = "MinigameLoseA";
    public float maxlight;
    public float minlight;

    private bool dialogueTriggered = false;  // C# side tracking only

    private void Start()
    {
        dialogueRunner.VariableStorage.SetValue("$hasPlayedMinigame", false);
        maxlight = 5;
        minlight = 3;
    }

    void Update()
    {
        // If minigame finished and dialogue not triggered yet
        if (!LevelManager.minigameStart && !dialogueTriggered && NPCnumber == LevelManager.countImportantNPC)
        {
            if (LevelManager.previousMinigameSucceed == 1)
            {
                StartCoroutine(StartDialogueWithDelay(winNode));
                dialogueTriggered = true;
                
                maxlight = 10;
                minlight = 6;
                Debug.Log("111: " + maxlight + minlight);
            }
            else if (LevelManager.previousMinigameSucceed == -1)
            {
                StartCoroutine(StartDialogueWithDelay(loseNode));
                dialogueTriggered = true;
                maxlight = 0;
                minlight = 0;
                Debug.Log("222: " + maxlight + minlight);
            }
        }
    }

    private IEnumerator StartDialogueWithDelay(string nodeName)
    {
        yield return new WaitForSeconds(0.2f); // small delay to avoid input issues
        dialogueRunner.StartDialogue(nodeName);

        // Set the Yarn variable so your Yarn script knows dialogue played
        dialogueRunner.VariableStorage.SetValue("$hasPlayedMinigame", true);
    }
}
