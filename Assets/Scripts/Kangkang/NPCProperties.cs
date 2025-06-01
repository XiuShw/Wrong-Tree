using UnityEngine;

public class NPCProperties : MonoBehaviour
{
	[Header("NPC Properties")]
	public float walkSpeed = 2f; // NPC's walking speed
	public float runSpeed = 5f; // NPC's running speed
	public float interactionRange = 1.5f; // Range for interaction with player
	public float shareCooldown = 3f; // Cooldown time for sharing action
	public float stealCooldown = 2f; // Cooldown time for stealing action

	[Header("NPC State")]
	public NPCBehavior.NPCState currentState = NPCBehavior.NPCState.Idle; // Current state of the NPC

	[Header("NPC Bars")]
	public float health = 100f;
	public float lightValue = 1f;

	private void Start()
	{
		// Initialize any necessary properties or states here
	}

	private void Update()
	{
		// Update logic based on the current state can be added here if needed
	}
}