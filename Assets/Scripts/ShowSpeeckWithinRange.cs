using UnityEngine;

public class ShowSpeeckWithinRange : MonoBehaviour
{
    [SerializeField] Transform playerTransform;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(Vector3.Distance(transform.position, playerTransform.position));
        if (Vector3.Distance(transform.position, playerTransform.position) < 20f && Vector3.Distance(transform.position, playerTransform.position) > 5f)
        {
            // ���NPC����ҵľ���С��20����5����speechBubble
            gameObject.transform.Find("NPC_A_Speech/Canvas/speechBubble").gameObject.SetActive(true);
        }
        else if (Vector3.Distance(transform.position, playerTransform.position) > 20f)
        {
            // ���NPC����ҵľ������20���ر�speechBubble
            gameObject.transform.Find("NPC_A_Speech/Canvas/speechBubble").gameObject.SetActive(false);
        }
    }
}
