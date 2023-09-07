using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class CrowScript : MonoBehaviour
{
    private FeatherAParticlesManager featherA;

    public string targetTag = "Child"; // 検索対象のTag名
    public Vector3 targetPos;
    public float moveSpeed = 1.0f;
    private float coolTime = 5.0f;
    private float kMaxcoolTime = 5.0f;
    private Transform closestChild = null;
    public bool lockOn;
    public bool isTakeAway;
    float angleX;
    float angleY;
    public enum Mode
    {
        stay,
        attak,
        leave,
        takeaway
    };
    [SerializeField] private Mode mode;

    // Start is called before the first frame update
    void Start()
    {
        featherA = GetComponent<FeatherAParticlesManager>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (mode)
        {
            case Mode.stay:
                angleX += Time.deltaTime;
                angleY += Time.deltaTime*10;
                FindClosestChild();
               transform.position= Vector2.MoveTowards(transform.position, new((closestChild.transform.position.x+Mathf.Sin(angleX)*5), (14.0f+Mathf.Sin(angleY)*0.5f)), Time.deltaTime*moveSpeed);
                coolTime -= Time.deltaTime;
                if (coolTime < 0)
                {
                    mode = Mode.attak;
                }
                isTakeAway = false;
                break;
            case Mode.attak:
                coolTime = kMaxcoolTime;
                Attak();
                float distance = Vector2.Distance( transform.position, targetPos);
                if (distance <= 0.1f)
                {
                    mode = Mode.stay;
                }
                break;
            case Mode.leave:



                break;
            case Mode.takeaway:
                transform.position += new Vector3(0, moveSpeed, 0)*Time.deltaTime;
                closestChild.transform.position = transform.position;
                closestChild.GetComponent<ChildManager>().isTakedAway = true;

                break;



        }

    }

    private void Attak()
    {
        Vector3 direction = targetPos - transform.position;

        // 方向ベクトルを正規化（長さを1にする）
        direction.Normalize();

        // 目標位置の方向に一定速度で移動
        transform.position += direction * moveSpeed * Time.deltaTime;


    }

    void FindClosestChild()
    {
        GameObject[] children = GameObject.FindGameObjectsWithTag(targetTag);

        float closestDistance = float.MaxValue;

        foreach (GameObject child in children)
        {
            float distanceToChild = Vector3.Distance(transform.position, child.transform.position);

            if (distanceToChild < closestDistance)
            {
                closestDistance = distanceToChild;
                closestChild = child.transform;
            }
        }

       
        if (closestChild != null)
        {
            lockOn = true;
            targetPos = closestChild.transform.position;
            
        }
        else
        {
            lockOn = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(targetTag))
        {
            if (mode == Mode.attak)
            {
                mode = Mode.takeaway;
                if (!isTakeAway) featherA.SetRunning(collision.transform.position);
                isTakeAway = true;
                
            }
           
            //collision.gameObject.GetComponent<ChildManager>().isTakedAway = true;
            //closestChild.transform.parent = transform;

        }
        else if (collision.CompareTag("Ground")&&!isTakeAway)
        {
            mode = Mode.stay;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(targetTag))
        {
            //closestChild.GetComponent<ChildManager>().isTakedAway = true;
        }
    }
}
