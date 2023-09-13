using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class HopParticle : MonoBehaviour
{

    private Vector3 vel = Vector3.zero;
    private float rotSpeed;

    private float ras;
    private float gravity;

    private float startConTime;
    private float progressConTime;

    private float eTime = 0.0f;


    private bool isPop = false;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (isPop == true)
        {

            if(eTime >= startConTime) {

                float t = (eTime - startConTime) / progressConTime;

                float ta = t;

                ta = Mathf.Clamp(ta,0.0f,1.0f);

                float sizexy = 1.0f - ta;

                Vector3 size = new Vector3(sizexy, sizexy, 0.0f);

                this.transform.localScale = size;

                if(ta >= 1.0f)
                {
                    Destroy(this.gameObject);
                }

            }


            if (Mathf.Abs(vel.x) <= ras * Time.deltaTime)
            {
                vel.x = 0;
            }
            else
            {
                if (vel.x > ras)
                {
                    vel.x -= ras * Time.deltaTime;
                }
                else if (vel.x < -ras)
                {
                    vel.x += ras * Time.deltaTime;
                }
            }

            vel.y -= gravity ;

            this.transform.position += vel * Time.deltaTime;


            Vector3 rot = this.transform.localEulerAngles;

            rot.z += rotSpeed;

            if (rot.z >= 360.0f)
            {
                rot.z -= 360.0f;
            }
            else if(rot.z <= -360.0f)
            {
                rot.z += 360.0f;
            }

            this.transform.localEulerAngles = rot;

            eTime += Time.deltaTime;
        }
    }

    public void Set(float popSpeed, float popAngle,float rotSpeed_,float ras_,float gravity_,float startConTime_,float progressConTime_)
    {
        isPop = true;

        float radian = Mathf.Deg2Rad * popAngle;

        vel.x = popSpeed * Mathf.Cos(radian) ;
        vel.y = popSpeed * Mathf.Sin(radian) ;

        rotSpeed = -rotSpeed_;
        ras = ras_;
        gravity = gravity_;
        startConTime = startConTime_;
        progressConTime = progressConTime_;


        if(vel.x < 0)
        {
            this.GetComponent<SpriteRenderer>().flipX = true;
            rotSpeed *= -1;
        }

    }
}
