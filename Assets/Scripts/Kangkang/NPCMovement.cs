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
		Flee = 2, // Aka. being stolen
		Smile = 3, // Aka. being shared
		Interact_Share = 4,
		Interact_Steal = 5,
		Dead = 6
	}
	private Dictionary<NPCState, float> stateDurations = new Dictionary<NPCState, float>
	{
		{ NPCState.Idle, 5f },
		{ NPCState.Wander, 3f },
		{ NPCState.Flee, 3f },
		{ NPCState.Dead, 3f }
	};
	private void SetState(NPCState newState)
	{
		currentState = newState;
		animator.SetInteger("State", (int)newState);
		float duration;
		if (stateDurations.TryGetValue(newState, out duration))
		{
			timer = duration;
		}
		else
		{
			timer = 3f; // Default duration if not defined
		}
	}

	// Some variables / properties
	[SerializeField] private NPCState currentState;
	private Animator animator;
	private float walkSpeed = 2f; // Speed for walking
	private float runSpeed = 5f; // Speed for running
	[SerializeField] private float timer; // for timing different states
	[SerializeField] private Vector2 walkDirection = new Vector2(1, 0); // Default walking direction

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		animator = GetComponent<Animator>();
		timer = 3f;
		currentState = NPCState.Idle;
	}

	// Update is called once per frame
	void Update()
	{
		timer -= Time.deltaTime;
		switch (currentState)
		{
			case NPCState.Idle:
				if (timer <= 0)
				{
					SetState(NPCState.Wander);
					walkDirection = new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)).normalized;
				}
				break;
			case NPCState.Wander:
				if (timer <= 0)
				{
					SetState(NPCState.Flee);
				}
				break;
			case NPCState.Flee:
				if (timer <= 0)
				{
					SetState(NPCState.Dead);
				}
				break;
			case NPCState.Dead:
				break;
		}
	}
}
