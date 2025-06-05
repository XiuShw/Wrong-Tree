using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EnableMinigame : MonoBehaviour
{
    [SerializeField] Image canInteract;
    [SerializeField] GameObject minigamePlayer;
    [SerializeField] GameObject minigameBackground;
    [SerializeField] FlashText argue1;


    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("NPC"))
        {
            canInteract.enabled = true;

            if (Input.GetKeyDown(KeyCode.F))
            {
                if (!LevelManager.isImportantNPC) { argue1.LoadNewText(4); }
                else
                {
                    if (LevelManager.countImportantNPC == 0) { argue1.LoadNewText(1); }
                    if (LevelManager.countImportantNPC == 1) { argue1.LoadNewText(2); }
                    if (LevelManager.countImportantNPC == 2) { argue1.LoadNewText(3); }
                }
                LevelManager.minigameStart = true;
                AudioManager.Instance.PlaySFX("lightGrass");
                AudioManager.Instance.PlayBGM("minigame");
                minigamePlayer.transform.position = minigameBackground.transform.position;
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        canInteract.enabled = false;
    }
}
