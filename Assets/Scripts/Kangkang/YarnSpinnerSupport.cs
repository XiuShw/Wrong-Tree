using Yarn.Unity;
using UnityEngine;

public class DialogueCallbacks : MonoBehaviour
{
    [SerializeField] DialogueRunner runner;
    [SerializeField] NPCBehavior npc;

    [YarnCommand("start_story")]
    public void StartNarrative()
    {
        npc.EnableNarrative();
        npc.SetState(NPCBehavior.NPCState.Narrative);
    }

    [YarnCommand("end_story")]
    public void EndNarrative()
    {
        npc.DisableNarrative();
    }
}
