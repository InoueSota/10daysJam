using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CatScript : MonoBehaviour
{
    public float pushForce = 10.0f; // 吹っ飛ばす力の大きさ
    enum Mode
    {
        Scan,
        Chase,
        Hikkaku,
        Kuwaeru,

    }
    [SerializeField] Mode mode = Mode.Scan;
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
    bool kuwaeru;
   public Transform closestChild;
    float BakuBakuTime;
   const float kBakuBakuTime=3.0f;
    float chaseCoolTime;
    const float kchaseCoolTime=3.0f;


    Animator anim;

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
            switch (mode)
            {
                case Mode.Scan:
                    if (chaseCoolTime > 0)
                    {
                        chaseCoolTime -= Time.deltaTime;
                    }
                    // ゲームオブジェクトの位置を中心に、半径5の範囲内のオブジェクトを探す
                    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 10f);
        distance = Vector2.Distance(transform.position, target.transform.position);

        switch (mode)
        {
            case Mode.Scan:
                if (chaseCoolTime > 0)
                {
                    chaseCoolTime -= Time.deltaTime;
                }
                // ゲームオブジェクトの位置を中心に、半径5の範囲内のオブジェクトを探す
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 5f);

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
                if (target != null) // targetがnullでないことを確認
                {
                    direction_ = target.transform.position - transform.position;
                    direction_.Normalize();
                   // transform.position += new Vector3(direction_.x * 5, 0, 0) * Time.deltaTime;
                    if (direction_.x > 0)
                    {
                        spriteRenderer.flipX = true;
                        direction_.x = 1.0f;
                    }
                    else
                    {
                        spriteRenderer.flipX = false;
                        direction_.x = -1.0f;
                    }
                    if (distance <= 24.0f)
                    {
                        transform.position +=new Vector3( direction_.x,0,0)*Time.deltaTime*3.0f;
                        if (distance <= 10.0f)
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
                case Mode.Chase:
                    chaseCoolTime = kchaseCoolTime;
                    if (target != null) // targetがnullでないことを確認
                    {
                        direction_ = target.transform.position - transform.position;
                        direction_.Normalize();
                        transform.position += new Vector3(direction_.x * 5, 0, 0) * Time.deltaTime;
                        if (direction_.x > 0)
                        {
                            spriteRenderer.flipX = true;
                            direction_.x = 1.0f;
                        }
                        else
                        {
                            direction_.x = -1.0f;
                            spriteRenderer.flipX = false;
                        }
                        float distance = Vector2.Distance(transform.position, target.transform.position);
                        if (distance <= 7.0f && !isEase)
                        {
                            isEase = true;
                            easePos = target.transform.position;
                            transform.DOMoveX(easePos.x, 1.0f).SetEase(Ease.InBack).OnComplete(EaseEndFanction);
                            isAttack = true;
                            mode = Mode.Hikkaku;
                        }
                    }
                    break;

            case Mode.Hikkaku:
                Debug.Log("hikkaku");
                if (target && target.GetComponent<PlayerManager>().isCatAttack)
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


                        if (closestChild != null)
                        {
                            //lockOn = true;
                            mode = Mode.Kuwaeru;

                        }
                        else
                        {
                            //lockOn = false;
                            //targetPos = player.transform.position;
                            // targetPos = Vector3.zero;
                            mode = Mode.Scan;

                        }
                    }
                    else
                    {
                        mode = Mode.Scan;

                    }
                    target = null;
                    break;

                case Mode.Kuwaeru:
                    if (kuwaeru)
                    {
                        closestChild.position = transform.position;
                        BakuBakuTime -= Time.deltaTime;
                    }
                    else
                    {
                        mode = Mode.Scan;
                    }
                    if (BakuBakuTime < 0)
                    {
                        Destroy(closestChild.gameObject);
                        mode = Mode.Scan;
                    }
                    break;



            }

            Animation();
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

        }
        if (collision.CompareTag("Child")&&mode==Mode.Kuwaeru)
        {
          //collision.transform.position = transform.position;
          kuwaeru=true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) // プレイヤーオブジェクトとの衝突を検出
        {
            // Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
           // isAttack = false;
        }
    }

    void Animation()
    {
        switch (mode)
        {
            case Mode.Scan:
                //anim.SetBool("isAttack", false);
                break;
            case Mode.Chase:
                //anim.SetBool("isAttack", false);
                break;

            case Mode.Hikkaku:
                //anim.SetBool("isAttack", true);
                break;

            case Mode.Kuwaeru:
                //anim.SetBool("isAttack", true);
                break;



        }
    }
}
