using UnityEngine;
using TMPro;

public class EnableMinigame : MonoBehaviour
{
    [SerializeField] TMP_Text canInteract;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("NPC")) { canInteract.text = "'E' to interact"; }
        else { canInteract.text = ""; }
    }
}
