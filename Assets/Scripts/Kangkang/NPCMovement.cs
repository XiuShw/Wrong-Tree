using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    Transform npcTransform;
    [SerializeField] bool lightStealed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        npcTransform = GetComponentInParent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
    }
}
