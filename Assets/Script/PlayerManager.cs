using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    Rigidbody2D rb;
    SpriteRenderer sP;
    float halfSize = 0f;
    
    // 入力された速度を格納する（最大１）
    Vector2 inputMove = Vector2.zero;
    // 移動速度
    [SerializeField] float moveSpeed = 0f;
    [SerializeField] float Jumpforce = 0f;
    // 左右どちらを向いているか
    public enum DIRECTION {
        LEFT,
        RIGHT
    }
    DIRECTION direction = DIRECTION.LEFT;

    // 全子ガモ
    public GameObject allChildObj;
    AllChildScript allChild;
    // 子ガモを格納する
    private GameObject[] children;
    public bool isJump;

    // 指示関係
    public bool orderLeft;      // 指示 - 左猛進
    public bool orderRight;     // 指示 - 右猛進
    public bool orderStack;     // 指示 - 積み上げ
    public bool orderDown;      // 指示 - 集合,待機
    public bool orderAttack;    // 指示 - カラスに攻撃

    // カラス関係
    private GameObject[] targets;
    private GameObject closeCrow;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sP = GetComponent<SpriteRenderer>();
        halfSize = transform.localScale.x * 0.5f;
        transform.position = new (transform.position.x + halfSize, transform.position.y + halfSize, transform.position.z);

        allChild = allChildObj.GetComponent<AllChildScript>();
        children = new GameObject[30];
    }

    void Update()
    {
        InputMove();
        Move();
        OrderChildren();
    }
    private void FixedUpdate()
    {
        if (isJump)
        {
            float jumpForce = Jumpforce * Time.deltaTime;
            rb.velocity = new Vector2(rb.velocity.x, Jumpforce);
           // rb.AddForce(Vector3.up * Jumpforce, ForceMode2D.Impulse);
            Debug.Log("jump");
            isJump = false;
        }
    }
    private void OrderChildren()
    {
        // フラグを初期化する
        orderLeft = false;
        orderRight = false;
        orderStack = false;
        orderDown = false;
        orderAttack = false;

        //コントローラー対応お願いします!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        //コードから察してね

        closeCrow = SearchCrow();

        if (closeCrow == null)
        {
            this.GetComponent<SpriteRenderer>().color = Color.white;

            // 指示 - 左猛進
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                if (Input.GetKeyDown(KeyCode.J))
                {
                    orderLeft = true;
                }
            }
            // 指示 - 右猛進
            else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                if (Input.GetKeyDown(KeyCode.J))
                {
                    orderRight = true;
                }
            }
            // 指示 - 積み上げ
            else if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
            {
                if (Input.GetKeyDown(KeyCode.J))
                {
                    orderStack = true;
                }
            }
            // 指示 - 集合,待機
            else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
            {
                if (Input.GetKeyDown(KeyCode.J))
                {
                    orderDown = true;
                }
            }
        }
        else
        {
            this.GetComponent<SpriteRenderer>().color = Color.red;

            // 指示 - カラスに攻撃
            if (Input.GetKeyDown(KeyCode.J))
            {
                allChild.AddChildObjects(children);

                for (int i = 0; i < children.GetLength(0); i++)
                {
                    ChildManager childManager = null;
                    if (children[i])
                    {
                        childManager = children[i].GetComponent<ChildManager>();
                    }
                    if (childManager && !childManager.GetIsThrow())
                    {
                        childManager.ThrowInitialize();
                        break;
                    }
                }
                orderAttack = true;
            }
        }
    }

    void InputMove()
    {
        inputMove = Vector2.zero;

        // X軸移動
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            inputMove.x = -1f;
            direction = DIRECTION.LEFT;
            sP.flipX = true;
        }
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            inputMove.x = 1f;
            direction = DIRECTION.RIGHT;
            sP.flipX = false;
        }
        //ジャンプ処理（Y軸イドウ）
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isJump = true;
        }
       

    }

    void Move()
    {
        float deltaMoveSpeed = moveSpeed * Time.deltaTime;
        transform.position = new Vector3(transform.position.x + inputMove.x * deltaMoveSpeed, transform.position.y, transform.position.z);
    }

    public DIRECTION GetDirection()
    {
        return direction;
    }

    private GameObject SearchCrow()
    {
        targets = GameObject.FindGameObjectsWithTag("Crow");
        GameObject nearCrow = null;

        foreach (GameObject t in targets)
        {
            float tDist = Vector3.Distance(transform.position, t.transform.position);

            if (tDist < 12.0f)
            {
                nearCrow = t;
            }
        }
        return nearCrow;
    }

    public Vector3 GetNearCrawPos()
    {
        return closeCrow.transform.position;
    }
}
