using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EnableMinigame : MonoBehaviour
{
    [SerializeField] GameObject minigamePlayer;
    [SerializeField] GameObject minigameBackground;
    [SerializeField] FlashText argue1;
    //[SerializeField] GameObject speechBubble;
    //[SerializeField] GameObject exclaimationNotice;

    // void OnTriggerStay2D(Collider2D collision)
    // {
    //     if (collision.CompareTag("NPC"))
    //     {
    //         EnableInteract(collision);
    //     }
    //     if (collision.CompareTag("ImportantNPC"))
    //     {
    //         EnableInteract(collision);
    //         LevelManager.isImportantNPC = true;
    //         collision.gameObject.transform.Find("NPC_A_Speech/Canvas/speechBubble").gameObject.SetActive(false);
    //         collision.gameObject.transform.Find("NPC_A_Speech/Canvas/exclaimationNotice").gameObject.SetActive(true);
    //     }
    // }

    // void OnTriggerExit2D(Collider2D collision)
    // {
    //     if (collision.CompareTag("ImportantNPC"))
    //     {
    //         LevelManager.isImportantNPC = false;
    //         collision.gameObject.transform.Find("Pop up").gameObject.SetActive(false);
    //         collision.gameObject.transform.Find("NPC_A_Speech/Canvas/speechBubble").gameObject.SetActive(true);
    //         collision.gameObject.transform.Find("NPC_A_Speech/Canvas/exclaimationNotice").gameObject.SetActive(false);
    //     }
    // }

    // void EnableInteract(Collider2D collision)
    // {
    //     // collision.gameObject.transform.Find("Pop up").gameObject.SetActive(true); // I found that there is an error. I commented this out. - Ken
    //     LevelManager.previousMinigameSucceed = 0;
    //     if (Input.GetKeyDown(KeyCode.Space))
    //     {
    //         if (!LevelManager.isImportantNPC) { argue1.LoadNewText(5); }
    //         else 
    //         { 
    //             argue1.LoadNewText(LevelManager.countImportantNPC + 1);
    //             collision.gameObject.GetComponent<BoxCollider2D>().enabled = false;
    //             collision.gameObject.transform.Find("NPC_A_Speech/Canvas/speechBubble").gameObject.SetActive(true);
    //             collision.gameObject.transform.Find("NPC_A_Speech/Canvas/exclaimationNotice").gameObject.SetActive(false);
    //         }
    //         LevelManager.minigameStart = true;
    //         AudioManager.Instance.PlayBGM("minigame");
    //         minigamePlayer.transform.position = minigameBackground.transform.position;
    //     }
    // }
}
