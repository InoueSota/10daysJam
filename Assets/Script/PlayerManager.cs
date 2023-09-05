using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    Rigidbody2D rb;
    float halfSize = 0f;
    
    // --- ��{�ړ� --- //
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
    public bool orderPileUp;   //�q����ςݏグ�Ă�����  
    public bool orderRight; //�X�e�B�b�N���E&�{�^��
    public bool orderLeft; //�X�e�B�b�N����&�{�^��
    public bool orderDown; //�X�e�B�b�N����&�{�^��
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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
        if (orderLeft)
        {
            orderLeft = false;
        }
        if (orderRight)
        {
            orderRight = false;
        }
        if (orderPileUp)
        {
            orderPileUp = false;
        }
        if (orderDown)
        {
            orderDown = false;
        }
        //�R���g���[���[�Ή����肢���܂�!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        //�R�[�h����@���Ă�

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                orderLeft = true;
            }
            else
            {
                orderLeft = false;
            }
        }
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                orderRight = true;
            }
            else
            {
                orderRight = false;
            }
        }
        else if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                orderPileUp = true;
            }
            else
            {
                orderPileUp = false;
            }
        }
        else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                orderDown = true;
            }
            else
            {
                orderDown = false;
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
        }
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            inputMove.x = 1f;
            direction = DIRECTION.RIGHT;
        }
        //�W�����v�����iY���C�h�E�j
        if (Input.GetKeyDown(KeyCode.Space))
        {
            float jumpforce = Jumpforce * Time.deltaTime;
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
}
