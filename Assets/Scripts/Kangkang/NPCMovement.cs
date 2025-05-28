using System;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovement : MonoBehaviour
{
	// The list of possible states for the NPC
	private enum NPCState
	{
		Idle = 0,
		Wander = 1,
		Smile = 2, // Aka. being shared
		Flee = 3, // Aka. being stolen
		Interact_Share = 4,
		Interact_Steal = 5,
		Dead = 6
	}
	private void SetState(NPCState newState)
	{
		currentState = newState;
		justChangedState = true; // Set the flag to true when changing state
		animator.SetInteger("State", (int)newState);
	}

	// Some variables / properties
	[SerializeField] private NPCState currentState;
	private bool justChangedState = true; // Flag to check if the state just changed
	private Animator animator;
	private float walkSpeed = 2f; // Speed for walking
	private float runSpeed = 5f; // Speed for running
	[SerializeField] private float timer; // for timing different states
	[SerializeField] private Vector2 walkDirection = new Vector2(1, 0); // Current walking direction

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		animator = GetComponent<Animator>();
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
					Vector2 dir;
					do {
						dir = UnityEngine.Random.insideUnitCircle;   // 位置分布更均匀
					} while (dir.sqrMagnitude < 0.01f);  // 避免极小向量
					walkDirection = dir.normalized;
				}
				transform.position += (Vector3)walkDirection * walkSpeed * Time.deltaTime;
				if (timer <= 0)
				{
					SetState(NPCState.Idle);
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
					timer = UnityEngine.Random.Range(1f, 2f);
				}
				if (timer <= 0)
				{
					SetState(NPCState.Smile);
				}
				break;
			case NPCState.Interact_Steal:
				// Interacting to steal
				if (firstFrameInState)
				{
					timer = UnityEngine.Random.Range(1f, 2f);
				}
				if (timer <= 0)
				{
					SetState(NPCState.Flee);
				}
				break;
			case NPCState.Dead:
				// NPC is dead, stop all movement
				timer = 0; // Stop the timer
				break;
		}
	}
}
