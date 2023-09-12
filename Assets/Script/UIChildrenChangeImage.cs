using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIChildrenChangeImage : MonoBehaviour
{
    // ŽqƒKƒ‚‚ª‚¢‚­‚Â–¢–ž‚É‚È‚Á‚½‚ç‰æ‘œ‚ð•Ï‚¦‚é‚©
    [SerializeField] private int childLowLimit;
    // •Ï‚¦‚é‰æ‘œ
    [SerializeField] private Sprite aliveImage;
    [SerializeField] private Sprite deathImage;
    private Image image;
    // •Ï‚¦‚é‚©ƒtƒ‰ƒO
    private bool isChange;

    XParticleManager xParticle;

    void Start()
    {
        image = GetComponent<Image>();
        isChange = false;
        xParticle = GetComponent<XParticleManager>();
    }

    void Update()
    {
        if (!isChange && ResultManager.childCount < childLowLimit)
        {
            isChange = true;
            xParticle.Set();
        }
        if (image != null)
        {
            if (isChange)
            {
                image.sprite = deathImage;
            }
            else
            {
                image.sprite = aliveImage;
            }
        }
    }
}
