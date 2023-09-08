using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
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
    DIRECTION direction = DIRECTION.RIGHT;

    private enum ORDERPATTERN
    {
        DASH,
        ATTACK
    }
    ORDERPATTERN orderPattern = ORDERPATTERN.ATTACK;

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

    //入力とるやつ
    private int inputJump = 0;
    private int preInputJump = 0;

    private int inputOrder = 0;
    private int preInputOrder = 0;

    //横
    private int inputHorizontal = 0;
    //縦
    private int inputVertical = 0;


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
            if (inputHorizontal < 0)
            {
                if (inputOrder != 0 && preInputOrder == 0)
                {
                    orderLeft = true;
                    orderRight = false;
                    CheckDiffChild(ORDERPATTERN.DASH);
                }
            }
            // 指示 - 右猛進
            else if (inputHorizontal > 0)
            {
                if (inputOrder != 0 && preInputOrder == 0)
                {
                    orderLeft = false;
                    orderRight = true;
                    CheckDiffChild(ORDERPATTERN.DASH);
                }
            }
            // 指示 - 積み上げ
            else if (inputVertical > 0)
            {
                if (inputOrder != 0 && preInputOrder == 0)
                {
                    allChild.stackCount = 0;
                    allChild.DiffInitialize();
                    orderStack = true;
                }
            }
            // 指示 - 集合,待機
            else if (inputVertical < 0)
            {
                if (inputOrder != 0 && preInputOrder == 0)
                {
                    allChild.DiffInitialize();
                    orderDown = true;
                }
            }
        }
        else
        {
            this.GetComponent<SpriteRenderer>().color = Color.red;

            // 指示 - カラスに攻撃
            if (inputOrder != 0 && preInputOrder == 0)
            {
                CheckDiffChild(ORDERPATTERN.ATTACK);
                orderAttack = true;
            }
        }
    }

    void InputMove()
    {
        inputMove = Vector2.zero;

        preInputJump = inputJump;
        preInputOrder = inputOrder;

        inputJump = (int)Input.GetAxisRaw("Abutton");
        inputOrder = (int)Input.GetAxisRaw("Xbutton");

        inputHorizontal = (int)Input.GetAxisRaw("Horizontal");
        inputVertical = (int)Input.GetAxisRaw("Vertical");

        // X軸移動
        if (inputHorizontal < 0)
        {
            inputMove.x = -1f;
            direction = DIRECTION.LEFT;
            if (!sP.flipX)
            {
                allChild.DiffInitialize();
                sP.flipX = true;
            }
        }
        else if (inputHorizontal > 0)
        {
            inputMove.x = 1f;
            direction = DIRECTION.RIGHT;
            if (sP.flipX)
            {
                allChild.DiffInitialize();
                sP.flipX = false;
            }
        }
        //ジャンプ処理（Y軸イドウ）
        if (inputJump != 0 && preInputJump == 0)
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

    private void CheckDiffChild(ORDERPATTERN orderPattern_)
    {
        // 全ての子ガモを取得する
        allChild.AddChildObjects(children);

        GameObject nearChild = null;
        int nearChildNumber = 0;
        bool isAssignment = false;

        // 親ガモに一番近い子ガモを取得する
        for (int i = 0; i < children.GetLength(0); i++)
        {
            ChildManager childManager = null;
            if (children[i])
            {
                childManager = children[i].GetComponent<ChildManager>();
            }
            // 指示を出せる状態か判定する
            if (childManager && !childManager.isTakedAway && childManager.isAddDiff && (orderPattern == ORDERPATTERN.DASH || (orderPattern == ORDERPATTERN.ATTACK && !childManager.GetIsThrow())))
            {
                // 距離を判定する
                if (!isAssignment || (nearChild && Vector3.Distance(nearChild.transform.position, transform.position) > Vector3.Distance(children[i].transform.position, transform.position)))
                {
                    nearChild = children[i];
                    nearChildNumber = i;
                    isAssignment = true;
                }
            }
        }

        if (nearChild)
        {
            ChildManager childManager = null;
            if (nearChild)
            {
                childManager = nearChild.GetComponent<ChildManager>();
            }

            // 指示の内容によって変える
            if (orderPattern_ == ORDERPATTERN.DASH)
            {
                childManager.DashInitialize(orderLeft);
            }
            if (orderPattern_ == ORDERPATTERN.ATTACK)
            {
                childManager.ThrowInitialize();
            }

            for (int i = 0; i < children.GetLength(0); i++)
            {
                if (children[i] && nearChildNumber != i)
                {
                    // 前にずらす
                    if (direction == DIRECTION.LEFT)
                    {
                        children[i].GetComponent<ChildManager>().diff -= 1.5f;
                    }
                    else
                    {
                        children[i].GetComponent<ChildManager>().diff += 1.5f;
                    }
                }
            }
            allChild.SubtractDiffSize();
        }
    }
}
