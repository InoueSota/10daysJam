using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
//using static UnityEngine.GraphicsBuffer;

public class CrowScript : MonoBehaviour
{
    private FeatherAParticlesManager featherA;
    private StanParticlesManager stan;
    private float halfWidth;
    private float halfHeight;

    public string targetTag = "Child"; // �E��E��E��E��E�Ώۂ�Tag�E��E�
    public Vector3 targetPos;
    public Vector3 startPos;
    public float moveSpeed = 1.0f;
    private float coolTime = 5.0f;
    [SerializeField] private float kMaxcoolTime = 5.0f;
    private Transform closestChild = null;
    public bool lockOn;
    public bool isTakeAway;
    float angleX;
    float angleY;
    private GameObject player;
    private int direction_ = 1;
    //[SerializeField] float easetime = 1.0f;
    private float stanTime = 4.0f;
    [SerializeField] const float kStanTime = 4.0f;

    // カメラ
    private GameObject cameraObj;
    private ScrollManager scrollManager;
    // カメラの中に入ったかフラグ
    private bool isEnterCamera;

    // ゲーム開始管理オブジェクト
    [SerializeField] private GameObject gameFlagObj;
    private GameFlagManager gameFlagManager;

    public enum Mode
    {
        stay,
        attak,
        leave,
        takeaway,
        stan
    };
    [SerializeField] private Mode mode;
    //[SerializeField] private float DistanceChangeTimeX;
    //[SerializeField] private float DistanceChangeTimeY;
    [SerializeField] float Distance_;
    // Start is called before the first frame update
    void Start()
    {
        featherA = GetComponent<FeatherAParticlesManager>();
        stan = GetComponent<StanParticlesManager>();
        player = GameObject.Find("Player");
        halfWidth = Camera.main.ScreenToWorldPoint(new(Screen.width, 0f, 0f)).x;
        halfHeight = Camera.main.ScreenToWorldPoint(new(0f, Screen.height, 0f)).y;
        cameraObj = GameObject.FindGameObjectWithTag("MainCamera");
        scrollManager = cameraObj.GetComponent<ScrollManager>();
        isEnterCamera = false;
        gameFlagManager = gameFlagObj.GetComponent<GameFlagManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isEnterCamera && gameFlagManager.GetIsStart())
        {
            switch (mode)
            {
                case Mode.stay:
                    angleX += Time.deltaTime;
                    angleY += Time.deltaTime * 10;
                    FindClosestChild();
                    transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, 14.0f), Time.deltaTime * moveSpeed);
                    coolTime -= Time.deltaTime;
                    if ((int)angleX % 3 == 0)
                    {
                        direction_ = UnityEngine.Random.Range(-1, 2);
                    }
                    transform.position += new Vector3(direction_ * moveSpeed, 0, 0) * Time.deltaTime;
                    Distance_ = Vector2.Distance(transform.position, targetPos);
                    //if (Distance_ < 15)
                    //{
                    //    DistanceChangeTimeX=1f;
                    //    DistanceChangeTimeY=1.5f;
                    //}
                    //else
                    //if (Distance_ < 20)
                    //{
                    //    DistanceChangeTimeX=2;
                    //    DistanceChangeTimeY=2.5f;
                    //}
                    //else
                    //if (Distance_ < 25)
                    //{
                    //    DistanceChangeTimeX=2.5f;
                    //    DistanceChangeTimeY=3f;
                    //}
                    //else
                    //{
                    //    DistanceChangeTimeX=3;
                    //    DistanceChangeTimeY=3.5f;
                    //}

                    if (coolTime < 0)
                    {
                        mode = Mode.attak;
                        startPos = transform.position;
                        //easetime = 1.2f;
                        coolTime = kMaxcoolTime;
                        Attak();
                    }
                    isTakeAway = false;
                    break;
                case Mode.attak:

                    // Attak();
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
                    // �����x�N�g���𐳋K���i������1�ɂ���j
                    direction.Normalize();
                    transform.position += new Vector3(-direction.x * moveSpeed, moveSpeed, 0) * Time.deltaTime;
                    if (closestChild)
                    {
                        closestChild.transform.position = transform.position;
                        if (!closestChild.GetComponent<ChildManager>().isTakedAway)
                        {
                            closestChild.GetComponent<ChildManager>().takeAwayCrowObj = gameObject;
                            closestChild.GetComponent<ChildManager>().isTakedAway = true;
                            GameObject.FindGameObjectWithTag("ChildManager").GetComponent<AllChildScript>().TakeOffDiffUpdate();
                        }
                        if (transform.position.y > halfHeight + transform.localScale.y)
                        {
                            Destroy(closestChild.gameObject);
                            mode = Mode.stay;
                            coolTime = kMaxcoolTime * 1.5f;
                        }
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

            // 画面内に収めさせる
            float thisLeft = transform.position.x - transform.localScale.x * 0.5f;
            float thisRight = transform.position.x + transform.localScale.x * 0.5f;
            float cameraLeft = scrollManager.GetScrollValue() - halfWidth;
            float cameraRight = scrollManager.GetScrollValue() + halfWidth;
            if (thisLeft < cameraLeft)
            {
                transform.position = new(cameraLeft + transform.localScale.x * 0.5f, transform.position.y, transform.position.z);
            }
            if (thisRight > cameraRight)
            {
                transform.position = new(cameraRight - transform.localScale.x * 0.5f, transform.position.y, transform.position.z);
            }
        }
        else
        {
            float thisRight = transform.position.x + transform.localScale.x * 0.5f;
            float cameraRight = scrollManager.GetScrollValue() + halfWidth;
            if (thisRight < cameraRight)
            {
                isEnterCamera = true;
            }
        }
    }

    private void Attak()
    {
        transform.DOMoveX(targetPos.x, 2f).SetEase(Ease.InBack);
        transform.DOMoveY(targetPos.y, 2.5f).SetEase(Ease.OutQuad);
       // transform.DOMove(targetPos, 0.5f).SetEase(Ease.OutBounce);
        //Vector3 direction = targetPos - transform.position;

        //// �E��E��E��E��E�x�E�N�E�g�E��E��E�𐳋K�E��E��E�i�E��E��E��E��E��E�1�E�ɂ��E��E�j
        //direction.Normalize();

        //// �E�ڕW�E�ʒu�E�̕��E��E��E�Ɉ�葬�E�x�E�ňړ�
        //transform.position += direction * moveSpeed * Time.deltaTime;



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
            if (!isTakeAway && mode == Mode.attak)
            {
                mode = Mode.takeaway;
                isTakeAway = true;
            }
            if (collision.GetComponent<ChildManager>().GetIsThrow())
            {
                if (!collision.GetComponent<ChildManager>().isTakedAway) featherA.SetRunning(collision.transform.position);
            }
            //collision.gameObject.GetComponent<ChildManager>().isTakedAway = true;
            //closestChild.transform.parent = transform;

        }
        if (collision.CompareTag("Ground")&& mode == Mode.attak)
        {
            mode = Mode.stan;
            stan.SetRun(kStanTime); //スタースタン
            transform.DOKill();//イージングを止める
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(targetTag))
        {
            //closestChild.GetComponent<ChildManager>().isTakedAway = true;
        }

    }

    public bool IsStan()
    {
        if (mode == Mode.stan)
        {
            return true;
        }
        return false;
    }


}


