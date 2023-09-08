using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
//using static UnityEngine.GraphicsBuffer;

public class CrowScript : MonoBehaviour
{
    private FeatherAParticlesManager featherA;
    public string targetTag = "Child"; // ・ｽ・ｽ・ｽ・ｽ・ｽﾎ象ゑｿｽTag・ｽ・ｽ
    public Vector3 targetPos;
    public Vector3 startPos;
    public float moveSpeed = 1.0f;
    private float coolTime = 5.0f;
    private float kMaxcoolTime = 5.0f;
    private Transform closestChild = null;
    public bool lockOn;
    public bool isTakeAway;
    float angleX;
    float angleY;
    private GameObject player;
    private int direction_ = 1;
   public float easetime = 1.0f;
    public float stanTime = 2.0f;
    const float kStanTime = 2.0f;
    public enum Mode
    {
        stay,
        attak,
        leave,
        takeaway,
        stan
    };
    [SerializeField] private Mode mode;

    // Start is called before the first frame update
    void Start()
    {
        featherA = GetComponent<FeatherAParticlesManager>();
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        switch (mode)
        {
            case Mode.stay:
                angleX += Time.deltaTime;
                angleY += Time.deltaTime * 10;
                FindClosestChild();
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(targetPos.x + (10 * direction_), 14.0f), Time.deltaTime * moveSpeed);
                coolTime -= Time.deltaTime;
                direction_ = UnityEngine.Random.Range(-1, 2);
                transform.position += new Vector3(direction_,0, 0)*Time.deltaTime;

                if (coolTime < 0)
                {
                    mode = Mode.attak;
                    startPos = transform.position;
                    easetime = 1.0f;
                }
                isTakeAway = false;
                break;
            case Mode.attak:
                coolTime = kMaxcoolTime;
                Attak();
                float distance = Vector2.Distance(transform.position, targetPos);
                if (distance <= 0.1f)
                {
                    mode = Mode.stay;
                }
                //if (easetime < 0f)
                //{
                //    mode = Mode.stay;
                //}
                break;
            case Mode.leave:



                break;
            case Mode.takeaway:
                Vector3 direction = targetPos - transform.position;
                // 方向ベクトルを正規化（長さを1にする）
                direction.Normalize();
                transform.position += new Vector3(-direction.x * moveSpeed, moveSpeed, 0) * Time.deltaTime;
                closestChild.transform.position = transform.position;

                closestChild.GetComponent<ChildManager>().isTakedAway = true;
                if (transform.position.y > 20)
                {
                    Destroy(closestChild.gameObject);
                    mode = Mode.stay;
                    coolTime = kMaxcoolTime * 1.5f;
                }

                break;
            case Mode.stan:
                Debug.Log("stan");
                stanTime -= Time.deltaTime;
                if (stanTime < 0)
                {
                    mode = Mode.stay;
                    stanTime = kStanTime;
                }
                break;
        }

    }

    private void Attak()
    {
        easetime -= Time.deltaTime * 0.5f;
        float t = (easetime / 1.0f);
        float y = Mathf.Lerp(targetPos.y, startPos.y, EaseInSine(t));
        float x = Mathf.Lerp(targetPos.x, startPos.x, EaseOutQuart(t*0.8f));
        transform.position = new Vector3(x, y, 0);

        Vector3 direction = targetPos - transform.position;

        // ・ｽ・ｽ・ｽ・ｽ・ｽx・ｽN・ｽg・ｽ・ｽ・ｽｳ規・ｽ・ｽ・ｽi・ｽ・ｽ・ｽ・ｽ・ｽ・ｽ1・ｽﾉゑｿｽ・ｽ・ｽj
        direction.Normalize();

        // ・ｽﾚ標・ｽﾊ置・ｽﾌ包ｿｽ・ｽ・ｽ・ｽﾉ茨ｿｽ闡ｬ・ｽx・ｽﾅ移難ｿｽ
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
            //targetPos = player.transform.position;
            targetPos = Vector3.zero;
        }
    }

    float EaseOutQuint(float t)
    {
        return 1 - Mathf.Pow(1 - t, 5);
    }
    float EaseInCubic(float t)
    {
        return t * t * t;
    }

    float EaseInBack(float t)
    {
        const float c1 = 1.70158f;
        const float c3 = c1 + 1;

        return c3 * t * t * t - c1 * t * t;
    }
    float EaseOutQuart(float t)
    {
        return 1 - Mathf.Pow(1 - t, 4);
    }
    float EaseInCirc(float t)
    {
        return 1 - Mathf.Sqrt(1 - Mathf.Pow(t, 2));
    }
    float EaseInSine(float t)
    {
        return 1 - Mathf.Cos((t * Mathf.PI) / 2);
    }
    float EaseInOutBack(float t)
    {
        const float c1 = 1.70158f;
        const float c2 = c1 * 1.525f;

        return t < 0.5
          ? (Mathf.Pow(2 * t, 2) * ((c2 + 1) * 2 * t - c2)) / 2
          : (Mathf.Pow(2 * t - 2, 2) * ((c2 + 1) * (t * 2 - 2) + c2) + 2) / 2;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(targetTag))
        {
            if (mode == Mode.attak)
            {
                mode = Mode.takeaway;                
                isTakeAway = true;
            }
            if (!isTakeAway) featherA.SetRunning(collision.transform.position);
            //collision.gameObject.GetComponent<ChildManager>().isTakedAway = true;
            //closestChild.transform.parent = transform;

        }
        if (collision.CompareTag("Ground")&& mode == Mode.attak)
        {
            mode = Mode.stan;
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


