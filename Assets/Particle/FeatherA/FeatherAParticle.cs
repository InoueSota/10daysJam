using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeatherAParticle : MonoBehaviour
{

    //private float radian = 0.0f;
    private bool isRun = false;

    private Vector3 startPos = Vector3.zero;
    private Vector3 lastPos = Vector3.zero;

    private float RunTimeMax = 0.5f;
    private float RunTime = 0.0f;

    private bool isFlipX = false;
    private SpriteRenderer spriteRenderer;

    private bool isDown= false;
    private float downT = 0.9f;

    // Start is called before the first frame update

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isRun)
        {



            float ta = RunTime / RunTimeMax;

            float t = Mathf.Clamp(ta, 0.0f, 1.0f);

            t = Mathf.Sqrt(1 - Mathf.Pow(t - 1, 2));

            //this.transform.position = Vector3.Lerp(startPos, lastPos, t);

            if (isDown)
            {
                Vector3 pos = Vector3.zero;
                pos.x = (startPos.x + (lastPos.x - startPos.x) * downT) - ((lastPos.x - startPos.x) * (t - downT));
                pos.y = (startPos.y + (lastPos.y - startPos.y) * downT) - (0.1f * ((t - downT) / (1.0f - downT)));

                this.transform.position = pos;

                if (lastPos.x > startPos.x)
                {
                    isFlipX = false;
                }
                else
                {
                    isFlipX = true;
                }
            }
            else
            {

                this.transform.position = startPos + (lastPos - startPos) * t;
            }
            RunTime += Time.deltaTime;
            spriteRenderer.flipX = isFlipX;

            if (t >= downT)
            {
                isDown = true;
            }

            if(ta >= 1.0f)
            {
                Destroy(this.gameObject);
            }
        }

        
    }

    public void SetStats(Vector3 startpos,Vector3 lastpos,float runtime)
    {
        startPos = startpos;
        lastPos = lastpos;
        RunTimeMax = runtime;

        if(lastPos.x > startPos.x)
        {
            isFlipX = true;
        }

        isRun = true;

        Debug.Log("BOOOOOYS");
    }
}
