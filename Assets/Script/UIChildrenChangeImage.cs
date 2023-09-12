using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIChildrenChangeImage : MonoBehaviour
{
    // �q�K�������������ɂȂ�����摜��ς��邩
    [SerializeField] private int childLowLimit;
    // �ς���摜
    [SerializeField] private Sprite aliveImage;
    [SerializeField] private Sprite deathImage;
    private Image image;
    // �ς��邩�t���O
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
