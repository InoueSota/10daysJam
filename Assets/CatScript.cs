using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class CatScript : MonoBehaviour
{
    public float pushForce = 10.0f; // 吹っ飛ばす力の大きさ
    public enum Mode
    {
        Scan,
        Chase,
        Hikkaku,
        Kuwaeru,

    }
    public Mode mode = Mode.Scan;
    [SerializeField] float distance;
    [SerializeField] Vector2 childPos;
    [SerializeField] GameObject target;
    public string TargetTag = "Player";
    SpriteRenderer spriteRenderer;
    [SerializeField] Vector3 easePos;
    bool isEase;
    public Vector2 direction_;
    GameObject player;
    public bool isAttack;
    [SerializeField] bool kuwaeru;
    public Transform closestChild;
    float BakuBakuTime = kBakuBakuTime;
    const float kBakuBakuTime = 5.0f;
    float chaseCoolTime;
    const float kchaseCoolTime = 3.0f;
    [SerializeField] bool onDanbol;

    Animator anim;
    private Vector3 prePos;
    private bool canFlip = true;

    // カメラ
    private GameObject cameraObj;
    private ScrollManager scrollManager;
    private float halfWidth;
    // カメラの中に入ったかフラグ
    private bool isEnterCamera;

    // ゲーム開始管理オブジェクト
    [SerializeField] private GameObject gameFlagObj;
    private GameFlagManager gameFlagManager;

    //[SerializeField] float distance;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        halfWidth = Camera.main.ScreenToWorldPoint(new(Screen.width, 0f, 0f)).x;
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
            if (direction_.x > 0)
            {
                if (kuwaeru)
                {
                    spriteRenderer.flipX = false;
                }
                else
                {
                    spriteRenderer.flipX = true;
                }
                direction_.x = 1.0f;
            }
            else
            {
                if (kuwaeru)
                {
                    spriteRenderer.flipX = true;
                }
                else
                {
                    spriteRenderer.flipX = false;

                }
                direction_.x = -1.0f;
            }
            if (target != null)
            {
                distance = Vector2.Distance(transform.position, target.transform.position);
                direction_ = target.transform.position - transform.position;
                direction_.Normalize();

            }

            switch (mode)
            {
                case Mode.Scan:
                    if (chaseCoolTime > 0)
                    {
                        chaseCoolTime -= Time.deltaTime;
                    }
                    // ゲームオブジェクトの位置を中心に、半径5の範囲内のオブジェクトを探す
                    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 10f);

                    foreach (Collider2D col in colliders)
                    {
                        //Debug.Log("Detected Object: " + col.gameObject.name);
                        if (col.gameObject.tag == TargetTag)
                        {
                            // 範囲内のオブジェクトへの参照を取得
                            target = col.gameObject;
                            mode = Mode.Chase;
                            // ここでtargetObjectを操作できます
                        }
                    }

                    break;

                case Mode.Chase:
                    chaseCoolTime = kchaseCoolTime;
                    Debug.Log("chase");

                    if (target != null) // targetがnullでないことを確認
                    {
                        direction_ = target.transform.position - transform.position;
                        direction_.Normalize();
                        // transform.position += new Vector3(direction_.x * 5, 0, 0) * Time.deltaTime;

                        if (distance <= 24.0f)
                        {
                            if (!onDanbol)
                            {
                                transform.position += new Vector3(direction_.x * 6.0f, 0, 0) * Time.deltaTime;
                            }
                            if (distance <= 7.0f)
                            {
                                mode = Mode.Hikkaku;
                            }
                        }
                        else
                        {
                            mode = Mode.Scan;
                        }

                    }
                    break;

                case Mode.Hikkaku:
                    Debug.Log("hikkaku");
                    FindClosestChild();
                    if (closestChild != null)
                    {
                        //lockOn = true;
                        //Debug.Log("tabetyaunyaaaaa");
                        //mode = Mode.Kuwaeru;
                    }
                    //else
                    //{
                    //    //lockOn = false;
                    //    //targetPos = player.transform.position;
                    //    // targetPos = Vector3.zero;
                    //    mode = Mode.Scan;

                    //}

                    //else
                    //{
                    //    mode = Mode.Scan;

                    //}
                    // target = null;
                    if (distance > 4.0f)
                    {
                        mode = Mode.Chase;
                    }

                    break;

                case Mode.Kuwaeru:
                    if (closestChild != null)
                    {
                        FindClosestChild();
                    }
                    if (kuwaeru && closestChild != null)
                    {
                        ChildManager childManager = closestChild.GetComponent<ChildManager>();
                        if (!childManager.isTakedAway)
                        {
                            childManager.takeAwayCrowObj = gameObject;
                            childManager.isTakedAway = true;
                            if (childManager.GetMoveType() == ChildManager.MoveType.STACK)
                            {
                                GameObject.FindGameObjectWithTag("ChildManager").GetComponent<AllChildScript>().StackTakeOffUpdate();
                            }
                            else
                            {
                                GameObject.FindGameObjectWithTag("ChildManager").GetComponent<AllChildScript>().TakeOffDiffUpdate();
                            }
                            if (distance >= 7.0f)
                            {
                                mode = Mode.Scan;
                                kuwaeru = false;
                            }
                           
                            
                        }
                        BakuBakuTime -= Time.deltaTime;
                        if (direction_.x < 0)
                        {
                            closestChild.position =new Vector3( transform.position.x+1.5f,transform.position.y-1.0f,0);
                        }
                        if (direction_.x >= 0)
                        {
                            closestChild.position = new Vector3(transform.position.x - 1.5f, transform.position.y - 1.0f, 0);
                        }
                        if (!onDanbol)
                        {
                            transform.position += new Vector3(direction_.x * -4.0f, 0, 0) * Time.deltaTime;
                        }
                    }
                    else
                    {
                        Vector2 direction = target.transform.position - transform.position;
                        direction.Normalize();
                        if (direction.x > 0)
                        {
                            direction.x = 1.0f;
                        }
                        else
                        {
                            direction.x = -1.0f;
                        }
                        if (!onDanbol)
                        {
                            transform.position += new Vector3(direction.x * 2.0f, 0, 0) * Time.deltaTime;
                        }
                    }
                    //else
                    //{
                    //    mode = Mode.Scan;
                    //}
                    if (BakuBakuTime < 0)
                    {
                        mode = Mode.Scan;
                        kuwaeru = false;
                        //Destroy(closestChild.gameObject);
                    }
                    break;



            }


            Debug.Log(mode);

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

        Animation();
    }
    private void FixedUpdate()
    {
        if (isAttack)
        {
            Vector2 forceDirection = direction_.normalized;
            //target.GetComponent<PlayerManager>().velocity = forceDirection;
            //target.transform.position = forceDirection;
        }
    }
    void EaseEndFanction()
    {
        mode = Mode.Hikkaku;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) // プレイヤーオブジェクトとの衝突を検出
        {
            // Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            // isAttack = true;
            collision.GetComponent<PlayerManager>().SetSpeedDown();
            isAttack = true;
        }
        if (collision.CompareTag("Child") && mode == Mode.Kuwaeru)
        {
            //collision.transform.position = transform.position;
            kuwaeru = true;
            if (BakuBakuTime <= 0.2f)
            {
                Destroy(collision.gameObject);
                mode = Mode.Scan;
                kuwaeru = false;
                BakuBakuTime = kBakuBakuTime;
            }
        }
        if (collision.CompareTag("Obstacle"))
        {
            float x = collision.transform.position.x - transform.position.x;
            if (direction_.x > 0 && x > 0)
            {
                onDanbol = true;
            }
            else
            if (direction_.x < 0 && x < 0)
            {
                onDanbol = true;
            }
            else
            {
                onDanbol = false;
            }
        }
        if (collision.CompareTag("Player") && mode == Mode.Hikkaku)
        {
            Debug.Log("tabetyaunyaaaaa");
            mode = Mode.Kuwaeru;
            anim.SetTrigger("Attack");
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Child") && mode == Mode.Kuwaeru)
        {
            //collision.transform.position = transform.position;
            kuwaeru = true;
            if (BakuBakuTime <= 0.2f)
            {
                Destroy(collision.gameObject);
                mode = Mode.Scan;
                kuwaeru = false;
                BakuBakuTime = kBakuBakuTime;
            }
        }
        if (collision.CompareTag("Player") && mode == Mode.Hikkaku)
        {
            Debug.Log("tabetyaunyaaaaa");
            mode = Mode.Kuwaeru;
            anim.SetTrigger("Attack");
        }
        if (collision.CompareTag("Obstacle"))
        {
            float x = collision.transform.position.x - transform.position.x;
            if (direction_.x > 0 && x > 0)
            {
                onDanbol = true;
            }
            else
            if (direction_.x < 0 && x < 0)
            {
                onDanbol = true;
            }
            else
            {
                onDanbol = false;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) // プレイヤーオブジェクトとの衝突を検出
        {
            // Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            isAttack = false;
        }
        if (collision.CompareTag("Obstacle"))
        {
            //mode = Mode.Scan;
            onDanbol = false;
            // Debug.Log("danbo-ru");
        }
    }

    public void rockFlipTrue()
    {

        canFlip = true;
    }

    public void rockFlipFalse()
    {

        canFlip = false;
    }

    void Animation()
    {

        bool isScan = false;
        bool isAttack = false;

        switch (mode)
        {
            case Mode.Scan:
                isScan = true;
                break;
            case Mode.Chase:

                break;

            case Mode.Hikkaku:
                isAttack = true;
                break;

            case Mode.Kuwaeru:

                break;
        }

        anim.SetBool("isScan", isScan);
        anim.SetBool("isAttack", isAttack);

        if (canFlip == true)
        {
            if (this.transform.position.x < prePos.x)
            {
                spriteRenderer.flipX = false;
            }
            else if (this.transform.position.x > prePos.x)
            {
                spriteRenderer.flipX = true;
            }
        }

        prePos = this.transform.position;
    }
    void FindClosestChild()
    {
        GameObject[] children = GameObject.FindGameObjectsWithTag("Child");

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



    }
}
