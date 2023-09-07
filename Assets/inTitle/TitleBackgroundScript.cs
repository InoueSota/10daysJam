using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TitleBackgroundScript : MonoBehaviour
{
    //private float backPoint = 0;
    private float width = 0;

    [SerializeField] private float speed = 0;
    private float multiSpeed = 1;

    // Start is called before the first frame update
    void Start()
    {
        width = GetComponent<SpriteRenderer>().bounds.size.x;


    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = this.transform.position;

        pos.x -= speed * Time.deltaTime * multiSpeed;

        if (pos.x <= -width)
        {
            pos.x += width * 2;
        }

        this.transform.position = pos;
    }

    public void SetSpeed(float speed_)
    {

        speed = speed_;

    }

    public void SetMultiSpeed(float m)
    {
        multiSpeed = m;
    }
}
