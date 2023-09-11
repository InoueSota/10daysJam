using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZanzoScript : MonoBehaviour
{

    [SerializeField] private float destroyTime = 1.0f;
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

        if(eTime >= destroyTime)
        {
            Destroy(this.gameObject);
        }

        eTime += Time.deltaTime;

    }

    public void SetSprite(Sprite s)
    {
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = s;
    }
}
