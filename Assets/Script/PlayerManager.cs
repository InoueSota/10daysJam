using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    Rigidbody2D rb;
    SpriteRenderer sP;
    float halfSize = 0f;
    
    // --- 基本移動 --- //
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
    public bool orderPileUp;   //子鴨を積み上げている状態  
    public bool orderRight; //スティックが右&ボタン
    public bool orderLeft; //スティックが左&ボタン
    public bool orderDown; //スティックが下&ボタン
    public bool orderAttack;

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
        if (orderDown)
        {
            orderAttack = false;
        }
        //コントローラー対応お願いします!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        //コードから察してね

        closeCrow = SearchCrow();

        if (closeCrow == null)
        {
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
        else
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                orderAttack = true;
            }
            else
            {
                orderAttack = false;
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

    private GameObject SearchCrow()
    {

        targets = GameObject.FindGameObjectsWithTag("Crow");
        GameObject nearCrow = null;
        float closeDist = 1000;

        foreach (GameObject t in targets)
        {

            float tDist = Vector3.Distance(transform.position, t.transform.position);

            if (closeDist > tDist)
            {
                closeDist = tDist;

                nearCrow = t;
            }
        }

        return nearCrow;
    }

    public Vector3 GetNeerCrawPos()
    {
        return closeCrow.transform.position;
    }
}
