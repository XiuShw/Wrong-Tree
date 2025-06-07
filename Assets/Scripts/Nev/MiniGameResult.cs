using UnityEngine;
using Yarn.Unity;

public class MiniGameResult : MonoBehaviour
{
    [SerializeField] private DialogueRunner dialogueRunner;
    [SerializeField] private string winNode = "MinigameWin";
    [SerializeField] private string loseNode = "MinigameLose";

    private bool dialogueTriggered = false;

    void Update()
    {
        // Only trigger once, right after minigame ends
        if (!LevelManager.minigameStart && !dialogueTriggered)
        {
            if (LevelManager.previousMinigameSucceed == 1)
            {
                dialogueRunner.StartDialogue(winNode);
                dialogueRunner.VariableStorage.SetValue("$hasPlayedMinigame", true);
                dialogueTriggered = true;
                enabled = false; // permanently stop this script
            }
            else if (LevelManager.previousMinigameSucceed == -1)
            {
                dialogueRunner.StartDialogue(loseNode);
                dialogueRunner.VariableStorage.SetValue("$hasPlayedMinigame", true);
                dialogueTriggered = true;
                enabled = false; // permanently stop this script
            }
        }
    }

}
