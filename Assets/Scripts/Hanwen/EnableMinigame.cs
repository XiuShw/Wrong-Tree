using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EnableMinigame : MonoBehaviour
{
    [SerializeField] Image canInteract;
    [SerializeField] GameObject minigamePlayer;
    [SerializeField] GameObject minigameBackground;
    [SerializeField] FlashText argue1;
    //[SerializeField] GameObject speechBubble;
    //[SerializeField] GameObject exclaimationNotice;

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("NPC"))
        {
            EnableInteract();
        }
        if (collision.CompareTag("ImportantNPC"))
        {
            EnableInteract();
            LevelManager.isImportantNPC = true;
            collision.gameObject.transform.Find("speechBubble").gameObject.SetActive(false);
            collision.gameObject.transform.Find("exclaimationNotice").gameObject.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        canInteract.enabled = false;
    }

    void EnableInteract()
    {
        canInteract.enabled = true;

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (!LevelManager.isImportantNPC) { argue1.LoadNewText(5); }
            else { argue1.LoadNewText(LevelManager.countImportantNPC + 1); }
            LevelManager.minigameStart = true;
            AudioManager.Instance.PlaySFX("lightGrass");
            AudioManager.Instance.PlayBGM("minigame");
            minigamePlayer.transform.position = minigameBackground.transform.position;
        }
    }
}
