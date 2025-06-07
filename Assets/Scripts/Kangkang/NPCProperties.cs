using UnityEngine;

public class NPCProperties : MonoBehaviour
{
	[Header("NPC Properties")]
	public float walkSpeed = 0.5f; // NPC's walking speed
	public float runSpeed = 1f; // NPC's running speed
	public float interactionRange = 1.5f; // Range for interaction with player
	public float shareCooldown = 3f; // Cooldown time for sharing action
	public float stealCooldown = 2f; // Cooldown time for stealing action
	public Vector2 anchorPosition;

	[Header("NPC State")]
	public NPCState currentState = NPCState.Idle; // Current state of the NPC
	public bool narrativeEnabled = false;
	public NPCAtitude currentAtitude = NPCAtitude.Neutral; // Current attitude towards player or other NPCs

	[Header("NPC Bars")]
	public float lightValue = 1f;

	private void Start()
	{
		// Initialize any necessary properties or states here
		anchorPosition = new Vector2(transform.position.x, transform.position.y);
	}

	private void Update()
	{
		// Update logic based on the current state can be added here if needed
	}
}