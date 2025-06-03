using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;

public class FlashingAnim : MonoBehaviour
{
    public bool isText;
    public bool startImmediately;

    public float fadeAmount = 0.5f;
    public float flashDuration = 1.0f;
    bool isActive;

    private Tween tween;
    TweenParams tweenp;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isActive = startImmediately;
        if (isActive)
        {
            startTween();
        }
    }

    void startTween()
    {
        tweenp = TweenParams.Params.SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
        if (isText)
        {
            tween = DOTween.To(() => GetAlphaText(), x => SetAlphaText(x), fadeAmount, flashDuration).SetAs(tweenp);
        }
        else
        {
            tween = DOTween.To(() => GetAlpha(), x => SetAlpha(x), fadeAmount, flashDuration).SetAs(tweenp);
        }
    }

    public void SetAnimated(bool anim)
    {
        isActive = anim;
        if (isActive)
        {
            startTween();
        }
        else
        {
            if (tween != null)
            {
                tween.Kill();
            }
            if (isText)
            {
                SetAlphaText(1.0f);
            }
            else
            {
                SetAlpha(1.0f);
            }
        }
    }

    float GetAlpha()
    {
        return GetComponent<SpriteRenderer>().color.a;
    }

    void SetAlpha(float a)
    {
        Color color = GetComponent<SpriteRenderer>().color;
        color.a = a;
        GetComponent<SpriteRenderer>().color = color;
    }

    float GetAlphaText()
    {
        return GetComponent<TextMeshProUGUI>().color.a;
    }

    void SetAlphaText(float a)
    {
        Color color = GetComponent<TextMeshProUGUI>().color;
        color.a = a;
        GetComponent<TextMeshProUGUI>().color = color;
    }
}
