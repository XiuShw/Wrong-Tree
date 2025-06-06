using UnityEngine;
using DG.Tweening;

public class SpeechBubbleFloat : MonoBehaviour
{
    public RectTransform bubbleTransform;
    public float floatDistance = 5f;
    public float duration = 1.5f;

    private Vector2 originalPosition;

    void Start()
    {
        if (bubbleTransform == null)
            bubbleTransform = GetComponent<RectTransform>();

        originalPosition = bubbleTransform.anchoredPosition;

        StartFloating();
    }

    void StartFloating()
    {
        // A gentle random-ish looping float
        bubbleTransform.DOAnchorPos(originalPosition + new Vector2(floatDistance, floatDistance), duration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }
}
