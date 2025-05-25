using UnityEngine;
using TMPro;

public class EnableMinigame : MonoBehaviour
{
    [SerializeField] TMP_Text canInteract;

    private void OnMouseEnter()
    {
        canInteract.text = "'Click' to interact";
    }

    private void OnMouseExit()
    {
        canInteract.text = "";
    }

    private void OnMouseDown()
    {
        Debug.Log("interact");
    }
}
