using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlphaImage : MonoBehaviour
{
    private Image image;

    // 画像の色
    private float red, green, blue, alpha;

    // α値をイージングで変化させる
    private float alphaTime;
    private float alphaLeftTime;

    private bool isFadeInStart;
    private bool isFadeOutStart;

    void Start()
    {
        Initialize();
    }

    void Update()
    {
        if (isFadeInStart)
        {
            alphaLeftTime -= Time.deltaTime;
            if (alphaLeftTime < 0f) { alphaLeftTime = 0f; }

            float t = alphaLeftTime / alphaTime;
            alpha = Mathf.Lerp(1f, 0f, t * t);
            image.color = new(red, green, blue, alpha);
            if (alphaLeftTime <= 0f) { isFadeInStart = false; }
        }

        if (isFadeOutStart)
        {
            alphaLeftTime -= Time.deltaTime;
            if (alphaLeftTime < 0f) { alphaLeftTime = 0f; }

            float t = alphaLeftTime / alphaTime;
            alpha = Mathf.Lerp(0f, 1f, t * t);
            image.color = new(red, green, blue, alpha);
            if (alphaLeftTime <= 0f) { gameObject.SetActive(false); }
        }
    }

    public void Initialize()
    {
        image = GetComponent<Image>();

        red = image.color.r;
        green = image.color.g;
        blue = image.color.b;
        alpha = 0f;
        image.color = new(red, green, blue, alpha);

        alphaTime = 0.3f;
        alphaLeftTime = alphaTime;

        isFadeInStart = true;
        isFadeOutStart = false;
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
