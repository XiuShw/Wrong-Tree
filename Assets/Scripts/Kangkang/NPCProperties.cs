using UnityEngine;

public class NPCProperties : MonoBehaviour
{
	[Header("NPC Properties")]
	public int npcID = 0; // Unique identifier for the NPC
	public float walkSpeed = 3f;
	public float runSpeed = 5f;
	public float decideRange = 5f; // 决定范围超参数：NPC在这个范围内会决定是否与玩家分享或偷窃
	public float shareVisionRange = 5f; // 可视距离超参数：追逐过程中如果距离超过了这个参数，则NPC会停止追逐
	public float stealAggroRange = 5f; // 仇恨距离超参数
	public float interactionRange = 0.05f; // 互动距离超参数：NPC与其它对象之间的距离小于这个值时，NPC会与其分享/偷窃

	public Vector2 anchorPosition;
	public float maxWanderDistance = 10f; // Maximum distance from anchor for wandering

	[Header("NPC State")]
	public NPCState currentState = NPCState.Idle; // Current state of the NPC
	private NPCState _lastState = NPCState.Idle; // Last state of the NPC, used for state change detection
	public bool narrativeEnabled = false;
	public NPCAtitude currentAtitude = NPCAtitude.Neutral; // Current attitude towards player or other NPCs

	[Header("NPC Bars")]
	public int lightValue = 1; // 0 - no light, 1 - some light, 2 - full light
	public void SetLightValue(int value)
	{
		lightValue = value switch
		{
			0 => 0, // No light
			1 => 1, // Some light
			2 => 2, // Full light
			_ => throw new System.ArgumentOutOfRangeException(nameof(value), $"Invalid light value: {value}"),
		};
		// 找到灯光子物件
		Transform lightTransform = transform.Find("Point Light");
		if (lightTransform == null)
		{
			Debug.LogWarning("Point Light not found in NPC: " + name);
			return;
		}
		Light light = lightTransform.GetComponent<Light>();
		NPCFlickeringLight flickeringLight = lightTransform.GetComponent<NPCFlickeringLight>();
		if (light == null)
		{
			Debug.LogWarning("Light component not found in NPC: " + name);
			return;
		}
		if (npcBehavior.IAmPlayer)
		{
			LevelManager.minLight = value switch
			{
				0 => 0f, // No light
				1 => 3f, // Some light
				2 => 18f, // Full light
				_ => light.intensity // Default to current intensity if value is out of range
			};
			LevelManager.maxLight = value switch
			{
				0 => 0f, // No light
				1 => 3f, // Some light
				2 => 20f, // Full light
				_ => light.intensity // Default to current intensity if value is out of range
			};
		}
		else
		{
			flickeringLight.minIntensity = value switch
			{
				0 => 0f, // No light
				1 => 3f, // Some light
				2 => 18f, // Full light
				_ => light.intensity // Default to current intensity if value is out of range
			};
			flickeringLight.maxIntensity = value switch
			{
				0 => 0f, // No light
				1 => 3f, // Some light
				2 => 20f, // Full light
				_ => light.intensity // Default to current intensity if value is out of range
			};
		}
	}

	private NPCBehavior npcBehavior; // Reference to the NPCBehavior component

	public void EnableNPC()
	{
		if (npcBehavior.IAmPlayer) return; // If this is the player, do not enable NPC
		npcBehavior.SetState(_lastState);
		// 逻辑：一共有8个npc。
		// 如果reputation >= 3，4个分享，4个中立
		// 如果reputation <= 0，8个偷窃
		// 如果reputation == 1，有2个分享，4个中立，2个偷窃
		// 如果reputation == 2，有2个分享，5个中立，1个偷窃
		// if (LevelManager.globalReputation <= 0)
		// {
		// 	currentAtitude = NPCAtitude.Steal;
		// }
		// else if (LevelManager.globalReputation == 1)
		// {
			if (npcID < 2)
			{
				currentAtitude = NPCAtitude.Share;
			}
			else if (npcID < 6)
			{
				currentAtitude = NPCAtitude.Neutral;
			}
			else
			{
				currentAtitude = NPCAtitude.Steal;
			}
		// }
		// else if (LevelManager.globalReputation == 2)
		// {
		// 	if (npcID < 6)
		// 	{
		// 		currentAtitude = NPCAtitude.Share;
		// 	}
		// 	else
		// 	{
		// 		currentAtitude = NPCAtitude.Steal;
		// 	}
		// }
		// else if (LevelManager.globalReputation >= 3)
		// {
		// 	currentAtitude = NPCAtitude.Share;
		// }
		// else
		// {
		// 	Debug.LogWarning("Unknown global reputation: " + LevelManager.globalReputation);
		// 	currentAtitude = NPCAtitude.Neutral; // Default attitude
		// }
	}

	public void DisableNPC()
	{
		if (npcBehavior.IAmPlayer) return; // If this is the player, do not disable NPC
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
		;
	}
}