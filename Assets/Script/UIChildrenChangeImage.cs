using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIChildrenChangeImage : MonoBehaviour
{
    // 子ガモがいくつ未満になったら画像を変えるか
    [SerializeField] private int childLowLimit;
    // 変える画像
    [SerializeField] private Sprite aliveImage;
    [SerializeField] private Sprite deathImage;
    private Image image;
    // 変えるかフラグ
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
