using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance { get; private set; }

	public List<NPCBehavior> allNPCs = new List<NPCBehavior>();
	public bool simulationEnabled = false;
	private bool _lastSimulationEnabled = true;
	private bool _lastMinigameRunning = false;

	void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}
	}

	void Update()
	{
		// Check for simulation toggle input
		if (!simulationEnabled && _lastSimulationEnabled)
		{
			foreach (var npc in allNPCs)
			{
				npc.properties.DisableNPC();
			}
		}
		else if (simulationEnabled && !_lastSimulationEnabled)
		{
			foreach (var npc in allNPCs)
			{
				npc.properties.EnableNPC();
			}
		}
		// Monitor the minigame status
		else if (LevelManager.minigameStart && !_lastMinigameRunning && simulationEnabled)
		{
			// If the minigame just started running, disable NPC simulation
			foreach (var npc in allNPCs)
			{
				npc.properties.DisableNPC();
			}
		}
		else if (!LevelManager.minigameStart && _lastMinigameRunning && simulationEnabled)
		{
			// If the minigame just ended, enable NPC simulation
			foreach (var npc in allNPCs)
			{
				npc.properties.EnableNPC();
			}
			OnFinishMinigame();
		}
		_lastSimulationEnabled = simulationEnabled;
		_lastMinigameRunning = LevelManager.minigameStart;
		// Update all NPCs
		foreach (var npc in allNPCs)
		{
			npc.CustomUpdate();
		}
		if (SceneManager.GetActiveScene().buildIndex != 2)
		{
			Destroy(gameObject);
		}
    }

	public void OnFinishMinigame() // Callback when the minigame finishes
	{
		Debug.Log("Minigame finished! Adjusting NPC light values...");
		// 如果是1，玩家给npc光
		// 如果是-1，拿npc的光
		int gameResult = LevelManager.previousMinigameSucceed;
		// 找到玩家
		NPCBehavior playerNPC = allNPCs.Find(npc => npc.IAmPlayer);
		if (playerNPC == null)
		{
			Debug.LogError("Player NPC not found!");
			return;
		}
		// 根据游戏结果调整玩家的光
		if (gameResult == 1)
		{
			playerNPC.properties.SetLightValue(0);
			if (playerNPC.whoSecuredMe != null)
			{
				playerNPC.whoSecuredMe.properties.SetLightValue(2);
			}
			else
			{
				Debug.LogWarning("Player NPC has no whoSecuredMe reference!");
			}
		}
		else if (gameResult == -1)
		{
			playerNPC.properties.SetLightValue(2);
			if (playerNPC.whoSecuredMe != null)
			{
				playerNPC.whoSecuredMe.OnStolen(); // Call OnStolen to handle the light value and state change
			}
			else
			{
				Debug.LogWarning("Player NPC has no whoSecuredMe reference!");
			}
		}
		else
		{
			Debug.LogWarning("Unknown game result: " + gameResult);
		}
	}
}