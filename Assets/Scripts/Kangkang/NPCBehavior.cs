// filepath: c:\Users\IWMAI\OneDrive - University of Toronto\Programs\Wrong-Tree\Assets\Scripts\Kangkang\NPCBehavior.cs
using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// The list of possible states for the NPC
public enum NPCState
{
	Idle = 0,
	Wander = 1,
	Smile = 2, // Aka. being shared
	Flee = 3, // Aka. being stolen
	Interact_Share = 4,
	Interact_Steal = 5,
	Dead = 6,
	Paused = 7,
	Crying = 8
}

// The list of possible attitudes the NPC can have towards other NPCs or the player
public enum NPCAtitude
{
	Neutral = 0,
	Share = 1,
	Steal = 2
}

public class NPCBehavior : MonoBehaviour
{

	public NPCProperties properties; // Reference to the NPCProperties component


	// A list storing all NPCs in the scene
	public static List<NPCBehavior> allNPCs = new List<NPCBehavior>();

	void OnEnable() => allNPCs.Add(this);
	void OnDisable() => allNPCs.Remove(this);

	public void SetState(NPCState newState)
	{
		CurrentState = newState;
		justChangedState = true; // Set the flag to true when changing state
		animator.SetInteger("State", (int)newState);
		// Update NPCProperties current state
		if (properties != null)
		{
			properties.currentState = newState;
		}
		// Debug.Log($"NPC state changed to: {newState}"); // Log the state change
	}

	// Some variables / properties
	public NPCState CurrentState { get; private set; } = NPCState.Idle; // Default state is Idle
	private bool justChangedState = true; // Flag to check if the state just changed
	private Animator animator;
	// Use properties from NPCProperties instead of local variables
	public float WalkSpeed => properties.walkSpeed;
	public float RunSpeed => properties.runSpeed;
	[SerializeField] private float timer; // for timing different states
	[SerializeField] private Vector2 walkDirection = new Vector2(1, 0); // Current walking direction
	[SerializeField] private float maxWanderDistance = 5f; // Maximum distance from anchor for wandering
	[SerializeField][Range(0f, 1f)] private float wanderBias = 0.5f; // Bias towards anchor when wandering
	public Vector2 AnchorPosition => properties != null ? properties.anchorPosition : new Vector2(transform.position.x, transform.position.y);

	public float nearestNPCDistance = float.MaxValue; // Distance to the nearest NPC
	public NPCBehavior GetNearestNPC()
	{
		NPCBehavior nearest = null;
		float minDist = float.MaxValue;
		Vector2 myPos = transform.position;

		foreach (var other in allNPCs)
		{
			if (other == this) continue;
			float d = Vector2.Distance(myPos, other.transform.position);
			if (d < minDist)
			{
				minDist = d;
				nearest = other;
			}
		}
		nearestNPCDistance = minDist; // Update the nearest NPC distance
		return nearest;
	}

	public float GetNearestNPCDistance()
	{
		NPCBehavior nearest = GetNearestNPC();
		if (nearest == null)
		{
			nearestNPCDistance = float.MaxValue;
			return nearestNPCDistance;
		}
		nearestNPCDistance = Vector2.Distance(transform.position, nearest.transform.position);
		return nearestNPCDistance;
	}

	void Start()
	{
		animator = GetComponent<Animator>();
		properties = GetComponent<NPCProperties>(); // Get reference to NPCProperties
		if (properties == null)
		{
			Debug.LogWarning("NPCProperties component not found on NPC. Using default values.");
		}
		SetState(NPCState.Idle); // Start with Idle state
	}

	// Update is called once per frame
	void Update()
	{
		if (properties == null)
		{
			Debug.LogWarning("NPCProperties is null. Cannot update state.");
			return; // Exit if properties are not set
		}
		if (CurrentState == NPCState.Paused)
		{
			// If in narrative state, do not update other states
			return;
		}
		// Update distance to nearest NPC each frame
		nearestNPCDistance = GetNearestNPCDistance();
		timer -= Time.deltaTime;
		bool firstFrameInState = justChangedState;
		justChangedState = false; // Reset the flag for the next frame
		switch (CurrentState)
		{
			case NPCState.Paused:
				break;
			case NPCState.Idle:
				// do nothing
				if (firstFrameInState)
				{
					timer = UnityEngine.Random.Range(1f, 2f);
				}
				if (timer <= 0)
				{
					SetState(NPCState.Wander);
				}
				break;
			case NPCState.Wander:
				// 随机游走
				if (firstFrameInState)
				{
					timer = UnityEngine.Random.Range(1f, 2f);
					// Calculate biased random direction towards anchor
					Vector2 randomDir = NPCStateUtils.GetRandomWalkDirection();
					Vector2 toAnchor = (AnchorPosition - (Vector2)transform.position).normalized;
					if (Vector2.Distance(transform.position, AnchorPosition) > maxWanderDistance)
						walkDirection = toAnchor;
					else
						walkDirection = Vector2.Lerp(randomDir, toAnchor, wanderBias).normalized;
				}
				// Move NPC and clamp within max distance
				{
					Vector2 currentPos2D = new Vector2(transform.position.x, transform.position.y);
					Vector2 newPos2D = currentPos2D + walkDirection * WalkSpeed * Time.deltaTime;
					Vector2 offset = newPos2D - AnchorPosition;
					if (offset.magnitude > maxWanderDistance)
					{
						newPos2D = AnchorPosition + offset.normalized * maxWanderDistance;
						walkDirection = (AnchorPosition - currentPos2D).normalized;
					}
					transform.position = new Vector3(newPos2D.x, newPos2D.y, transform.position.z);
				}
				if (timer <= 0)
				{
					NPCState _nextState = NPCStateUtils.DecideIdleShareSteal(this, properties);
					SetState(_nextState);
				}
				break;
			case NPCState.Smile:
				// 被分享
				if (firstFrameInState)
				{
					timer = UnityEngine.Random.Range(2f, 3f);
				}
				if (timer <= 0)
				{
					SetState(NPCState.Idle);
				}
				break;
			case NPCState.Flee:
				// 被偷
				if (firstFrameInState)
				{
					timer = UnityEngine.Random.Range(1f, 2f);
				}
				if (timer <= 0)
				{
					SetState(NPCState.Dead);
				}
				break;
			case NPCState.Interact_Share:
				// Interacting to share
				if (firstFrameInState)
				{
					NPCStateUtils.ResetShareFSM(); // Reset the share FSM
				}
				// Call the sub FSM for sharing with result handling
				NPCBehavior nearestNPC = GetNearestNPC();
				if (nearestNPC == null)
				{
					Debug.LogWarning("No other NPCs found to share with.");
					SetState(NPCState.Idle); // No other NPCs to share with, go idle
					return;
				}
				var shareResult = NPCStateUtils.Share(this, nearestNPC.transform.position, nearestNPC);
				if (shareResult == NPCStateUtils.InteractionResult.Success)
				{
					SetState(NPCState.Smile);
				}
				else if (shareResult == NPCStateUtils.InteractionResult.Fail)
				{
					SetState(NPCState.Idle);
				}
				break;
			case NPCState.Interact_Steal:
				// Interacting to steal
				if (firstFrameInState)
				{
					NPCStateUtils.ResetStealFSM(); // Reset the steal FSM
				}
				// Get the nearest NPC to steal from
				nearestNPC = GetNearestNPC();
				if (nearestNPC == null)
				{
					Debug.LogWarning("No other NPCs found to steal from.");
					SetState(NPCState.Idle); // No other NPCs to steal from, go idle
					return;
				}
				// Call the sub FSM for stealing with result handling
				var stealResult = NPCStateUtils.Steal(this, nearestNPC.transform.position, nearestNPC);
				if (stealResult == NPCStateUtils.InteractionResult.Success)
				{
					SetState(NPCState.Idle);
				}
				else if (stealResult == NPCStateUtils.InteractionResult.Fail)
				{
					SetState(NPCState.Idle);
				}
				break;
			case NPCState.Dead:
				// NPC is dead, stop all movement
				timer = 0; // Stop the timer
				break;
		}
	}

	// ############################################################
	// APIs
	// ############################################################
	public void OnStolen() // Callback when another is stealing this NPC
	{
		// reduce the light value to 0
		properties.lightValue = 0;
		SetState(NPCState.Flee); // Set the NPC state to Flee
	}

	public void OnShared() // Callback when another is sharing with this NPC
	{
		properties.lightValue = 2;
		SetState(NPCState.Smile); // Set the NPC state to Smile
	}
}
