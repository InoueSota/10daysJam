using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZanzoScript : MonoBehaviour
{

    private float destroyTime = 1.0f;
    private float eTime = 0.0f;
    private SpriteRenderer spriteRenderer;

   // Start is called before the first frame update
   void Start()
    {
        //spriteRenderer = this.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

        float Alpha = 1.0f - (eTime / destroyTime);

        spriteRenderer.color = new Color(1.0f,1.0f,1.0f,Alpha);


        if(eTime >= destroyTime)
        {
            Destroy(this.gameObject);
        }

        eTime += Time.deltaTime;

    }

    public void SetSprite(Sprite s,float destroytime,bool isFlipX)
    {
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = s;
        spriteRenderer.flipX = isFlipX;
        destroyTime = destroytime;
    }
}
