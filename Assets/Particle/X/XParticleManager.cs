using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class XParticleManager : MonoBehaviour
{

    [SerializeField] private float finishTime = 1.0f;
    private float eTime = 0.0f;

    [SerializeField] private XScript X = null;
    private XScript x = null;

    [SerializeField] private float sizeBig = 10.0f;

    //private GameObject camera;
    //private float shakeRadius = 0.0f;
    //[SerializeField] private float shakeRadiusMax = 1.0f;

    //[SerializeField] private float shakeTime = 1.0f;
    //private float shakeETime = 0;
    //camera = Camera.main.gameObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(x != null)
        {
            if (eTime < finishTime)
            {
                eTime += Time.deltaTime;
                float t = eTime / finishTime;

                float ta = t * t * t;

                ta = Mathf.Clamp01(ta);
                float size = sizeBig * (1.0f - ta) + 1.0f;

                Vector3 scale = new Vector3(size, size,0.0f);
                x.transform.localScale = scale;

                x.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, ta);

            }

            
        }
    }

    public void Set()
    {
        if (x == null)
        {
            x = Instantiate(X, this.transform.position, Quaternion.identity);
            x.transform.parent = this.transform;
            x.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        }
    }
}
