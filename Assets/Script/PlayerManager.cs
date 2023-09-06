using System.Collections;
using System.Collections.Generic;
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
    DIRECTION direction = DIRECTION.LEFT;

    // �w���֌W
    public bool orderPileUp;    // �w�� - �q����ςݏグ�Ă�����  
    public bool orderRight;     // �w�� - �E�Ґi
    public bool orderLeft;      // �w�� - ���Ґi
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
    }

    void Update()
    {
        InputMove();
        Move();
        OrderChildren();
    }

    private void OrderChildren()
    {
        orderLeft = false;
        orderRight = false;
        orderPileUp = false;
        orderDown = false;
        orderAttack = false;

        //�R���g���[���[�Ή����肢���܂�!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        //�R�[�h����@���Ă�

        closeCrow = SearchCrow();

        if (closeCrow == null)
        {
            this.GetComponent<SpriteRenderer>().color = Color.white;

            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                if (Input.GetKeyDown(KeyCode.J))
                {
                    orderLeft = true;
                }
            }
            else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                if (Input.GetKeyDown(KeyCode.J))
                {
                    orderRight = true;
                }
            }
            else if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
            {
                if (Input.GetKeyDown(KeyCode.J))
                {
                    orderPileUp = true;
                }
            }
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

            if (Input.GetKeyDown(KeyCode.J))
            {
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
            sP.flipX = true;
        }
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            inputMove.x = 1f;
            direction = DIRECTION.RIGHT;
            sP.flipX = false;
        }
        //�W�����v�����iY���C�h�E�j
        if (Input.GetKeyDown(KeyCode.Space))
        {
            float jumpForce = Jumpforce * Time.deltaTime;
            rb.velocity = new Vector2(rb.velocity.x, Jumpforce);
            // rb.AddForce(Vector3.up * Jumpforce,ForceMode2D.Impulse);
            Debug.Log("jump");
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
