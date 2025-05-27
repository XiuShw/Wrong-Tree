using UnityEngine;
using TMPro;

public class EnableMinigame : MonoBehaviour
{
    [SerializeField] TMP_Text canInteract;
    [SerializeField] GameObject minigamePlayer;
    [SerializeField] GameObject minigameBackground;

    private void OnMouseEnter()
    {
        if (!LevelManager.minigameStart)
        {
            canInteract.text = "'Click' to interact";
        }
    }

    private void OnMouseExit()
    {
        canInteract.text = "";
    }

    private void OnMouseDown()
    {
        if (!LevelManager.minigameStart)
        {
            LevelManager.minigameStart = true;
            minigamePlayer.transform.position = minigameBackground.transform.position;
        }
    }
}
