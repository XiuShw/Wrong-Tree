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
		// 根据NPC的属性决定状态
		if (properties.currentAtitude == NPCAtitude.Neutral || npc.GetNearestNPCDistance() > 3f)
		{
			// 如果没有其他NPC在附近，或者当前态度是中立，则闲逛
			return NPCState.Idle; // 这里可以根据需要调整为其他状态
		}
		else if (properties.currentAtitude == NPCAtitude.Share && properties.lightValue > 1f)
		{
			// 如果态度是分享且光值大于1，则进入分享状态
			return NPCState.Interact_Share;
		}
		else if (properties.currentAtitude == NPCAtitude.Steal)
		{
			// 如果态度是偷窃，则进入偷窃状态
			return NPCState.Interact_Steal;
		}
		else
		{
			Debug.LogWarning("Unknown NPC attitude: " + properties.currentAtitude);
			return NPCState.Idle; // 默认回到闲逛状态
		}
	}

	// Define result type for share/steal sub-FSM
	public enum InteractionResult { Running, Success, Fail }

	// 分享逻辑：小型FSM，先接近玩家，再放大缩小，最后结束
	public enum ShareState { Approaching, Scaling, Done }
	private static ShareState shareState = ShareState.Approaching;
	private static float shareScaleTimer = 0f;
	private static float shareVisionRange = 3f; // 可视距离超参数

	public static InteractionResult Share(NPCBehavior npc, Vector3 targetPosition)
	{
		switch (shareState)
		{
			case ShareState.Approaching:
				// Default interaction range
				float interactionRange = 1f;
				float distance = Vector3.Distance(npc.transform.position, targetPosition);
				if (distance > shareVisionRange) // 超出可视距离，触发Fail
				{
					shareState = ShareState.Done;
					return InteractionResult.Fail;
				}
				if (distance > interactionRange)
				{
					Vector3 direction = (targetPosition - npc.transform.position).normalized;
					npc.transform.position += direction * Time.deltaTime * npc.WalkSpeed;
					return InteractionResult.Running;
				}
				else
				{
					shareState = ShareState.Scaling;
					shareScaleTimer = 0f;
					return InteractionResult.Running;
				}
			case ShareState.Scaling:
				// 0~0.5秒放大到1.5x，0.5~1秒缩回1x，不再旋转
				float scaleDuration = 0.5f;
				float totalDuration = 1f;
				shareScaleTimer += Time.deltaTime;
				float t = shareScaleTimer;
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
				if (shareScaleTimer >= totalDuration)
				{
					npc.transform.localScale = Vector3.one; // 确保回到1x
					shareState = ShareState.Done;
					return InteractionResult.Success;
				}
				return InteractionResult.Running;
			case ShareState.Done:
			default:
				return InteractionResult.Success;
		}
	}

	// 偷窃逻辑：小型FSM，先跑步接近玩家，再缩小再恢复，最后结束
	public enum StealState { Approaching, Scaling, Done }
	private static StealState stealState = StealState.Approaching;
	private static float stealScaleTimer = 0f;
	private static float stealAggroRange = 4f; // 仇恨距离超参数

	public static InteractionResult Steal(NPCBehavior npc, Vector3 targetPosition)
	{
		switch (stealState)
		{
			case StealState.Approaching:
				// Default interaction range
				float interactionRange = 1f;
				float distance = Vector3.Distance(npc.transform.position, targetPosition);
				if (distance > stealAggroRange) // 超出仇恨距离，触发Fail
				{
					stealState = StealState.Done;
					return InteractionResult.Fail;
				}
				if (distance > interactionRange)
				{
					Vector3 direction = (targetPosition - npc.transform.position).normalized;
					npc.transform.position += direction * Time.deltaTime * npc.RunSpeed; // 跑步速度
					return InteractionResult.Running;
				}
				else
				{
					stealState = StealState.Scaling;
					stealScaleTimer = 0f;
					return InteractionResult.Running;
				}
			case StealState.Scaling:
				// 0~0.5秒缩小到0.5x，0.5~1秒恢复1x
				float scaleDuration2 = 0.5f;
				float totalDuration2 = 1f;
				stealScaleTimer += Time.deltaTime;
				float t2 = stealScaleTimer;
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
				if (stealScaleTimer >= totalDuration2)
				{
					npc.transform.localScale = Vector3.one; // 确保回到1x
					stealState = StealState.Done;
					return InteractionResult.Success;
				}
				return InteractionResult.Running;
			case StealState.Done:
			default:
				return InteractionResult.Success;
		}
	}

	public static void ResetShareFSM()
	{
		shareState = ShareState.Approaching;
		shareScaleTimer = 0f;
	}

	public static void ResetStealFSM()
	{
		stealState = StealState.Approaching;
		stealScaleTimer = 0f;
	}
}
