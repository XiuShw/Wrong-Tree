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
            // 如果NPC和玩家的距离小于12大于5，打开speechBubble
            gameObject.transform.Find("NPC_A_Speech/Canvas/speechBubble").gameObject.SetActive(true);
        }
        else if (Vector3.Distance(transform.position, playerTransform.position) > 12f)
        {
            // 如果NPC和玩家的距离大于12，关闭speechBubble
            gameObject.transform.Find("NPC_A_Speech/Canvas/speechBubble").gameObject.SetActive(false);
        }
    }
}
