using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeatherAParticlesManager : MonoBehaviour
{
    // Start is called before the first frame update

    private bool isRunning = false;
    private Vector3 hitPos = Vector3.zero;

    [SerializeField] private int generateCount = 10;

    [SerializeField] private float shotAngleRand = 45.0f;
    private float shotRadian = 0.0f;

    [SerializeField] private FeatherAParticle particle;


    [SerializeField] private float shotDistance = 1.0f;
    [SerializeField] private float shotDistanceRand = 0.2f;

    [SerializeField] private float RunTimeMax = 0.5f;
    [SerializeField] private float RunTimeMaxRand = 0.2f;

    // Update is called once per frame
    void Update()
    {

        if (particle != null)
        {
            if (isRunning)
            {
                for (int i = 0; i < generateCount; i++)
                {

                    FeatherAParticle particleI = Instantiate(particle, this.transform.position, Quaternion.identity);

                    Vector3 shotVec = Vector3.Normalize(hitPos - this.transform.position);

                    shotRadian = Mathf.Atan2(shotVec.y, shotVec.x);

                    float shotAngle = shotRadian * (180 / Mathf.PI);

                    shotAngle = shotAngle - shotAngleRand;

                    shotAngle += Random.Range(0.0f, shotAngleRand * 2);

                    shotRadian = shotAngle * (Mathf.PI / 180);

                    float shotDistanceA = shotDistance - shotDistanceRand;
                    shotDistanceA += Random.Range(0.0f, shotDistanceRand * 2);

                    shotVec.x = shotDistanceA * Mathf.Cos(shotRadian);
                    shotVec.y = shotDistanceA * Mathf.Sin(shotRadian);

                    //shotVec.x *= shotDistance;
                    //shotVec.y *= shotDistance;

                    Vector3 lastpos = hitPos + shotVec;

                    float RunTimeMaxA = RunTimeMax - RunTimeMaxRand;
                    RunTimeMaxA += Random.Range(0.0f, RunTimeMaxRand * 2);

                    particleI.SetStats(this.transform.position, lastpos, RunTimeMaxA);
                }


                isRunning = false;
            }
        }


    }

    public void SetRunning(Vector3 hitpos)
    {
        hitPos = hitpos;
        isRunning = true;
    }
}
