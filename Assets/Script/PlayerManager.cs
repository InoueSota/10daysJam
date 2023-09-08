using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    Rigidbody2D rb;
    SpriteRenderer sP;
    float halfSize = 0f;
    
    // ���͂��ꂽ���x���i�[����i�ő�P�j
    Vector2 inputMove = Vector2.zero;
    // �ړ����x
    [SerializeField] float moveSpeed = 0f;
    [SerializeField] float Jumpforce = 0f;
    // ���E�ǂ���������Ă��邩
    public enum DIRECTION {
        LEFT,
        RIGHT
    }
    DIRECTION direction = DIRECTION.RIGHT;

    private enum INPUTDIRECTION
    {
        LEFT,
        RIGHT,
        UP,
        DOWN
    }
    INPUTDIRECTION inputDirection = INPUTDIRECTION.RIGHT;

    // �S�q�K��
    public GameObject allChildObj;
    AllChildScript allChild;
    // �q�K�����i�[����
    private GameObject[] children;
    public bool isJump;

    // �w���֌W
    public bool orderLeft;      // �w�� - ���Ґi
    public bool orderRight;     // �w�� - �E�Ґi
    public bool orderStack;     // �w�� - �ςݏグ
    public bool orderDown;      // �w�� - �W��,�ҋ@
    public bool orderAttack;    // �w�� - �J���X�ɍU��

    // �J���X�֌W
    private GameObject[] targets;
    private GameObject closeCrow;

    // ���͂Ƃ���
    private int inputJump = 0;
    private int preInputJump = 0;

    private int inputOrder = 0;
    private int preInputOrder = 0;

    //��
    private int inputHorizontal = 0;
    //�c
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
        closeCrow = SearchCrow();

        if (inputOrder != 0 && preInputOrder == 0)
        {
            if (closeCrow == null)
            {
                // �w�� - ���Ґi
                if (!orderStack && inputDirection == INPUTDIRECTION.LEFT)
                {
                    OrderInitialize();
                    orderLeft = true;
                    CheckDiffChild(true);
                }
                // �w�� - �E�Ґi
                else if (!orderStack && inputDirection == INPUTDIRECTION.RIGHT)
                {
                    OrderInitialize();
                    orderRight = true;
                    CheckDiffChild(true);
                }
                // �w�� - �ςݏグ
                else if (!orderStack && inputDirection == INPUTDIRECTION.UP)
                {
                    OrderInitialize();
                    allChild.stackCount = 0;
                    allChild.DiffInitialize();
                    orderStack = true;
                }
                // �w�� - �ςݏグ�U��
                else if (orderStack && (inputDirection == INPUTDIRECTION.LEFT || inputDirection == INPUTDIRECTION.RIGHT))
                {
                    StackInitialize();
                }
                // �w�� - �W��,�ҋ@
                else if (inputDirection == INPUTDIRECTION.DOWN)
                {
                    OrderInitialize();
                    allChild.DiffInitialize();
                    orderDown = true;
                }
            }
            else
            {
                // �J���X���߂��ɂ���Ƃ����ςݏグ����悤�ɂ���
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
                else if (!orderStack)
                {
                    // �w�� - �J���X�ɍU��
                    CheckDiffChild(false);
                    orderAttack = true;
                }
            }
        }

        // �F�ύX
        if (!closeCrow)
        {
            GetComponent<SpriteRenderer>().color = Color.white;
        }
        else
        {
            GetComponent<SpriteRenderer>().color = Color.red;
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

        // X���ړ�
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
        //�W�����v�����iY���C�h�E�j
        if (inputJump != 0 && preInputJump == 0)
        {
            isJump = true;
        }

        // ���͂��ꂽ�������擾����
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
        // ���ςݏグ�U��
        if (inputDirection == INPUTDIRECTION.LEFT)
        {
            orderLeft = true;
        }
        // �E�ςݏグ�U��
        else if (inputDirection == INPUTDIRECTION.RIGHT)
        {
            orderRight = true;
        }

        // �S�Ă̎q�K�����擾����
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
        // �S�Ă̎q�K�����擾����
        allChild.AddChildObjects(children);

        GameObject nearChild = null;
        int nearChildNumber = 0;
        bool isAssignment = false;

        // �e�K���Ɉ�ԋ߂��q�K�����擾����
        for (int i = 0; i < children.GetLength(0); i++)
        {
            ChildManager childManager = null;
            if (children[i])
            {
                childManager = children[i].GetComponent<ChildManager>();
            }
            // �w�����o�����Ԃ����肷��
            if (childManager && !childManager.isTakedAway && childManager.isAddDiff &&
                (isOrderDash ||
                (!isOrderDash && !childManager.GetIsThrow())))
            {
                // �����𔻒肷��
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

            // �w���̓��e�ɂ���ĕς���
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
                    // �O�ɂ��炷
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
