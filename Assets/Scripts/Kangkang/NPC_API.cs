/*
Chinese:
这个文件描述了暴露在外的NPC相关API，并兼容Yarn Spinner。
包括：
1. `SetNPCState(NPCState newState)`              设置NPC状态
2. `GetNPCState()`                               获取NPC状态
3. `EnableNarrative()`                           启用叙事模式
4. `DisableNarrative(bool restorePrev = true)`   禁用叙事模式
5. `StartCasualConversation()`                   开始日常对话
6. `EndCasualConversation()`                     结束日常对话
7. `TakeDamage(float amount)`                    受到伤害
8. `Heal(float amount)`                          治疗NPC
9. `StealLight(float amount)`                    偷取光
10. `GiveLight(float amount)`                    给予光

English:
This file describes the exposed API for NPC-related functionalities, compatible with Yarn Spinner.
It includes:
1. `SetNPCState(NPCState newState)`:             Set NPC state
2. `GetNPCState()`:                              Get NPC state
3. `EnableNarrative()`:                          Enable narrative mode
4. `DisableNarrative(bool restorePrev = true)`:  Disable narrative mode
5. `StartCasualConversation()`:                  Start casual conversation
6. `EndCasualConversation()`:                    End casual conversation
7. `TakeDamage(float amount)`:                   Take damage
8. `Heal(float amount)`:                         Heal NPC
9. `StealLight(float amount)`:                   Steal light
10. `GiveLight(float amount)`:                   Give light
*/

using UnityEngine;
using Yarn.Unity;

public class NPC_API : MonoBehaviour
{
	[SerializeField] private NPCBehavior npc;
	[SerializeField] private DialogueRunner runner;
	static NPCState prevStateBeforePause;

	[YarnCommand("set_npc_state")]
	public void SetNPCState(NPCState newState)
	{
		npc.SetState(newState);
	}

	public NPCState GetNPCState()
	{
		return npc.currentState;
	}

	[YarnCommand("start_narrative")]
	public void EnableNarrative()
	{
		npc.narrativeEnabled = true;
		prevStateBeforePause = npc.currentState;
		npc.SetState(NPCState.Paused);
	}

	[YarnCommand("end_narrative")]
	public void DisableNarrative(bool restorePrev = true)
	{
		npc.narrativeEnabled = false;
		if (restorePrev && npc.currentState == NPCState.Paused)
		{
			npc.SetState(prevStateBeforePause);
		}
	}

	[YarnCommand("start_casual_conversation")]
	public void StartCasualConversation()
	{
		// CH: 开始NPC的日常对话
		// EN: Start NPC's casual conversation
		throw new System.NotImplementedException();
	}

	[YarnCommand("end_casual_conversation")]
	public void EndCasualConversation()
	{
		// CH: 结束NPC的日常对话
		// EN: End NPC's casual conversation
		throw new System.NotImplementedException();
	}

	[YarnCommand("take_damage")]
	public void TakeDamage(float amount)
	{
		// CH: 玩家攻击NPC
		// EN: Player attacks NPC
		throw new System.NotImplementedException();
	}

	[YarnCommand("heal")]
	public void Heal(float amount)
	{
		// CH: 玩家治疗NPC
		// EN: Player heals NPC
		throw new System.NotImplementedException();
	}

	[YarnCommand("steal_light")]
	public void StealLight(float amount)
	{
		// CH: 玩家偷取NPC的光
		// EN: Player steals light from NPC
		throw new System.NotImplementedException();
	}

	[YarnCommand("give_light")]
	public void GiveLight(float amount)
	{
		// CH: 玩家给予NPC光
		// EN: Player gives light to NPC
		throw new System.NotImplementedException();
	}
}