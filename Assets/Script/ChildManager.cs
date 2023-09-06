using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ChildManager : MonoBehaviour
{
    float halfSize = 0f;

    // プレイヤーとの差分座標を目掛ける
    public GameObject player;
    PlayerManager playerManager;
    [SerializeField] float diffValue = 0f;
    [SerializeField] float followValue = 0f;
    float diff = 0f;
    Vector2 diffPosition = Vector2.zero;

    

    private Rigidbody2D rb;

    private int playerDirection = 1;

    [SerializeField] float dashSpeed = 8f;

    private enum MoveType
    {
        Follow,//親についていく
        Dash,//突撃
        Stack,//かさねる
        StackAttack,
        Stay,//そのばたいき
        AttackCraw,
    }

    [SerializeField] private MoveType moveType = MoveType.Follow;

    Vector3 vec = Vector3.zero;


    private int OrderDirection = 0;

    private static int stackCount = 0;
    private int stackIndex = 0;

    public bool isPileUpped = false;

    private Vector3 stackPos = Vector3.zero;

    private Vector3 nearCrawPos = Vector3.zero;
    private static bool isThrow = false;

    private int throwCoolDown = 0;
    private bool isCrawHit = false;


    private Vector3 prePos = Vector3.zero;

    private bool direction = false;

    void Start()
    {
        halfSize = transform.localScale.x * 0.5f;
        transform.position = new Vector3(transform.position.x, halfSize, transform.position.z);
        playerManager = player.GetComponent<PlayerManager>();

        rb = this.GetComponent<Rigidbody2D>();

        prePos = this.transform.position;
    }

    void Update()
    {
        prePos = this.transform.position;

        if (isThrow == false)
        {
            throwCoolDown = 0;
        }
        else { 
            if(throwCoolDown == 1)
            {
                isThrow = false;
            }

            throwCoolDown++; 
        }



        


            if (moveType == MoveType.Stack)
        {
            if (playerManager.orderRight == true)
            {

                OrderDirection = 1;


                vec.x = dashSpeed * OrderDirection;
                vec.y = dashSpeed;
                stackCount = 0;
                isPileUpped = false;
                SetMove(3);
            }

            if (playerManager.orderLeft == true)
            {
                OrderDirection = -1;

                vec.x = dashSpeed * OrderDirection;
                vec.y = dashSpeed;
                isPileUpped = false;
                stackCount = 0;
                SetMove(3);
            }
        }
        else
        {

            if (playerManager.orderRight == true)
            {

                OrderDirection = 1;
                SetMove(1);
            }

            if (playerManager.orderLeft == true)
            {

                OrderDirection = -1;
                SetMove(1);

            }


            if (playerManager.orderPileUp == true)
            {
                stackIndex = stackCount;
                stackPos.x = playerManager.transform.position.x;
                stackPos.y = (playerManager.transform.position.y + playerManager.transform.localScale.y * 0.5f) + transform.localScale.y * 0.5f;
                isPileUpped = true;
                SetMove(2);
                stackCount++;
            }
        }
        if(playerManager.orderDown == true)
        {

            Vector3 vec = Vector3.zero;

            vec.x = 0.0f;
            vec.y = 0.0f;

            isPileUpped = false;
            stackCount = 0;
            SetMove(0);
        }

        if (moveType != MoveType.AttackCraw)
        {
            if (playerManager.orderAttack == true && isThrow == false)
            {

                nearCrawPos = playerManager.GetNeerCrawPos();
                Vector3 playerPos = playerManager.transform.position;

                this.transform.position = playerPos;

                vec = Vector3.Normalize(nearCrawPos - playerPos) * 16.0f;

                isThrow = true;
                SetMove(5);
            }
        }

        Move();

        if (prePos.x - this.transform.position.x < 0.0f)
        {
            direction = false;
        }
        else if (prePos.x - this.transform.position.x > 0.0f)
        {
            direction = true;
        }

        this.GetComponent<SpriteRenderer>().flipX = direction;
    }

    void Move()
    {
        playerDirection = (int)playerManager.GetDirection();


        switch (moveType)
        {
            case MoveType.Follow:

                MoveFollow();

                break;
            case MoveType.Dash:

                MoveDash();

                break;
            case MoveType.Stack:

                MoveStack();

                break;

            case MoveType.StackAttack:

                MoveStackAttack();

                break;
            //case MoveType.Stay:

            //    break;
            case MoveType.AttackCraw:

                MoveAttackCraw();

                break;
        }

        
    }

    //動きをセットする
    public void SetMove(int type)
    {
        moveType = (MoveType)Enum.ToObject(typeof(MoveType), type);

       
    }

    //フォロー時の動き
    void MoveFollow()
    {
        GetPlayerDiffPosition();
        
        transform.position += new Vector3(diffPosition.x - transform.position.x, 0f, 0f) * (followValue * Time.deltaTime);

        vec.y -= 3.0f * Time.deltaTime * 9.81f;
        rb.velocity = vec;
    }

    //ダッシュ時の動き
    void MoveDash()
    {

        vec = Vector3.zero;

        vec.x = dashSpeed;

        
        vec.x *= OrderDirection;
        rb.velocity = vec;
    }

    void MoveStack()
    {

        Vector3 pos = stackPos;

        pos.x = playerManager.transform.position.x;
        pos.y = stackPos.y + stackIndex * transform.localScale.y;

        transform.position = pos;

    }

    void MoveStackAttack()
    {

        

        vec.y -= 3.0f * Time.deltaTime * 9.81f;
        vec.x -= 3.0f * Time.deltaTime ;

        
        rb.velocity = vec;
    }

    void MoveAttackCraw()
    {
        if(isCrawHit == true)
        {
            vec.x = 0;
            vec.y -= 8.0f * Time.deltaTime * 9.81f;

            if (this.transform.position.y < 0.55f)
            {

                isCrawHit = false;
                SetMove(0);
            }
        }


        rb.velocity = vec;
    }

    void GetPlayerDiffPosition()
    {
        // 左を向いているとき
        if (playerDirection == 0)
        {
            diff = diffValue;
        }
        // 右を向いているとき
        else
        {
            diff = -diffValue;
        }

        diffPosition = new(player.transform.position.x + diff, player.transform.position.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Crow")
        {
            isCrawHit = true;
        }

    }



}
