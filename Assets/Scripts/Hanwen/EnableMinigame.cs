using UnityEngine;
using Yarn.Unity;

public class EnableMinigame : MonoBehaviour
{
    [Header("Minigame Setup")]
    [SerializeField] private GameObject minigamePlayer;
    [SerializeField] private GameObject minigameBackground;
    [SerializeField] private FlashText argue1;

    [Header("UI Elements")]
    [SerializeField] private GameObject speechBubble;
    [SerializeField] private GameObject exclaimationNotice;
    [SerializeField] private GameObject talkPrompt;

    [Header("Yarn")]
    [SerializeField] private DialogueRunner dialogueRunner;

    private bool minigameFinished = false;
    private bool minigameWon = false;

    private GameObject currentImportantNPC = null;
    private bool isDialogueRunning = false;

    // --- New minigame logic ---
    private bool minigameActive = false;
    private float minigameTimer = 5f; // example: 5 seconds to complete minigame
    private float minigameElapsed = 0f;

    void Start()
    {
        if (dialogueRunner != null)
        {
            dialogueRunner.onDialogueStart.AddListener(() => isDialogueRunning = true);
            dialogueRunner.onDialogueComplete.AddListener(() => isDialogueRunning = false);
        }
    }

    void Update()
    {
        // Debug test keys to immediately end minigame
        if (Input.GetKeyDown(KeyCode.M)) OnMinigameEnd(true);
        if (Input.GetKeyDown(KeyCode.N)) OnMinigameEnd(false);

        // Minigame running logic
        if (minigameActive && !minigameFinished)
        {
            minigameElapsed += Time.deltaTime;

            // Example condition: player "wins" if timer reaches minigameTimer
            if (minigameElapsed >= minigameTimer)
            {
                OnMinigameEnd(true); // Player won by lasting the whole time
            }

            // (You can replace this with your own minigame conditions!)
            // For example, listen to player input or minigame states here
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("NPC"))
        {
            EnableInteract(collision);
        }
        else if (collision.CompareTag("ImportantNPC"))
        {
            LevelManager.isImportantNPC = true;
            currentImportantNPC = collision.gameObject;

            if (!minigameFinished)
                ShowExclamationAndPrompt(currentImportantNPC);
            else
                ShowDialogueBubble(currentImportantNPC);

            EnableInteract(collision);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("ImportantNPC"))
        {
            LevelManager.isImportantNPC = false;
            HideExclamationAndPrompt(collision.gameObject);
            HideDialogueBubble(collision.gameObject);
            currentImportantNPC = null;
        }
    }

    void EnableInteract(Collider2D collision)
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isDialogueRunning)
        {
            if (!LevelManager.isImportantNPC)
            {
                argue1.LoadNewText(5);
            }
            else if (!minigameFinished)
            {
                // Save the NPC reference explicitly here
                currentImportantNPC = collision.gameObject;

                // Start minigame
                LevelManager.previousMinigameSucceed = 0;
                LevelManager.minigameStart = true;
                AudioManager.Instance?.PlayBGM("minigame");

                minigamePlayer.transform.position = minigameBackground.transform.position;

                HideExclamationAndPrompt(collision.gameObject);

                collision.GetComponent<BoxCollider2D>().enabled = false;

                // Start minigame logic
                minigameActive = true;
                minigameElapsed = 0f;

                Debug.Log("Minigame started!");
            }
        }
    }


    public void OnMinigameEnd(bool won)
    {
        if (minigameFinished) return; // prevent multiple calls

        Debug.Log("OnMinigameEnd called, won = " + won);

        minigameFinished = true;
        minigameWon = won;

        minigameActive = false; // stop minigame logic

        if (currentImportantNPC == null)
        {
            Debug.LogWarning("No currentImportantNPC when minigame ended.");
            return;
        }

        ShowDialogueBubble(currentImportantNPC);

        if (dialogueRunner != null)
        {
            string node = won ? "MinigameWinA" : "MinigameLoseA";
            dialogueRunner.StartDialogue(node);
        }
        else
        {
            Debug.LogWarning("DialogueRunner is not assigned!");
        }
    }

    // Call these if you want to trigger minigame results from elsewhere
    public void PlayerWinsMinigame() => OnMinigameEnd(true);
    public void PlayerLosesMinigame() => OnMinigameEnd(false);

    void ShowExclamationAndPrompt(GameObject npc)
    {
        var bubble = npc.transform.Find("NPC_A_Speech/Canvas/speechBubble");
        var exclaim = npc.transform.Find("NPC_A_Speech/Canvas/exclaimationNotice");

        if (bubble != null) bubble.gameObject.SetActive(false);
        if (exclaim != null) exclaim.gameObject.SetActive(true);
        if (talkPrompt != null) talkPrompt.SetActive(true);
    }

    void HideExclamationAndPrompt(GameObject npc)
    {
        var exclaim = npc.transform.Find("NPC_A_Speech/Canvas/exclaimationNotice");

        if (exclaim != null) exclaim.gameObject.SetActive(false);
        if (talkPrompt != null) talkPrompt.SetActive(false);
    }

    void ShowDialogueBubble(GameObject npc)
    {
        var bubble = npc.transform.Find("NPC_A_Speech/Canvas/speechBubble");
        var exclaim = npc.transform.Find("NPC_A_Speech/Canvas/exclaimationNotice");

        if (bubble != null) bubble.gameObject.SetActive(true);
        if (exclaim != null) exclaim.gameObject.SetActive(false);
        if (talkPrompt != null) talkPrompt.SetActive(false);
    }

    void HideDialogueBubble(GameObject npc)
    {
        var bubble = npc.transform.Find("NPC_A_Speech/Canvas/speechBubble");
        if (bubble != null) bubble.gameObject.SetActive(false);
    }
}
