using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class HopParticlesManager : MonoBehaviour
{
    //���񂿂񂿂񂿂񂿂񂿂񂿂񂿂񂿂񂿂񂿂񂿂񂿂񂿂񂿂񂿂񂿂񂿂񂿂񂿂�
    private GameObject Particles;
    [SerializeField] private int popCount = 1;
    [SerializeField] private float popCoolTimeMax = 0.1f;
    private float popCoolTime = 0.0f;
    [SerializeField] private bool isRunnning = false;

    [SerializeField] HopParticle hop;

    private float popSpeedA;
    private float popAngleA;
    private float rotSpeedA;

    [SerializeField] private float ras;
    [SerializeField] private float gravity;

    [SerializeField] private float startConTime;
    [SerializeField] private float progressConTime;

    [System.Serializable]
    struct RandomValue
    {
        [SerializeField] public float value;
        [SerializeField] public float valueRand;
    }

    [SerializeField] private RandomValue popSpeed;
    [SerializeField] private RandomValue popAngle;


    [SerializeField] private RandomValue rotSpeed;

    [SerializeField] private float popRadius;


    // Start is called before the first frame update
    void Start()
    {
        Particles = GameObject.FindGameObjectWithTag("Particles");
    }

    // Update is called once per frame
    void Update()
    {
        if(isRunnning == true)
        {
            if(popCoolTime >= popCoolTimeMax)
            {
                for(int i = 0; i < popCount; i++)
                {

                    

                    Vector3 pos = this.transform.position;

                    float randrot = Random.Range(0.0f, 360.0f);

                    pos.x += popRadius * Mathf.Cos(randrot) ;
                    pos.y += popRadius * Mathf.Sin(randrot) ;

                    HopParticle hopper = Instantiate(hop, pos, Quaternion.identity);
                    hopper.transform.parent = Particles.transform;

                    float popAngleA = Randomizer(popAngle.value, popAngle.valueRand);
                    float popSpeedA = Randomizer(popSpeed.value, popSpeed.valueRand);
                    float rotSpeedA = Randomizer(rotSpeed.value, rotSpeed.valueRand);

                    hopper.Set(popSpeedA, popAngleA,rotSpeedA,ras,gravity,startConTime,progressConTime);

                }

                popCoolTime -= popCoolTimeMax;
            }
            else
            {
                popCoolTime += Time.deltaTime;
            }
        }
    }

    private float Randomizer(float num,float rand)
    {
        float Answer = 0;

        Answer = num - rand;
        Answer += Random.Range(0.0f, rand * 2);

        return Answer;
    }

    public void SetRunnning(bool runnning)
    {
        isRunnning = runnning;
    }
}
