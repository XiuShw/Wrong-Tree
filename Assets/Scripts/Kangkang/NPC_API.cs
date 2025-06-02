/*
Chinese:
这个文件描述了暴露在外的NPC相关的API，同时兼容yarn spinner
包括：
1. 设置NPC状态 `SetNPCState(NPCBehavior.NPCState newState)`
2. 获取NPC状态 `GetNPCState()`
3. 启用叙事模式 `EnableNarrative()`
4. 禁用叙事模式 `DisableNarrative(bool restorePrev = true)`

English:
This file describes the exposed API for NPC-related functionalities.
It includes:
1. Set NPC state: `SetNPCState(NPCBehavior.NPCState newState)`
2. Get NPC state: `GetNPCState()`
3. Enable narrative mode: `EnableNarrative()`
4. Disable narrative mode: `DisableNarrative(bool restorePrev = true)`
*/

using UnityEngine;
using Yarn.Unity;

public class NPC_API : MonoBehaviour
{
	[SerializeField] private NPCBehavior npc;
	[SerializeField] private DialogueRunner runner;
	static NPCBehavior.NPCState prevStateBeforeNarrative;

	[YarnCommand("set_npc_state")]
	public void SetNPCState(NPCBehavior.NPCState newState)
	{
		npc.SetState(newState);
	}

	public NPCBehavior.NPCState GetNPCState()
	{
		return npc.currentState;
	}

	[YarnCommand("start_narrative")]
	public void EnableNarrative()
	{
		npc.narrativeEnabled = true;
		prevStateBeforeNarrative = npc.currentState;
		npc.SetState(NPCBehavior.NPCState.Narrative);
	}

	[YarnCommand("end_narrative")]
	public void DisableNarrative(bool restorePrev = true)
	{
		npc.narrativeEnabled = false;
		if (restorePrev && npc.currentState == NPCBehavior.NPCState.Narrative)
		{
			npc.SetState(prevStateBeforeNarrative);
		}
	}
}