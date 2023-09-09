using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using static ChildManager;

public class PlayerManager : MonoBehaviour
{
    Rigidbody2D rb;
    SpriteRenderer sP;
    Animator anim;

    public float halfSize = 0f;
    
    // 入力された速度を格納する（最大１）
    Vector2 inputMove = Vector2.zero;
    // 移動速度
    [SerializeField] float moveSpeed = 0f;
    // 基本移動速度を格納する
    public Vector3 velocity = Vector3.zero;
    // ジャンプ
    private bool isJump = false;
    // 接地判定
    [SerializeField] private bool judgeGround = false;
    // 左右どちらを向いているか
    public enum DIRECTION {
        LEFT,
        RIGHT
    }
    private DIRECTION direction = DIRECTION.RIGHT;

    // 障害物に当たりに行けてしまうのを解消する
    private DIRECTION checkIsSameDirection = DIRECTION.RIGHT;
    private bool isEnterObstacle = false;
    private GameObject collisionObstacle = null;

    private enum INPUTDIRECTION
    {
        LEFT,
        RIGHT,
        UP,
        DOWN
    }
    INPUTDIRECTION inputDirection = INPUTDIRECTION.RIGHT;

    // 全子ガモ
    public GameObject allChildObj;
    AllChildScript allChild;
    // 子ガモを格納する
    private GameObject[] children;

    // 指示関係
    public bool orderLeft;      // 指示 - 左猛進
    public bool orderRight;     // 指示 - 右猛進
    public bool orderStack;     // 指示 - 積み上げ
    public bool orderDown;      // 指示 - 集合,待機
    public bool orderAttack;    // 指示 - カラスに攻撃

    // カラス関係
    private GameObject[] targets;
    private GameObject closeCrow;
    public GameObject targetMarkObj;

    // 入力とるやつ
    private int inputJump = 0;
    private int preInputJump = 0;

    private int inputOrder = 0;
    private int preInputOrder = 0;

    //横
    private int inputHorizontal = 0;
    //縦
    private int inputVertical = 0;

    Vector3 prePos = Vector3.zero;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sP = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        halfSize = transform.localScale.x * 0.5f;
        transform.position = new (transform.position.x + halfSize, transform.position.y + halfSize, transform.position.z);

        allChild = allChildObj.GetComponent<AllChildScript>();
        children = new GameObject[30];
    }

    void Update()
    {
        InputMove();
        Move();
        Gravity();
        OrderChildren();
        Animation();
    }
    private void FixedUpdate()
    {
        rb.velocity = velocity;
    }
    private void OrderChildren()
    {
        // 近くのカラスを取得
        closeCrow = SearchCrow();
        // 近くにカラスがいるならターゲットマークをその位置に描画する
        if (closeCrow)
        {
            if (!targetMarkObj.activeInHierarchy)
            {
                targetMarkObj.SetActive(true);
            }
            targetMarkObj.transform.position = closeCrow.transform.position;
        }
        else
        {
            if (targetMarkObj.activeInHierarchy)
            {
                targetMarkObj.SetActive(false);
            }
        }

        orderDown = false;

        if (inputOrder != 0 && preInputOrder == 0)
        {
            if (!closeCrow)
            {
                // 指示 - 左猛進
                if (!orderStack && inputDirection == INPUTDIRECTION.LEFT)
                {
                    OrderInitialize();
                    orderLeft = true;
                    CheckDiffChild(true);
                }
                // 指示 - 右猛進
                else if (!orderStack && inputDirection == INPUTDIRECTION.RIGHT)
                {
                    OrderInitialize();
                    orderRight = true;
                    CheckDiffChild(true);
                }
                // 指示 - 積み上げ
                else if (!orderStack && inputDirection == INPUTDIRECTION.UP)
                {
                    OrderInitialize();
                    allChild.stackCount = 0;
                    allChild.DiffInitialize();
                    orderStack = true;
                }
                // 指示 - 積み上げ攻撃
                else if (orderStack && (inputDirection == INPUTDIRECTION.LEFT || inputDirection == INPUTDIRECTION.RIGHT))
                {
                    StackInitialize();
                }
                // 指示 - 集合,待機
                else if (inputDirection == INPUTDIRECTION.DOWN)
                {
                    OrderInitialize();
                    allChild.DiffInitialize();
                    orderDown = true;
                }
            }
            else
            {
                // カラスが近くにいるときも積み上げられるようにする
                if (!orderStack && inputDirection == INPUTDIRECTION.UP)
                {
                    OrderInitialize();
                    allChild.stackCount = 0;
                    allChild.DiffInitialize();
                    orderStack = true;
                }
                else if (orderStack && (inputDirection == INPUTDIRECTION.LEFT || inputDirection == INPUTDIRECTION.RIGHT))
                {
                    StackInitialize();
                }
                else if (judgeGround && !orderStack)
                {
                    // 指示 - カラスに攻撃
                    CheckDiffChild(false);
                    orderAttack = true;
                }
            }
        }
    }

    private void OrderInitialize()
    {
        orderLeft = false;
        orderRight = false;
        orderStack = false;
        orderDown = false;
        orderAttack = false;
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

        // 障害物に当たりに行けてしまうのを解消する
        CheckOblstacleY();
        if (isEnterObstacle)
        {
            if (checkIsSameDirection == direction)
            {
                inputMove.x = 0f;
            }
            else
            {
                isEnterObstacle = false;
            }
        }

        // 入力された方向を取得する
        if (inputHorizontal < 0)
        {
            inputDirection = INPUTDIRECTION.LEFT;
        }
        else if (inputHorizontal > 0)
        {
            inputDirection = INPUTDIRECTION.RIGHT;
        }
        else if (inputVertical > 0)
        {
            inputDirection = INPUTDIRECTION.UP;
        }
        else if (inputVertical < 0)
        {
            inputDirection = INPUTDIRECTION.DOWN;
        }
    }

    void Move()
    {
        float deltaMoveSpeed = moveSpeed * Time.deltaTime;
        transform.position = new Vector3(transform.position.x + inputMove.x * deltaMoveSpeed, transform.position.y, transform.position.z);

        //ジャンプ処理（Y軸イドウ）
        if (inputJump != 0 && preInputJump == 0)
        {
            velocity.y = 13f;
            isJump = true;
        }
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

    private void StackInitialize()
    {
        OrderInitialize();
        allChild.DiffInitialize();
        // 左積み上げ攻撃
        if (inputDirection == INPUTDIRECTION.LEFT)
        {
            orderLeft = true;
        }
        // 右積み上げ攻撃
        else if (inputDirection == INPUTDIRECTION.RIGHT)
        {
            orderRight = true;
        }

        // 全ての子ガモを取得する
        allChild.AddChildObjects(children);
        for (int i = 0; i < children.GetLength(0); i++)
        {
            if (children[i])
            {
                children[i].GetComponent<ChildManager>().StackAttackInitialize(orderLeft);
            }
        }
    }

    private void CheckDiffChild(bool isOrderDash)
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
            if (childManager && !childManager.isTakedAway && childManager.isAddDiff &&
                (isOrderDash ||
                (!isOrderDash && !childManager.GetIsThrow())))
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
            if (isOrderDash)
            {
                childManager.DashInitialize(orderLeft);
            }
            else
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

    // 接地判定
    private void OnCollisionExit2D(Collision2D collision)
    {
        // 接地判定（true → false）
        if (judgeGround && collision.gameObject.CompareTag("Ground"))
        {
            judgeGround = false;
        }
        if (judgeGround && collision.gameObject.CompareTag("Obstacle"))
        {
            judgeGround = false;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 接地判定（false → true）
        if (!judgeGround && collision.gameObject.CompareTag("Ground"))
        {
            isJump = false;
            judgeGround = true;
        }

        if (collision.gameObject.CompareTag("Obstacle"))
        {
            ContactPoint2D[] contacts = collision.contacts;
            Vector3 otherNormal = contacts[0].normal;
            float dotUN = Vector3.Dot(Vector3.up, otherNormal);
            float dotDeg = Mathf.Acos(dotUN) * Mathf.Rad2Deg;
            if (dotDeg < 45) { judgeGround = true; isJump = false; }
            else if (dotDeg >= 45 && dotDeg <= 135)
            {
                collisionObstacle = collision.gameObject;
                checkIsSameDirection = direction;
                isEnterObstacle = true;
            }
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            isEnterObstacle = true;
            checkIsSameDirection = direction;
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        // 接地判定（false → true）
        if (!judgeGround && collision.gameObject.CompareTag("Ground"))
        {
            isJump = false;
            judgeGround = true;
        }
        if (!judgeGround && collision.gameObject.CompareTag("Obstacle"))
        {
            ContactPoint2D[] contacts = collision.contacts;
            Vector3 otherNormal = contacts[0].normal;
            float dotUN = Vector3.Dot(Vector3.up, otherNormal);
            float dotDeg = Mathf.Acos(dotUN) * Mathf.Rad2Deg;
            if (dotDeg < 45) { judgeGround = true; isJump = false; }
        }
    }
    public bool GetJudgeGround()
    {
        return judgeGround;
    }
    // 重力処理
    void Gravity()
    {
        if (!judgeGround)
        {
            velocity.y -= 5.0f * Time.deltaTime * 9.81f;
        }
        else if (!isJump)
        {
            velocity.y = 0f;
        }
    }

    // 障害物に当たったのち、障害物に当たることのない高さにまで上昇したら横移動を可能にする
    void CheckOblstacleY()
    {
        if (collisionObstacle && isEnterObstacle)
        {
            // 高さの判定
            if (transform.position.y - halfSize > collisionObstacle.transform.position.y + collisionObstacle.transform.localScale.y * 0.5f ||
                transform.position.y + halfSize < collisionObstacle.transform.position.y - collisionObstacle.transform.localScale.y * 0.5f)
            {
                isEnterObstacle = false;
            }
        }
    }

    void Animation()
    {
        Vector3 thisPos = this.transform.position;

        bool isWalk = false;
        bool jump = false;
        bool isJump = false;
        bool isFall = false;
        bool flight = false;

        if (thisPos.x != prePos.x)
        {
            isWalk = true;
        }

        if (velocity.y > 0)
        {
            isJump = true;
        }

        if (velocity.y < 0)
        {
            isFall = true;
        }

        if (inputJump != 0 && preInputJump == 0)
        {
            if (judgeGround == true)
            {
                jump = true;
            }
            else
            {
                flight = true;
            }
        }

        anim.SetBool("isWalk", isWalk);

        anim.SetBool("isFall", isFall);
        anim.SetBool("isJump", isJump);
        if (jump == true) anim.SetTrigger("jump");
        if (flight == true) anim.SetTrigger("flight");

        prePos = thisPos;
    }
}
