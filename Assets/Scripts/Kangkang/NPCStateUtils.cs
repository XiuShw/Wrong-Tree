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

	public static NPCMBehavior.NPCState DecideIdleShareSteal()
	{
		// 随机决定是闲逛、分享还是偷窃
		int randomValue = Random.Range(0, 100);
		if (randomValue < 50)
		{
			return NPCMBehavior.NPCState.Idle; // 闲逛
		}
		else if (randomValue < 80)
		{
			return NPCMBehavior.NPCState.Interact_Share; // 分享
		}
		else
		{
			return NPCMBehavior.NPCState.Interact_Steal; // 偷窃
		}
	}
}
