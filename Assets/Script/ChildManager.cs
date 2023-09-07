using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ChildManager : MonoBehaviour
{
    private Rigidbody2D rb;
    float halfSize = 0f;

    // 親ガモを追いかける
    public GameObject player;
    PlayerManager playerManager;
    // どれくらいの差か
    [SerializeField] float diffValue = 0f;
    Vector3 diffPosition = Vector3.zero;
    float diff = 0f;
    // 追いかける強さ
    [SerializeField] float followPower = 0f;

    // プレイヤーの向き
    private int playerDirection = 1;

    public enum MoveType
    {
        FOLLOW,     // 親についていく
        DASH,       // 猪突猛進
        STACK,      // 積む
        STACKATTACK,// 積まれた状態の攻撃
        STAY,       // その場に待機
        ATTACKCROW, // カラスに攻撃
    }
    [SerializeField] private MoveType moveType = MoveType.FOLLOW;

    // 猛進速度
    [SerializeField] float dashSpeed = 8f;

    // 基本移動速度
    Vector3 velocity = Vector3.zero;

    // 速度を方向に応じて変化させる
    private int orderDirection = 0;
    const int kLeft = -1;
    const int kRight = 1;

    // 積み上げの高さをカウントで変える
    private static int stackCount = 0;
    private int stackIndex = 0;
    // 積み上げフラグ
    public bool isPiledUp = false;
    // 積み上げ座標
    private Vector3 stackPos = Vector3.zero;

    // 攻撃時間
    [SerializeField] private float attackCrowTime = 0f;
    private float attackCrowLeftTime = 0f;
    // 攻撃終了フラグ
    private bool isFinishAttackCrow = false;
    // １つずつ投げるためのフラグ
    [SerializeField] private bool isThrow = false;
    // カラスに当たったかフラグ
    private bool isCrawHit = false;
    // カラスに連れられたかフラグ
    public bool isTakedAway = false;

    void Start()
    {
        halfSize = transform.localScale.x * 0.5f;
        rb = this.GetComponent<Rigidbody2D>();

        transform.position = new Vector3(transform.position.x, halfSize, transform.position.z);
        playerManager = player.GetComponent<PlayerManager>();
    }

    void Update()
    {
        // カラスに連れられていないとき
        if (!isTakedAway)
        {
            // 積み上げられた状態で投げられる
            if (moveType == MoveType.STACK)
            {
                // 左に投げられる
                if (playerManager.orderLeft)
                {
                    orderDirection = kLeft;
                    StackAttackInitialize();
                }
                // 右に投げられる
                if (playerManager.orderRight)
                {
                    orderDirection = kRight;
                    StackAttackInitialize();
                }
            }
            // 積み上げられていないとき
            else
            {
                // 指示 - 左猛進
                if (playerManager.orderLeft)
                {
                    orderDirection = kLeft;
                    ChangeMoveType(MoveType.DASH);
                }
                // 指示 - 右猛進
                if (playerManager.orderRight)
                {
                    orderDirection = kRight;
                    ChangeMoveType(MoveType.DASH);
                }
                // 指示 - 積み上げ
                if (playerManager.orderStack)
                {
                    stackIndex = stackCount;
                    stackPos.x = playerManager.transform.position.x;
                    stackPos.y = (playerManager.transform.position.y + playerManager.transform.localScale.y * 0.5f) + transform.localScale.y * 0.5f;
                    isPiledUp = true;
                    ChangeMoveType(MoveType.STACK);
                    stackCount++;
                }
            }
            // 指示 - 集合,待機
            if (playerManager.orderDown)
            {
                isPiledUp = false;
                stackCount = 0;
                ChangeMoveType(MoveType.FOLLOW);
            }
            Move();
            ImageFlip();
        }
    }

    void Move()
    {
        playerDirection = (int)playerManager.GetDirection();

        switch (moveType)
        {
            case MoveType.FOLLOW:

                MoveFollow();

                break;
            case MoveType.DASH:

                MoveDash();

                break;
            case MoveType.STACK:

                MoveStack();

                break;

            case MoveType.STACKATTACK:

                MoveStackAttack();

                break;
            //case MoveType.Stay:

            //    break;
            case MoveType.ATTACKCROW:

                MoveAttackCrow();

                break;
        }
    }

    //動きをセットする
    public void ChangeMoveType(MoveType nextMoveType)
    {
        moveType = nextMoveType;
    }

    //フォロー時の動き
    void MoveFollow()
    {
        GetPlayerDiffPosition();
        
        transform.position += new Vector3(diffPosition.x - transform.position.x, 0f, 0f) * (followPower * Time.deltaTime);

        velocity.y -= 3.0f * Time.deltaTime * 9.81f;
        rb.velocity = velocity;

        if (isThrow && Vector3.Distance(diffPosition, transform.position) < 0.2f)
        {
            isThrow = false;
        }
    }

    //ダッシュ時の動き
    void MoveDash()
    {
        velocity = Vector3.zero;

        velocity.x = dashSpeed;

        velocity.x *= orderDirection;
        rb.velocity = velocity;
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
        velocity.y -= 3.0f * Time.deltaTime * 9.81f;
        velocity.x -= 3.0f * Time.deltaTime;
        
        rb.velocity = velocity;
    }

    void MoveAttackCrow()
    {
        if (isThrow)
        {
            attackCrowLeftTime -= Time.deltaTime;
            if (attackCrowLeftTime < 0f) { isFinishAttackCrow = true; }
        }
        if(isCrawHit || isFinishAttackCrow)
        {
            velocity.x = 0;
            velocity.y -= 8.0f * Time.deltaTime * 9.81f;

            if (this.transform.position.y < 0.55f)
            {
                isCrawHit = false;
                isThrow = false;
                ChangeMoveType(MoveType.FOLLOW);
            }
        }
        rb.velocity = velocity;
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
        diffPosition = new(player.transform.position.x + diff, transform.position.y, transform.position.z);
    }

    void ImageFlip()
    {
        // 画像の反転処理は親ガモ依存なので、指示を受けたら反転しないようにする

        bool isFlipX;
        if (playerDirection == 0)
        {
            isFlipX = true;
        }
        else
        {
            isFlipX = false;
        }
        this.GetComponent<SpriteRenderer>().flipX = isFlipX;
    }
    void StackAttackInitialize()
    {
        velocity.x = dashSpeed * orderDirection;
        velocity.y = dashSpeed;
        stackCount = 0;
        isPiledUp = false;
        ChangeMoveType(MoveType.STACKATTACK);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Crow"))
        {
            isCrawHit = true;
        }
    }

    public bool GetIsThrow()
    {
        return isThrow;
    }

    public void ThrowInitialize()
    {
        velocity = playerManager.GetNearCrawPos() - player.transform.position;
        velocity = velocity.normalized * 20.0f;
        transform.position = player.transform.position;
        attackCrowLeftTime = attackCrowTime;
        isFinishAttackCrow = false;
        isThrow = true;
        ChangeMoveType(MoveType.ATTACKCROW);
    }
}
