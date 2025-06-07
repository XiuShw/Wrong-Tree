using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public List<NPCBehavior> allNPCs = new List<NPCBehavior>();
    public bool simulationEnabled = false;
    private bool _lastSimulationEnabled = true;

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
        _lastSimulationEnabled = simulationEnabled;
    }
}