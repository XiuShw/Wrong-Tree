using UnityEngine;

public class NPCProperties : MonoBehaviour
{
	[Header("NPC Properties")]
	public int npcID = 0; // Unique identifier for the NPC
	public bool simulationEnabled = false; // make this to be true when we want the NPC to be simulated
	private bool _lastSimulationEnabled = false; // Used to track changes in simulation state
	public float walkSpeed = 0.5f; // NPC's walking speed
	public float runSpeed = 1f; // NPC's running speed
	public float interactionRange = 1.5f; // Range for interaction with player
	public float shareCooldown = 3f; // Cooldown time for sharing action
	public float stealCooldown = 2f; // Cooldown time for stealing action
	public Vector2 anchorPosition;

	[Header("NPC State")]
	public NPCState currentState = NPCState.Idle; // Current state of the NPC
	private NPCState _lastState = NPCState.Idle; // Last state of the NPC, used for state change detection
	public bool narrativeEnabled = false;
	public NPCAtitude currentAtitude = NPCAtitude.Neutral; // Current attitude towards player or other NPCs

	[Header("NPC Bars")]
	public float lightValue = 1f;

	private NPCBehavior npcBehavior; // Reference to the NPCBehavior component

	void EnableNPC()
	{
		npcBehavior.SetState(_lastState);
		// 逻辑：一共有8个npc。
		// 如果reputation >= 3，所有态度都是分享
		// 如果reputation <= 0，所有态度都是偷窃
		// 如果reputation == 1，有4个分享，4个偷窃
		// 如果reputation == 2，有6个分享，2个偷窃
		if (LevelManager.globalReputation <= 0)
		{
			currentAtitude = NPCAtitude.Steal;
		}
		else if (LevelManager.globalReputation == 1)
		{
			if (npcID < 4)
			{
				currentAtitude = NPCAtitude.Share;
			}
			else
			{
				currentAtitude = NPCAtitude.Steal;
			}
		}
		else if (LevelManager.globalReputation == 2)
		{
			if (npcID < 6)
			{
				currentAtitude = NPCAtitude.Share;
			}
			else
			{
				currentAtitude = NPCAtitude.Steal;
			}
		}
		else if (LevelManager.globalReputation >= 3)
		{
			currentAtitude = NPCAtitude.Share;
		}
		else
		{
			Debug.LogWarning("Unknown global reputation: " + LevelManager.globalReputation);
			currentAtitude = NPCAtitude.Neutral; // Default attitude
		}
	}

	void DisableNPC()
	{
		// Reset the NPC properties when simulation is disabled
		_lastState = currentState; // Store the last state before disabling
		npcBehavior.SetState(NPCState.Paused); // Set the NPC to a paused state
	}

	private void Start()
	{
		// Initialize any necessary properties or states here
		anchorPosition = new Vector2(transform.position.x, transform.position.y);
		npcBehavior = GetComponent<NPCBehavior>(); // Get reference to NPCBehavior
	}

	private void Update()
	{
		// 
		if (!simulationEnabled && _lastSimulationEnabled)
		{
			DisableNPC();
		}
		else if (!_lastSimulationEnabled && simulationEnabled)
		{
			// If simulation was just enabled, set the initial attitude
			EnableNPC();
		}
		_lastSimulationEnabled = simulationEnabled;
	}
}