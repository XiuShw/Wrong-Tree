using UnityEngine;
using Yarn.Unity;
using System.Collections;

public class MiniGameResult : MonoBehaviour
{
    [SerializeField] private DialogueRunner dialogueRunner;
    [SerializeField] private string winNode = "MinigameWinA";
    [SerializeField] private string loseNode = "MinigameLoseA";


    private bool dialogueTriggered = false;  // C# side tracking only

    private void Start()
    {
        dialogueRunner.VariableStorage.SetValue("$hasPlayedMinigame", false);
    }

    void Update()
    {
        // If minigame finished and dialogue not triggered yet
        if (!LevelManager.minigameStart && !dialogueTriggered)
        {
            if (LevelManager.previousMinigameSucceed == 1)
            {
                StartCoroutine(StartDialogueWithDelay(winNode));
                dialogueTriggered = true;
            }
            else if (LevelManager.previousMinigameSucceed == -1)
            {
                StartCoroutine(StartDialogueWithDelay(loseNode));
                dialogueTriggered = true;
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
