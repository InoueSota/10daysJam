using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StanParticlesManager : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private StarParticle starParticle;

    private GameObject[] star = null;
    [SerializeField] private int starCount = 3;

    //星一つ一つの間の距離
    private float starArc = 0;

    private float rot = 30;
    [SerializeField] private float rotSpeed = 2.0f;

    [SerializeField] private float radius = 2.0f;


    [SerializeField] private float posY;

    [SerializeField] private float normalSize = 1.0f;
    [SerializeField] private float sizeVariation = 0.5f;



    private bool isRunnning = false;

    private float destroyTime = 3.0f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isRunnning == true)
        {

            rot += Time.deltaTime * rotSpeed;

            if (rot >= 360.0f)
            {

                rot -= 360.0f;

            }
            MoveStar();

            if (destroyTime <= 0)
            {
                for (int i = 0; i < starCount; i++)
                {
                    isRunnning = false;
                    Destroy(star[i]);
                }
            }

            destroyTime -= Time.deltaTime;
        }
    }

    void MoveStar()
    {
        

        for (int i = 0; i < starCount; i++)
        {
            float rotA = rot + starArc * i;
            if (rotA >= 360.0f) rotA -= 360.0f;

            float radian = Mathf.Deg2Rad * rotA;

            Vector3 starPos = Vector3.zero;

            starPos.x = Mathf.Cos(radian) * radius;
            starPos.y = posY;

            Vector3 starSizeVec3 = Vector3.one;
            float starSize = 1.0f;

            starSize = normalSize + Mathf.Sin(radian) * sizeVariation * normalSize;

            starSizeVec3.x = starSize;
            starSizeVec3.y = starSize;

            star[i].transform.localPosition = starPos;
            star[i].transform.localScale = starSizeVec3;
        }

        
    }

    public void SetRun(float destroytime)
    {
        star = new GameObject[starCount];
        starArc = 360.0f / starCount;

        for (int i = 0; i < starCount; i++)
        {
            star[i] = Instantiate(starParticle.gameObject, this.transform.position, Quaternion.identity);
            //子オブジェクト化
            star[i].transform.parent = this.transform;
        }

        MoveStar();

        isRunnning = true;
        destroyTime = destroytime;
    }
}
