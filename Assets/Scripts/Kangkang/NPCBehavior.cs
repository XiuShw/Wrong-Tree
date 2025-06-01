// filepath: c:\Users\IWMAI\OneDrive - University of Toronto\Programs\Wrong-Tree\Assets\Scripts\Kangkang\NPCBehavior.cs
using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NPCBehavior : MonoBehaviour
{
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
	}
	
	private NPCProperties properties; // Reference to the NPCProperties component

	private void SetState(NPCState newState)
	{
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
	
	public NPCState GetState()
	{
		return currentState;
	}

	// Some variables / properties
	[SerializeField] private NPCState currentState;
	public NPCState CurrentState => currentState; // Expose the current state
	private bool justChangedState = true; // Flag to check if the state just changed
	private Animator animator;
	// Use properties from NPCProperties instead of local variables
	public float WalkSpeed => properties != null ? properties.walkSpeed : 2f;
	public float RunSpeed => properties != null ? properties.runSpeed : 5f;
	[SerializeField] private float timer; // for timing different states
	[SerializeField] private Vector2 walkDirection = new Vector2(1, 0); // Current walking direction
	// 关于血条和光条（光度值）
	// [SerializeField] private float health = 100f; // 移除本地 health
	public float Health => properties != null ? properties.health : 100f;
	// [SerializeField] private float lightValue = 1f; // 移除本地 lightValue
	public float LightValue => properties != null ? properties.lightValue : 1f;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
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
					walkDirection = NPCStateUtils.GetRandomWalkDirection(); // Get a random walk direction
				}
				transform.position += Time.deltaTime * WalkSpeed * (Vector3)walkDirection;
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
				// Call the sub FSM for sharing
				bool isDone = NPCStateUtils.Share(this, PlayerMovement.Instance);
				if (isDone)
				{
					SetState(NPCState.Smile);
				}
				break;
			case NPCState.Interact_Steal:
				// Interacting to steal
				if (firstFrameInState)
				{
					NPCStateUtils.ResetStealFSM(); // Reset the steal FSM
				}
				// Call the sub FSM for stealing
				bool stealDone = NPCStateUtils.Steal(this, PlayerMovement.Instance);
				if (stealDone)
				{
					SetState(NPCState.Idle);
				}
				break;
			case NPCState.Dead:
				// NPC is dead, stop all movement
				timer = 0; // Stop the timer
				break;
		}
		// 移除 npcStateText 相关逻辑
	}
}
