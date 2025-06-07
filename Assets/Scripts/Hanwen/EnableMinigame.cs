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
            EnableInteract(collision);
        }
        if (collision.CompareTag("ImportantNPC"))
        {
            EnableInteract(collision);
            LevelManager.isImportantNPC = true;
            collision.gameObject.transform.Find("speechBubble").gameObject.SetActive(false);
            collision.gameObject.transform.Find("exclaimationNotice").gameObject.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        canInteract.enabled = false;
    }

    void EnableInteract(Collider2D collision)
    {
        canInteract.enabled = true;
        LevelManager.previousMinigameSucceed = 0;
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (!LevelManager.isImportantNPC) { argue1.LoadNewText(5); }
            else 
            { 
                argue1.LoadNewText(LevelManager.countImportantNPC + 1);
                collision.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            }
            LevelManager.minigameStart = true;
            AudioManager.Instance.PlayBGM("minigame");
            minigamePlayer.transform.position = minigameBackground.transform.position;
        }
    }
}
