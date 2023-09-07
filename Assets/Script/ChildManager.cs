using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ChildManager : MonoBehaviour
{
    private Rigidbody2D rb;
    private float halfSize = 0f;
    private bool isFlipX = false;

    // 親ガモを追いかける
    public GameObject player;
    private PlayerManager playerManager;
    // どれくらいの差か
    [SerializeField]private Vector3 diffPosition = Vector3.zero;
    public float diff = 0f;
    public bool isAddDiff = false;
    // 追いかける強さ
    [SerializeField] private float followPower = 0f;

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
    [SerializeField] private float dashSpeed = 8f;

    // 基本移動速度
    [SerializeField] private Vector3 velocity = Vector3.zero;

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

        rb.velocity = velocity;
    }

    // 動きをセットする
    public void ChangeMoveType(MoveType nextMoveType)
    {
        moveType = nextMoveType;
    }

    void GetPlayerDiffPosition()
    {
        if (!isAddDiff)
        {
            if (Mathf.Abs(player.transform.position.x - transform.position.x) < 0.5f)
            {
                if (playerDirection == 0)
                {
                    diff = transform.parent.gameObject.GetComponent<AllChildScript>().AddDiffSize();
                }
                else
                {
                    diff = -transform.parent.gameObject.GetComponent<AllChildScript>().AddDiffSize();
                }
                isAddDiff = true;
            }
            else
            {
                // 親の方に行く - 左
                if (player.transform.position.x - transform.position.x < 0f)
                {
                    velocity.x = -10.0f;
                }
                // 親の方に行く - 右
                else
                {
                    velocity.x = 10.0f;
                }
            }
        }
        diffPosition = new(player.transform.position.x + diff, transform.position.y, transform.position.z);
    }

    // フォロー時の動き
    void MoveFollow()
    {
        // 親とどれくらい離れるかを取得
        GetPlayerDiffPosition();

        if (isAddDiff)
        {
            if (Mathf.Abs(diffPosition.x - transform.position.x) < 1.0f)
            {
                // 離れた座標に向かう
                transform.position += new Vector3(diffPosition.x - transform.position.x, 0f, 0f) * (followPower * Time.deltaTime);
                velocity = Vector3.zero;
            }
            else
            {
                // 親の方に行く - 左
                if (diffPosition.x - transform.position.x < 0f)
                {
                    velocity.x = -10.0f;
                }
                // 親の方に行く - 右
                else
                {
                    velocity.x = 10.0f;
                }
            }
        }
    }

    //ダッシュ時の動き
    void MoveDash()
    {
        velocity.x = dashSpeed * orderDirection;
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
                isAddDiff = false;
                velocity = Vector3.zero;
                ChangeMoveType(MoveType.FOLLOW);
            }
        }
    }

    void ImageFlip()
    {
        // 画像の反転処理は親ガモ依存なので、指示を受けたら反転しないようにする

        if (!isAddDiff)
        {
            if (!isFlipX && velocity.x < 0f)
            {
                isFlipX = true;
            }
            else if (isFlipX && velocity.x > 0f)
            {
                isFlipX = false;
            }
        }
        else
        {
            if (playerDirection == 0)
            {
                isFlipX = true;
            }
            else
            {
                isFlipX = false;
            }
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
            velocity.y = 5.0f;
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
        velocity = velocity.normalized * 30.0f;
        transform.position = player.transform.position;
        attackCrowLeftTime = attackCrowTime;
        isFinishAttackCrow = false;
        isThrow = true;
        ChangeMoveType(MoveType.ATTACKCROW);
    }
}
