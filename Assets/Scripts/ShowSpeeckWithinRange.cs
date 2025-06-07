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
        if (Vector3.Distance(transform.position, playerTransform.position) < 12f && Vector3.Distance(transform.position, playerTransform.position) > 5f)
        {
            // ���NPC����ҵľ���С��12����5����speechBubble
            gameObject.transform.Find("NPC_A_Speech/Canvas/speechBubble").gameObject.SetActive(true);
        }
        else if (Vector3.Distance(transform.position, playerTransform.position) > 12f)
        {
            // ���NPC����ҵľ������12���ر�speechBubble
            gameObject.transform.Find("NPC_A_Speech/Canvas/speechBubble").gameObject.SetActive(false);
        }
    }
}
