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

    private enum ORDERPATTERN
    {
        DASH,
        ATTACK
    }
    ORDERPATTERN orderPattern = ORDERPATTERN.ATTACK;

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
        // �t���O������������
        orderLeft = false;
        orderRight = false;
        orderStack = false;
        orderDown = false;
        orderAttack = false;

        //�R���g���[���[�Ή����肢���܂�!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        //�R�[�h����@���Ă�

        closeCrow = SearchCrow();

        if (closeCrow == null)
        {
            this.GetComponent<SpriteRenderer>().color = Color.white;

            // �w�� - ���Ґi
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                if (Input.GetKeyDown(KeyCode.J))
                {
                    orderLeft = true;
                    orderRight = false;
                    CheckDiffChild(ORDERPATTERN.DASH);
                }
            }
            // �w�� - �E�Ґi
            else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                if (Input.GetKeyDown(KeyCode.J))
                {
                    orderLeft = false;
                    orderRight = true;
                    CheckDiffChild(ORDERPATTERN.DASH);
                }
            }
            // �w�� - �ςݏグ
            else if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
            {
                if (Input.GetKeyDown(KeyCode.J))
                {
                    allChild.stackCount = 0;
                    allChild.DiffInitialize();
                    orderStack = true;
                }
            }
            // �w�� - �W��,�ҋ@
            else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
            {
                if (Input.GetKeyDown(KeyCode.J))
                {
                    allChild.DiffInitialize();
                    orderDown = true;
                }
            }
        }
        else
        {
            this.GetComponent<SpriteRenderer>().color = Color.red;

            // �w�� - �J���X�ɍU��
            if (Input.GetKeyDown(KeyCode.J))
            {
                CheckDiffChild(ORDERPATTERN.ATTACK);
                orderAttack = true;
            }
        }
    }

    void InputMove()
    {
        inputMove = Vector2.zero;

        // X���ړ�
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            inputMove.x = -1f;
            direction = DIRECTION.LEFT;
            if (!sP.flipX)
            {
                allChild.DiffInitialize();
                sP.flipX = true;
            }
        }
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
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

    private void CheckDiffChild(ORDERPATTERN orderPattern_)
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
            if (childManager && !childManager.isTakedAway && childManager.isAddDiff && (orderPattern == ORDERPATTERN.DASH || (orderPattern == ORDERPATTERN.ATTACK && !childManager.GetIsThrow())))
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
