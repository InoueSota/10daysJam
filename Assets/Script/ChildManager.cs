using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ChildManager : MonoBehaviour
{
    private Rigidbody2D rb;
    private float halfSize = 0f;
    private bool isFlipX = false;
    private Animator anim;
    private ZanzoesManager zanzo;
    private HopParticlesManager[] Hoppers;
    private HopParticlesManager GrassHop;
    private HopParticlesManager SweatHop;

    // 親ガモを追いかける
    public GameObject player;
    private PlayerManager playerManager;
    // どれくらいの差か
    private Vector3 diffPosition = Vector3.zero;
    public float diff = 0f;
    public bool isAddDiff = false;
    // 追いかける強さ
    private float followPower = 12f;

    // 基本移動速度
    public Vector3 velocity = Vector3.zero;
    // プレイヤーの向き
    public int playerDirection = 1;
    // 接地判定
    [SerializeField]private bool judgeGround = false;

    public enum MoveType
    {
        FOLLOW,     // 親についていく
        DASH,       // 猪突猛進
        TOSTACK,    // 積まれに行く
        STACK,      // 積む
        STACKCANCEL,// 積みを中断する
        STACKATTACK,// 積まれた状態の攻撃
        STAY,       // その場に待機
        ATTACKCROW, // カラスに攻撃
        EATGRASS,   // 草を食べる
        LOST,       // 迷っている
        PANIC       // パニック
    }
    [SerializeField]private MoveType moveType = MoveType.FOLLOW;

    // 猛進フラグ
    private bool isDash = false;
    // 猛進速度
    private float dashSpeed = 12f;
    // 速度を方向に応じて変化させる
    private int orderDirection = 0;
    const int kLeft = -1;
    const int kRight = 1;

    // 待機位置をランダムに設定する
    private float stayRandomPositionX = 0f;

    // 積み上げの高さをカウントで変える
    public int stackIndex = 0;
    // 積み上げ座標
    private Vector3 stackPos = Vector3.zero;
    // 積み上げを中断した時のランダム速度
    private float cancelRandomX = 0f;

    // 攻撃最大時間
    private float attackCrowTime = 0.5f;
    private float attackCrowLeftTime = 0f;
    // 攻撃終了フラグ
    private bool isFinishAttackCrow = false;
    // １つずつ投げるためのフラグ
    private bool isThrow = false;
    // カラスに当たったかフラグ
    private bool isCrawHit = false;
    // カラスに連れられたかフラグ
    public bool isTakedAway = false;
    // 連れられたカラスのオブジェクト
    public GameObject takeAwayCrowObj = null;
    // 攻撃時に獲得できるスコア
    private int attackScore = 0;

    // 食事に掛かる時間
    [SerializeField] private float eatGrassTime = 0f;
    private float eatGrassLeftTime = 0f;
    // 食べたら大きさを変える
    private Vector3 kAddScale = new Vector3(0.15f, 0.15f, 0.15f);
    // 草を格納して食事後に消す
    private GameObject grassObj = null;
    //パワーアップパーティクル
    private PowerUpParticlesManager powerUpParticle = null;

    // 迷っている時間
    [SerializeField] private float lostTime = 0f;
    private float lostLeftTime = 0f;

    // パニック
    private bool isPanic = false;
    private float changeOfDirectionIntervalLeftTime = 0f;

    //1F前のpos
    private Vector3 prePos;

    void Start()
    {
        halfSize = transform.localScale.x * 0.5f;
        rb = this.GetComponent<Rigidbody2D>();

        transform.position = new Vector3(transform.position.x, halfSize, transform.position.z);
        playerManager = player.GetComponent<PlayerManager>();

        powerUpParticle = GetComponent<PowerUpParticlesManager>();
        anim = GetComponent<Animator>();
        prePos = this.transform.position;

        zanzo = GetComponent<ZanzoesManager>();

        Hoppers = this.GetComponents<HopParticlesManager>();

        GrassHop = Hoppers[0];
        SweatHop = Hoppers[1];
    }

    private void FixedUpdate()
    {
        rb.velocity = velocity;
    }

    void Update()
    {
        // カラスに連れられていないとき
        if (!isTakedAway)
        {
            // 指示が通るとき
            if (moveType == MoveType.FOLLOW || moveType == MoveType.STAY)
            {
                // 指示 - 積み上げ
                if (playerManager.orderStack)
                {
                    ChangeMoveType(MoveType.TOSTACK);
                }
            }

            Move();
            
            Gravity();
            
            ImageFlip();
        }
        // カラスに連れられたとき
        else
        {
            if (!takeAwayCrowObj)
            {
                isTakedAway = false;
            }
        }

        Animation();
    }

    void Move()
    {
        playerDirection = (int)playerManager.GetDirection();
        GrassHop.SetRunnning(false);
        switch (moveType)
        {
            case MoveType.FOLLOW:
                MoveFollow();
                // 指示 - 集合,待機
                if (playerManager.orderDown)
                {
                    stayRandomPositionX = Random.Range(player.transform.position.x - 3f, player.transform.position.x + 3f);
                    moveType = MoveType.STAY;
                }
                break;
            case MoveType.DASH:
                MoveDash();
                break;
            case MoveType.TOSTACK:
                MoveToStack();
                break;
            case MoveType.STACK:
                MoveStack();
                // 指示 - 集合,待機
                if (playerManager.orderDown)
                {
                    transform.parent.gameObject.GetComponent<AllChildScript>().stackCount = 0;
                    cancelRandomX = Random.Range(3.0f, 6.0f);
                    ChangeMoveType(MoveType.STACKCANCEL);
                }
                break;
            case MoveType.STACKCANCEL:
                MoveStackCancel();
                break;
            case MoveType.STACKATTACK:
                MoveStackAttack();
                break;
            case MoveType.STAY:

                if (Mathf.Abs(stayRandomPositionX - transform.position.x) < 1.0f)
                {
                    // 離れた座標に向かう
                    velocity = Vector3.zero;
                }
                else
                {
                    // 親の方に行く - 左
                    if (stayRandomPositionX - transform.position.x < 0f)
                    {
                        velocity.x = -12.0f;
                    }
                    // 親の方に行く - 右
                    else
                    {
                        velocity.x = 12.0f;
                    }
                }
                if (playerManager.orderDown)
                {
                    moveType = MoveType.FOLLOW;
                }
                break;
            case MoveType.ATTACKCROW:
                MoveAttackCrow();
                break;
            case MoveType.EATGRASS:
                MoveEatGrass();
                break;
            case MoveType.LOST:
                MoveLost();
                break;
            case MoveType.PANIC:
                MovePanic();
                break;
        }
    }

    // 重力処理
    void Gravity()
    {
        if (!judgeGround && moveType != MoveType.STACK && moveType != MoveType.ATTACKCROW)
        {
            velocity.y -= 3.0f * Time.deltaTime * 9.81f;
        }
        else if (moveType == MoveType.FOLLOW || moveType == MoveType.DASH)
        {
            velocity.y = 0f;
        }
    }

    // 動きをセットする
    void ChangeMoveType(MoveType nextMoveType)
    {
        moveType = nextMoveType;
    }
    public MoveType GetMoveType()
    {
        return moveType;
    }

    // 追いかけ関係
    void GetPlayerDiffPosition()
    {
        if (!isAddDiff)
        {
            if (Mathf.Abs(player.transform.position.x - transform.position.x) < 0.5f)
            {
                float diffSize = transform.parent.gameObject.GetComponent<AllChildScript>().AddDiffSize();
                if (playerDirection == 0)
                {
                    diff = diffSize;
                }
                else
                {
                    diff = -diffSize;
                }
                isAddDiff = true;
            }
            else
            {
                // 親の方に行く - 左
                if (player.transform.position.x - transform.position.x < 0f)
                {
                    velocity.x = -12.0f;
                }
                // 親の方に行く - 右
                else
                {
                    velocity.x = 12.0f;
                }
            }
        }
        diffPosition = new(player.transform.position.x + diff, transform.position.y, transform.position.z);
    }
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
                velocity.x = 0f;
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

    // 猛進関係
    public void DashInitialize(bool isLeft)
    {
        if (isLeft)
        {
            orderDirection = kLeft;
        }
        else
        {
            orderDirection = kRight;
        }
        isAddDiff = false;
        isDash = true;
        ChangeMoveType(MoveType.DASH);
    }
    void MoveDash()
    {
        velocity.x = dashSpeed * orderDirection;
    }

    // 積み上げ関係
    void MoveToStack()
    {
        if (CanReceiveOrder() && Mathf.Abs(player.transform.position.x - transform.position.x) < 0.2f)
        {
            stackIndex = transform.parent.gameObject.GetComponent<AllChildScript>().stackCount;
            stackPos.x = playerManager.transform.position.x;
            stackPos.y = (playerManager.transform.position.y + playerManager.transform.localScale.y * 0.5f) + 0.5f;
            isAddDiff = false;
            transform.parent.gameObject.GetComponent<AllChildScript>().stackCount++;
            ChangeMoveType(MoveType.STACK);
        }
        else
        {
            if (Mathf.Abs(player.transform.position.x - transform.position.x) < 1.0f)
            {
                // 離れた座標に向かう
                transform.position += new Vector3(player.transform.position.x - transform.position.x, 0f, 0f) * (followPower * Time.deltaTime);
                velocity.x = 0f;
            }
            else
            {
                // 親の方に行く - 左
                if (player.transform.position.x - transform.position.x < 0f)
                {
                    velocity.x = -12.0f;
                }
                // 親の方に行く - 右
                else
                {
                    velocity.x = 12.0f;
                }
            }
        }
    }
    void MoveStack()
    {
        Vector3 pos = stackPos;

        pos.x = playerManager.transform.position.x;
        pos.y = (playerManager.transform.position.y + playerManager.transform.localScale.y * 0.5f) + 0.5f + stackIndex * 1f;

        transform.position = pos;
    }
    void MoveStackCancel()
    {
        velocity.x = cancelRandomX;

        if (judgeGround)
        {
            velocity = Vector3.zero;
            ChangeMoveType(MoveType.FOLLOW);
        }
    }
    void MoveStackAttack()
    {
        velocity.x -= 3.0f * Time.deltaTime;

        if (judgeGround)
        {
            velocity = Vector3.zero;
            ChangeMoveType(MoveType.FOLLOW);
        }
    }
    public void StackAttackInitialize(bool isLeft)
    {
        if (isLeft)
        {
            orderDirection = kLeft;
        }
        else
        {
            orderDirection = kRight;
        }
        velocity.x = 8f * orderDirection;
        velocity.y = 8f;
        transform.parent.gameObject.GetComponent<AllChildScript>().stackCount = 0;
        ChangeMoveType(MoveType.STACKATTACK);
    }

    // カラスに攻撃関係
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
        zanzo.SetRunning(true);
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
            zanzo.SetRunning(false);
            if (judgeGround)
            {
                isCrawHit = false;
                isThrow = false;
                isAddDiff = false;
                velocity = Vector3.zero;
                ChangeMoveType(MoveType.FOLLOW);
            }
        }
    }

    // 草食事関係
    void MoveEatGrass()
    {

        eatGrassLeftTime -= Time.deltaTime;
        GrassHop.SetRunnning(true);
        if (eatGrassLeftTime < 0f) 
        {
            transform.localScale += kAddScale;
            powerUpParticle.SetParticle();
            ChangeMoveType(MoveType.FOLLOW);

            if (grassObj)
            {
                Destroy(grassObj);
            }
        }
    }

    // 迷い関係
    void MoveLost()
    {
        // 迷い時間を減らす
        lostLeftTime -= Time.deltaTime;

        if (judgeGround)
        {
            velocity.x = 0f;
        }

        // キョロキョロさせる
        if (lostLeftTime % 2f < 0.5f)
        {
            isFlipX = false;
        }
        else if ((lostLeftTime + 1f) % 2 < 0.5f)
        {
            isFlipX = true;
        }

        // 近くに行ったら戻す
        if (CanReceiveOrder() && Mathf.Abs(player.transform.position.x - transform.position.x) < 3f)
        {
            if (playerManager.orderStack)
            {
                ChangeMoveType(MoveType.TOSTACK);
            }
            else
            {
                ChangeMoveType(MoveType.FOLLOW);
            }
        }

        // 迷いきったらパニックになる
        if (lostLeftTime < 0f)
        {
            float x = Random.Range(-3f, 3f);
            velocity.x = x;
            float randomInterval = Random.Range(0.2f, 1.0f);
            changeOfDirectionIntervalLeftTime = randomInterval;
            isPanic = true;
            ChangeMoveType(MoveType.PANIC);
        }
    }

    // パニック関係
    void MovePanic()
    {
        // 向かう方向を再設定するまでの時間
        changeOfDirectionIntervalLeftTime -= Time.deltaTime;
        if (changeOfDirectionIntervalLeftTime < 0f && judgeGround)
        {
            velocity.x = Random.Range(3f, 6f);
            velocity.y = Random.Range(3f, 6f);
            changeOfDirectionIntervalLeftTime = Random.Range(0.5f, 1.0f);

            // ランダムに取得した数字が0ならX軸速度をマイナスにする
            int randomMinus = Random.Range(0, 99);
            if (randomMinus % 2 == 0)
            {
                velocity.x *= -1f;
            }
        }

        if (CanReceiveOrder() && Mathf.Abs(player.transform.position.x - transform.position.x) < 0.5f)
        {
            if (playerManager.orderStack)
            {
                ChangeMoveType(MoveType.TOSTACK);
            }
            else
            {
                ChangeMoveType(MoveType.FOLLOW);
            }
            isPanic = false;
        }
    }
    public bool GetIsPanic()
    {
        return isPanic;
    }

    // 当たり判定（対象によって行動が変わる）
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Crow"))
        {
            // カラスのHP処理
            EnemyStatus enemyStatus = collision.GetComponent<EnemyStatus>();
            if (isThrow)
            {
                if (enemyStatus)
                {
                    enemyStatus.Damage(1);
                    // ダメージを与えHPがなくなったら死亡
                    if (enemyStatus.GetHP() <= 0)
                    {
                        attackScore = 1000;
                        // スコアを加算する
                        AttackScoreIngame(48, 0.5f, collision);
                        Destroy(collision.gameObject);
                    }
                    else
                    {
                        attackScore = 100;
                        // スコアを加算する
                        AttackScoreIngame(32, 0.5f, collision);
                    }
                }
                velocity.y = 5.0f;
                isCrawHit = true;
            }
            else if (moveType == MoveType.STACKATTACK || moveType == MoveType.DASH)
            {
                if (enemyStatus)
                {
                    enemyStatus.Damage(2);
                    // ダメージを与えHPがなくなったら死亡
                    if (enemyStatus.GetHP() <= 0)
                    {
                        attackScore = 2000;
                        // スコアを加算する
                        AttackScoreIngame(48, 0.5f, collision);
                        Destroy(collision.gameObject);
                    }
                    else
                    {
                        attackScore = 200;
                        // スコアを加算する
                        AttackScoreIngame(32, 0.5f, collision);
                    }
                }
            }
        }

        if (collision.CompareTag("Cat"))
        {
            // カラスのHP処理
            EnemyStatus enemyStatus = collision.GetComponent<EnemyStatus>();
            if (isThrow)
            {
                if (enemyStatus)
                {
                    enemyStatus.Damage(1);
                    // ダメージを与えHPがなくなったら死亡
                    if (enemyStatus.GetHP() <= 0)
                    {
                        attackScore = 1000;
                        // スコアを加算する
                        AttackScoreIngame(48, 0.5f, collision);
                        Destroy(collision.gameObject);
                    }
                    else
                    {
                        attackScore = 100;
                        // スコアを加算する
                        AttackScoreIngame(32, 0.5f, collision);
                    }
                }
                velocity.y = 5.0f;
                isCrawHit = true;
            }
            else if (moveType == MoveType.STACKATTACK || moveType == MoveType.DASH)
            {
                if (enemyStatus)
                {
                    enemyStatus.Damage(2);
                    // ダメージを与えHPがなくなったら死亡
                    if (enemyStatus.GetHP() <= 0)
                    {
                        attackScore = 2000;
                        // スコアを加算する
                        AttackScoreIngame(48, 0.5f, collision);
                        Destroy(collision.gameObject);
                    }
                    else
                    {
                        attackScore = 200;
                        // スコアを加算する
                        AttackScoreIngame(32, 0.5f, collision);
                    }
                }
            }
        }

        // 草に当たったら時間をかけたのちに食べる
        if (isDash && collision.CompareTag("Grass"))
        {
            if (collision && !collision.GetComponent<GrassManager>().GetIsEaten())
            {
                ChangeMoveType(MoveType.EATGRASS);
                velocity = Vector3.zero;
                eatGrassLeftTime = eatGrassTime;
                isDash = false;
                grassObj = collision.gameObject;
                collision.GetComponent<GrassManager>().SetIsEaten();
            }
        }
    }

    private void AttackScoreIngame(int size, float deathTime, Collider2D collision)
    {
        playerManager.GetGameFlowManager().AddScore(attackScore);
        GameObject scoreText = Instantiate(playerManager.GetGameFlowManager().GetComponent<GameFlowManager>().scoreIngamePrefab);
        scoreText.transform.SetParent(playerManager.GetGameFlowManager().GetComponent<GameFlowManager>().canvas.transform, false);
        scoreText.GetComponent<ScoreIngameManager>().Initialized(attackScore, size, deathTime);
        scoreText.transform.position = new(collision.transform.position.x + collision.transform.localScale.x * 0.5f, collision.transform.position.y + collision.transform.localScale.y * 0.5f, collision.transform.position.z);
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
            judgeGround = true;
        }

        // 障害物に当たったら迷う
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            if (isThrow)
            {
                isCrawHit = true;
            }
            else if (isDash)
            {
                ChangeMoveType(MoveType.LOST);
                lostLeftTime = lostTime;
                velocity.x = Random.Range(2f, 7f) * -orderDirection;
                velocity.y = Random.Range(3f, 6f);
                judgeGround = false;
                isDash = false;
            }
            else
            {
                ContactPoint2D[] contacts = collision.contacts;
                Vector3 otherNormal = contacts[0].normal;
                Vector3 upVector = new Vector3(0, 1, 0);
                float dotUN = Vector3.Dot(upVector, otherNormal);
                float dotDeg = Mathf.Acos(dotUN) * Mathf.Rad2Deg;
                if (dotDeg < 45) { judgeGround = true; }
            }
        }
        if (isDash && collision.gameObject.CompareTag("Wall"))
        {
            ChangeMoveType(MoveType.LOST);
            lostLeftTime = lostTime;
            velocity.x = Random.Range(2f, 7f) * -orderDirection;
            velocity.y = Random.Range(3f, 6f);
            judgeGround = false;
            isDash = false;
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        // 接地判定（false → true）
        if (!judgeGround && collision.gameObject.CompareTag("Ground"))
        {
            judgeGround = true;
        }
        if (!judgeGround && collision.gameObject.CompareTag("Obstacle"))
        {
            ContactPoint2D[] contacts = collision.contacts;
            Vector3 otherNormal = contacts[0].normal;
            Vector3 upVector = new Vector3(0, 1, 0);
            float dotUN = Vector3.Dot(upVector, otherNormal);
            float dotDeg = Mathf.Acos(dotUN) * Mathf.Rad2Deg;
            if (dotDeg < 45) { judgeGround = true; }
        }
    }

    // 画像の反転
    void ImageFlip()
    {
        if (((!isAddDiff && moveType != MoveType.TOSTACK) || (moveType == MoveType.TOSTACK && velocity.x != 0f)) && moveType != MoveType.STACK && moveType != MoveType.LOST)
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
        else if ((isAddDiff || moveType == MoveType.STACK || moveType == MoveType.TOSTACK) && moveType != MoveType.LOST)
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

    bool CanReceiveOrder()
    {
        float playerBottomY = player.transform.position.y - playerManager.halfSize;
        float childBottomY = transform.position.y - transform.localScale.y * 0.5f;
        if (judgeGround && playerManager.GetJudgeGround() && Mathf.Abs(playerBottomY - childBottomY) < 0.1f)
        {
            return true;
        }
        return false;
    }

    private void Animation()
    {
        bool isWalk = false;

        if (Mathf.Abs(Mathf.Abs(this.transform.position.x ) - Mathf.Abs(prePos.x)) > 0.01f)
        {
            isWalk = true;
        }



        anim.SetBool("isWalk", isWalk);

        prePos = this.transform.position;
    }
}
