using UnityEngine;
using TMPro;

public class NPCText : MonoBehaviour
{
	[SerializeField] private TMP_Text npcStateText; // Text to display the current state of the NPC
	private NPCBehavior npcBehavior;

	void Start()
	{
		if (npcStateText == null)
		{
			Debug.LogError("NPCStateText is not assigned in the inspector!");
		}
		npcBehavior = GetComponent<NPCBehavior>();
		if (npcBehavior == null)
		{
			Debug.LogError("NPCBehavior component not found on this GameObject!");
		}
		if (npcStateText != null && npcBehavior != null)
		{
			npcStateText.text = npcBehavior.GetState().ToString();
		}
	}

	void Update()
	{
		if (npcStateText == null || npcBehavior == null)
		{
			return;
		}
		// Update the text to show the current state of the NPC
		npcStateText.text = npcBehavior.GetState().ToString();

		// 将文本固定在屏幕右上角
		if (Camera.main != null)
		{
			Vector3 screenPos = new Vector3(Screen.width - 150, Screen.height - 50, 0); // 距离右上角一定像素
			npcStateText.transform.position = screenPos;
		}
	}
}