using UnityEngine;
using DG.Tweening;

public class ExclamationMarkUI : MonoBehaviour
{
    public float growDuration = 0.4f;
    public float shakeDuration = 0.3f;
    public float shakeStrength = 10f;
    public float scaleFactor = 0.1f; // How much bigger/smaller it pulses
    public float duration = 0.5f;    // Time for one pulse (up + down)

    private RectTransform rect;
    private Vector3 originalScale;

    void Start()
    {
        rect = GetComponent<RectTransform>();
        originalScale = rect.localScale;

        rect.localScale = new Vector3(originalScale.x, 0f, originalScale.z);

        rect.DOScaleY(originalScale.y, growDuration)
            .SetEase(Ease.OutBack)
            .OnComplete(() =>
            {
                Debug.Log("UI scale complete ¡ª starting shake.");
                rect.DOShakeAnchorPos(
                    duration: shakeDuration,
                    strength: new Vector2(0, shakeStrength),
                    vibrato: 10,
                    randomness: 90,
                    fadeOut: false
                ).SetLoops(-1);
            });
    }
}
