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
	Paused = 7 // Narrative state, for storytelling purposes
}

public class NPCBehavior : MonoBehaviour
{

	private NPCProperties properties; // Reference to the NPCProperties component

	// Add narrative control fields
	[SerializeField] public bool narrativeEnabled = false;

	public void SetState(NPCState newState)
	{
		// Prevent entering narrative if disabled
		if (newState == NPCState.Paused && !narrativeEnabled)
			return;

		currentState = newState;
		justChangedState = true; // Set the flag to true when changing state
								 // npcStateText.text = $"{newState}"; // 移除文本更新逻辑
		animator.SetInteger("State", (int)newState);
		// Update NPCProperties current state
		if (properties != null)
		{
			properties.currentState = newState;
		}
		// Debug.Log($"NPC state changed to: {newState}"); // Log the state change
	}

	// Some variables / properties
	[SerializeField] public NPCState currentState { get; private set; } = NPCState.Idle; // Default state is Idle
	private bool justChangedState = true; // Flag to check if the state just changed
	private Animator animator;
	// Use properties from NPCProperties instead of local variables
	public float WalkSpeed => properties != null ? properties.walkSpeed : 2f;
	public float RunSpeed => properties != null ? properties.runSpeed : 5f;
	[SerializeField] private float timer; // for timing different states
	[SerializeField] private Vector2 walkDirection = new Vector2(1, 0); // Current walking direction
	[SerializeField] private float maxWanderDistance = 5f; // Maximum distance from anchor for wandering
	[SerializeField][Range(0f, 1f)] private float wanderBias = 0.5f; // Bias towards anchor when wandering
	public Vector2 AnchorPosition => properties != null ? properties.anchorPosition : new Vector2(transform.position.x, transform.position.y);

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
		timer -= Time.deltaTime;
		bool firstFrameInState = justChangedState;
		justChangedState = false; // Reset the flag for the next frame
		switch (currentState)
		{
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
					NPCState _nextState = NPCStateUtils.DecideIdleShareSteal();
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
					walkDirection = -walkDirection; // Reverse direction to flee
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
				var shareResult = NPCStateUtils.Share(this, PlayerMovement.Instance);
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
				// Call the sub FSM for stealing with result handling
				var stealResult = NPCStateUtils.Steal(this, PlayerMovement.Instance);
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
}
