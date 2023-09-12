using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class AlphaText : MonoBehaviour
{
    private TextMeshProUGUI text;

    // テキストの色
    private float red, green, blue, alpha;

    // α値をイージングで変化させる
    private float alphaTime;
    private float alphaLeftTime;

    // イージングできたかフラグ
    private bool isFadeInClear;
    private bool isFadeOutStart;

    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();

        red = text.color.r;
        green = text.color.g;
        blue = text.color.b;
        alpha = 0f;
        text.color = new(red, green, blue, alpha);

        alphaTime = 0.3f;
        alphaLeftTime = alphaTime;

        isFadeInClear = false;
        isFadeOutStart = false;
    }

    void Update()
    {
        if (!isFadeInClear)
        {
            alphaLeftTime -= Time.deltaTime;
            if (alphaLeftTime < 0f) { alphaLeftTime = 0f; }

            float t = alphaLeftTime / alphaTime;
            alpha = Mathf.Lerp(1f, 0f, t * t);
            text.color = new(red, green, blue, alpha);
            if (alphaLeftTime <= 0f) { isFadeInClear = true; }
        }
        else if (isFadeOutStart)
        {
            alphaLeftTime -= Time.deltaTime;
            if (alphaLeftTime < 0f) { alphaLeftTime = 0f; }

            float t = alphaLeftTime / alphaTime;
            alpha = Mathf.Lerp(0f, 1f, t * t);
            text.color = new(red, green, blue, alpha);
            if (alphaLeftTime <= 0f) { gameObject.SetActive(false); }
        }
    }

    public bool GetIsFadeInClear()
    {
        return isFadeInClear;
    }

    public void FadeOutInitialize()
    {
        if (!isFadeOutStart)
        {
            alphaLeftTime = alphaTime;
            isFadeOutStart = true;
        }
    }
}
