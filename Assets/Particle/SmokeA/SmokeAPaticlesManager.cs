using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeAPaticlesManager : MonoBehaviour
{

    private GameObject Particles;
    [SerializeField] private SmokeAParicle smoke;

    [System.Serializable]
    struct RandomValue
    {
        [SerializeField] public float value;
        [SerializeField] public float valueRand;
    }

    [SerializeField] private RandomValue popSpeed;
    [SerializeField] private RandomValue popAngle;

    [SerializeField] private RandomValue popRadius;

    [SerializeField] private float ras;
    [SerializeField] private float gravity;

    [SerializeField] private float startConTime;
    [SerializeField] private float progressConTime;

    [SerializeField] private bool isRunning = false;

    [SerializeField]
    private int smokeCount = 1;

    // Start is called before the first frame update
    void Start()
    {
        Particles = GameObject.FindGameObjectWithTag("Particles");
    }

    // Update is called once per frame
    void Update()
    {
        if (isRunning == true)
        {
            Set(this.transform.position);
            isRunning = false;
        }
    }

    public void Set(Vector3 pos_)
    {

        for (int i = 0; i < smokeCount; i++)
        {
            float popSpeedA = Randomizer(popSpeed.value, popSpeed.valueRand);
            float popAngleA = Randomizer(popAngle.value, popAngle.valueRand);

            float popRadiusA = Randomizer(popRadius.value, popRadius.valueRand);

            Vector3 pos = pos_;

            float randrot = Random.Range(0.0f, 360.0f);

            pos.x += popRadiusA * Mathf.Cos(randrot);
            pos.y += popRadiusA * Mathf.Sin(randrot);

            SmokeAParicle s = Instantiate(smoke, pos, Quaternion.identity);
            s.Set(popSpeedA, popAngleA, ras, gravity, startConTime, progressConTime);
            s.transform.parent = Particles.transform;
        }
            
    }

    private float Randomizer(float num, float rand)
    {
        float Answer = 0;

        Answer = num - rand;
        Answer += Random.Range(0.0f, rand * 2);

        return Answer;
    }
}
