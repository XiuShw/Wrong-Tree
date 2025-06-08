// filepath: c:\Users\IWMAI\OneDrive - University of Toronto\Programs\Wrong-Tree\Assets\Scripts\Kangkang\NPCStateUtils.cs
using Unity.VisualScripting;
using UnityEngine;

public static class NPCStateUtils
{
	// 生成一个随机方向（避免极小向量，避免与上次方向过于相似或者相反）
	static private Vector2 lastDir = Vector2.zero;

	public static Vector2 GetRandomWalkDirection()
	{
		Vector2 dir;
		do
		{
			dir = Random.insideUnitCircle;
		} while (dir.sqrMagnitude < 0.01f || Vector2.Dot(dir, lastDir) > 0.2f ||
			Vector2.Dot(dir, lastDir) < -0.2f);
		lastDir = dir.normalized;
		return lastDir;
	}

	// 决定闲逛、分享或偷窃的状态
	static public int playerShareCount = 0;
	static public int playerStealCount = 0;
	public static NPCState DecideIdleShareSteal(NPCBehavior npc, NPCProperties properties)
	{
		NPCBehavior nearestNPC = npc.GetNearestNPC();
		float nearestNPCDistance = float.MaxValue; // 初始化为最大值
		if (nearestNPC != null)
		{
			Debug.Log("Nearest NPC found: " + nearestNPC.name);
			nearestNPCDistance = Vector3.Distance(npc.transform.position, nearestNPC.transform.position);
		}
		// 根据NPC的属性决定状态
		if (properties.currentAtitude == NPCAtitude.Neutral || nearestNPCDistance > properties.decideRange)
		{
			// 如果没有其他NPC在附近，或者当前态度是中立，则进入闲逛状态
			return NPCState.Idle;
		}
		bool secureResult = NPCBehavior.OnSecured(npc, nearestNPC);
		if (!secureResult)
		{
			// 说明锁定该NPC失败，那就进入闲逛状态
			return NPCState.Idle;
		}
		if (properties.currentAtitude == NPCAtitude.Share)
		{
			// 如果态度是分享，则进入分享状态
			return NPCState.Interact_Share;
		}
		if (properties.currentAtitude == NPCAtitude.Steal)
		{
			// 如果态度是偷窃，则进入偷窃状态
			return NPCState.Interact_Steal;
		}
		Debug.LogWarning("Unknown NPC attitude: " + properties.currentAtitude);
		return NPCState.Interact_Steal;
	}

	// Define result type for share/steal sub-FSM
	public enum InteractionResult { Running, Success, Fail }

	// 分享逻辑：小型FSM，先接近玩家，再放大缩小，最后结束
	public enum ShareState { Approaching, Scaling, Sharing, Done }

	public static InteractionResult Share(NPCBehavior npc)
	{
		NPCBehavior targetNPC = npc.ISecuredWhom;
		switch (npc.shareState)
		{
			case ShareState.Approaching:
				// Default interaction range
				float distance = Vector3.Distance(npc.transform.position, targetNPC.transform.position);
				if (distance > targetNPC.properties.shareVisionRange) // 超出可视距离，触发Fail
				{
					npc.shareState = ShareState.Done;
					NPCBehavior.OnReleased(npc, targetNPC);
					return InteractionResult.Fail;
				}
				if (distance > targetNPC.properties.interactionRange)
				{
					Vector3 direction = (targetNPC.transform.position - npc.transform.position).normalized;
					npc.transform.position += direction * Time.deltaTime * npc.WalkSpeed;
					return InteractionResult.Running;
				}
				else
				{
					npc.shareState = ShareState.Scaling;
					npc.shareScaleTimer = 0f;
					return InteractionResult.Running;
				}
			case ShareState.Scaling:
				// 0~0.5秒放大到1.5x，0.5~1秒缩回1x，不再旋转
				float scaleDuration = 0.5f;
				float totalDuration = 1f;
				npc.shareScaleTimer += Time.deltaTime;
				float t = npc.shareScaleTimer;
				if (t <= scaleDuration)
				{
					float scale = Mathf.Lerp(1f, 1.5f, t / scaleDuration);
					npc.transform.localScale = new Vector3(scale, scale, 1f);
				}
				else if (t <= totalDuration)
				{
					float scale = Mathf.Lerp(1.5f, 1f, (t - scaleDuration) / scaleDuration);
					npc.transform.localScale = new Vector3(scale, scale, 1f);
				}
				if (npc.shareScaleTimer >= totalDuration)
				{
					npc.transform.localScale = Vector3.one; // 确保回到1x
					npc.shareState = ShareState.Sharing;
				}
				return InteractionResult.Running;
			case ShareState.Sharing:
				// 共享光逻辑
				targetNPC.OnShared(); // 调用目标NPC的共享方法
				npc.shareState = ShareState.Done;
				return InteractionResult.Running;
			case ShareState.Done:
			default:
				NPCBehavior.OnReleased(npc, targetNPC);
				return InteractionResult.Success;
		}
	}

	// 偷窃逻辑：小型FSM，先跑步接近玩家，再缩小再恢复，最后结束
	public enum StealState { Approaching, Scaling, Stealing, Done }

	public static InteractionResult Steal(NPCBehavior npc)
	{
		NPCBehavior targetNPC = npc.ISecuredWhom;
		switch (npc.stealState)
		{
			case StealState.Approaching:
				// Default interaction range
				float distance = Vector3.Distance(npc.transform.position, targetNPC.transform.position);
				if (distance > targetNPC.properties.stealAggroRange) // 超出仇恨距离，触发Fail
				{
					npc.stealState = StealState.Done;
					NPCBehavior.OnReleased(npc, targetNPC);
					return InteractionResult.Fail;
				}
				if (distance > targetNPC.properties.interactionRange)
				{
					Vector3 direction = (targetNPC.transform.position - npc.transform.position).normalized;
					npc.transform.position += direction * Time.deltaTime * npc.RunSpeed; // 跑步速度
					return InteractionResult.Running;
				}
				else
				{
					npc.stealState = StealState.Scaling;
					npc.stealScaleTimer = 0f;
					return InteractionResult.Running;
				}
			case StealState.Scaling:
				// 0~0.5秒缩小到0.5x，0.5~1秒恢复1x
				float scaleDuration2 = 0.5f;
				float totalDuration2 = 1f;
				npc.stealScaleTimer += Time.deltaTime;
				float t2 = npc.stealScaleTimer;
				if (t2 <= scaleDuration2)
				{
					float scale = Mathf.Lerp(1f, 0.5f, t2 / scaleDuration2);
					npc.transform.localScale = new Vector3(scale, scale, 1f);
				}
				else if (t2 <= totalDuration2)
				{
					float scale = Mathf.Lerp(0.5f, 1f, (t2 - scaleDuration2) / scaleDuration2);
					npc.transform.localScale = new Vector3(scale, scale, 1f);
				}
				if (npc.stealScaleTimer >= totalDuration2)
				{
					npc.transform.localScale = Vector3.one; // 确保回到1x
					npc.stealState = StealState.Stealing;
				}
				return InteractionResult.Running;
			case StealState.Stealing:
				// 偷窃逻辑
				targetNPC.OnStolen(); // 调用目标NPC的偷窃方法
				npc.properties.lightValue = 2;
				npc.stealState = StealState.Done;
				return InteractionResult.Running;
			case StealState.Done:
			default:
				NPCBehavior.OnReleased(npc, targetNPC);
				return InteractionResult.Success;
		}
	}

	public static void ResetShareFSM(NPCBehavior npc)
	{
		npc.shareState = ShareState.Approaching;
		npc.shareScaleTimer = 0f;
	}

	public static void ResetStealFSM(NPCBehavior npc)
	{
		npc.stealState = StealState.Approaching;
		npc.stealScaleTimer = 0f;
	}
}
