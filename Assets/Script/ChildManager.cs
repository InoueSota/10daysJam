using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ChildManager : MonoBehaviour
{
    private Rigidbody2D rb;
    private float halfSize = 0f;
    private bool isFlipX = false;

    // �e�K����ǂ�������
    public GameObject player;
    private PlayerManager playerManager;
    // �ǂꂭ�炢�̍���
    private Vector3 diffPosition = Vector3.zero;
    public float diff = 0f;
    public bool isAddDiff = false;
    // �ǂ������鋭��
    private float followPower = 12f;

    // ��{�ړ����x
    private Vector3 velocity = Vector3.zero;
    // �v���C���[�̌���
    private int playerDirection = 1;
    // �ڒn����
    [SerializeField]private bool judgeGround = false;

    public enum MoveType
    {
        FOLLOW,     // �e�ɂ��Ă���
        DASH,       // ���˖Ґi
        TOSTACK,    // �ς܂�ɍs��
        STACK,      // �ς�
        STACKCANCEL,// �ς݂𒆒f����
        STACKATTACK,// �ς܂ꂽ��Ԃ̍U��
        STAY,       // ���̏�ɑҋ@
        ATTACKCROW, // �J���X�ɍU��
        EATGRASS,   // ����H�ׂ�
        LOST,       // �����Ă���
        PANIC       // �p�j�b�N
    }
    [SerializeField]private MoveType moveType = MoveType.FOLLOW;

    // �Ґi�t���O
    private bool isDash = false;
    // �Ґi���x
    private float dashSpeed = 12f;
    // ���x������ɉ����ĕω�������
    private int orderDirection = 0;
    const int kLeft = -1;
    const int kRight = 1;

    // �ςݏグ�̍������J�E���g�ŕς���
    private int stackIndex = 0;
    // �ςݏグ���W
    private Vector3 stackPos = Vector3.zero;
    // �ςݏグ�𒆒f�������̃����_�����x
    private float cancelRandomX = 0f;

    // �U���ő厞��
    private float attackCrowTime = 1.5f;
    private float attackCrowLeftTime = 0f;
    // �U���I���t���O
    private bool isFinishAttackCrow = false;
    // �P�������邽�߂̃t���O
    private bool isThrow = false;
    // �J���X�ɓ����������t���O
    private bool isCrawHit = false;
    // �J���X�ɘA���ꂽ���t���O
    public bool isTakedAway = false;

    // �H���Ɋ|���鎞��
    [SerializeField] private float eatGrassTime = 0f;
    private float eatGrassLeftTime = 0f;
    // �H�ׂ���傫����ς���
    private Vector3 kAddScale = new Vector3(0.15f, 0.15f, 0.15f);

    // �����Ă��鎞��
    [SerializeField] private float lostTime = 0f;
    private float lostLeftTime = 0f;

    // �p�j�b�N
    private bool isPanic = false;
    private float changeOfDirectionIntervalLeftTime = 0f;

    void Start()
    {
        halfSize = transform.localScale.x * 0.5f;
        rb = this.GetComponent<Rigidbody2D>();

        transform.position = new Vector3(transform.position.x, halfSize, transform.position.z);
        playerManager = player.GetComponent<PlayerManager>();
    }

    private void FixedUpdate()
    {
        rb.velocity = velocity;
    }

    void Update()
    {
        // �J���X�ɘA����Ă��Ȃ��Ƃ�
        if (!isTakedAway)
        {
            // �w�����ʂ�Ƃ�
            if (moveType != MoveType.STACK && moveType != MoveType.PANIC && (moveType == MoveType.FOLLOW || moveType == MoveType.STACKCANCEL))
            {
                // �w�� - �ςݏグ
                if (playerManager.orderStack)
                {
                    ChangeMoveType(MoveType.TOSTACK);
                }
            }

            Move();
            
            Gravity();
            
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
            case MoveType.TOSTACK:
                MoveToStack();
                break;
            case MoveType.STACK:
                MoveStack();
                // �w�� - �W��,�ҋ@
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
            //case MoveType.Stay:

            //    break;
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

    // �d�͏���
    void Gravity()
    {
        if (!judgeGround && (moveType == MoveType.FOLLOW || moveType == MoveType.STACKCANCEL || moveType == MoveType.STACKATTACK || moveType == MoveType.LOST || moveType == MoveType.PANIC))
        {
            velocity.y -= 3.0f * Time.deltaTime * 9.81f;
        }
        else if (moveType == MoveType.FOLLOW)
        {
            velocity.y = 0f;
        }
    }

    // �������Z�b�g����
    void ChangeMoveType(MoveType nextMoveType)
    {
        moveType = nextMoveType;
    }

    // �ǂ������֌W
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
                // �e�̕��ɍs�� - ��
                if (player.transform.position.x - transform.position.x < 0f)
                {
                    velocity.x = -10.0f;
                }
                // �e�̕��ɍs�� - �E
                else
                {
                    velocity.x = 10.0f;
                }
            }
        }
        diffPosition = new(player.transform.position.x + diff, transform.position.y, transform.position.z);
    }
    void MoveFollow()
    {
        // �e�Ƃǂꂭ�炢����邩���擾
        GetPlayerDiffPosition();

        if (isAddDiff)
        {
            if (Mathf.Abs(diffPosition.x - transform.position.x) < 1.0f)
            {
                // ���ꂽ���W�Ɍ�����
                transform.position += new Vector3(diffPosition.x - transform.position.x, 0f, 0f) * (followPower * Time.deltaTime);
                velocity = Vector3.zero;
            }
            else
            {
                // �e�̕��ɍs�� - ��
                if (diffPosition.x - transform.position.x < 0f)
                {
                    velocity.x = -10.0f;
                }
                // �e�̕��ɍs�� - �E
                else
                {
                    velocity.x = 10.0f;
                }
            }
        }
    }

    // �Ґi�֌W
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

    // �ςݏグ�֌W
    void MoveToStack()
    {
        if (judgeGround)
        {
            if (Mathf.Abs(player.transform.position.x - transform.position.x) < 0.2f)
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
                // �e�̕��ɍs�� - ��
                if (player.transform.position.x - transform.position.x < 0f)
                {
                    velocity.x = -12.0f;
                }
                // �e�̕��ɍs�� - �E
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
        pos.y = stackPos.y + stackIndex * 1f;

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

    // �J���X�ɍU���֌W
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

    // ���H���֌W
    void MoveEatGrass()
    {
        eatGrassLeftTime -= Time.deltaTime;
        if (eatGrassLeftTime < 0f) 
        {
            transform.localScale += kAddScale;
            ChangeMoveType(MoveType.FOLLOW);
        }
    }

    // �����֌W
    void MoveLost()
    {
        // �������Ԃ����炷
        lostLeftTime -= Time.deltaTime;

        if (judgeGround)
        {
            velocity.x = 0f;
        }

        // �L�����L����������
        if (lostLeftTime % 2f < 0.5f)
        {
            isFlipX = false;
        }
        else if ((lostLeftTime + 1f) % 2 < 0.5f)
        {
            isFlipX = true;
        }

        // �߂��ɍs������߂�
        if (judgeGround && Mathf.Abs(player.transform.position.x - transform.position.x) < 5f)
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

        // ������������p�j�b�N�ɂȂ�
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

    // �p�j�b�N�֌W
    void MovePanic()
    {
        // �������������Đݒ肷��܂ł̎���
        changeOfDirectionIntervalLeftTime -= Time.deltaTime;
        if (changeOfDirectionIntervalLeftTime < 0f && judgeGround)
        {
            velocity.x = Random.Range(3f, 6f);
            velocity.y = Random.Range(3f, 6f);
            changeOfDirectionIntervalLeftTime = Random.Range(0.2f, 1.0f);

            // �����_���Ɏ擾����������0�Ȃ�X�����x���}�C�i�X�ɂ���
            int randomMinus = Random.Range(0, 99);
            if (randomMinus % 2 == 0)
            {
                velocity.x *= -1f;
            }
        }

        if (judgeGround && Mathf.Abs(player.transform.position.x - transform.position.x) < 0.5f)
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

    // �����蔻��i�Ώۂɂ���čs�����ς��j
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �J���X�ɓ��������ۂ͍U��
        if (isThrow && collision.CompareTag("Crow"))
        {
            velocity.y = 5.0f;
            isCrawHit = true;
        }

        // ���ɓ��������玞�Ԃ��������̂��ɐH�ׂ�
        if (isDash && collision.CompareTag("Grass"))
        {
            ChangeMoveType(MoveType.EATGRASS);
            velocity = Vector3.zero;
            eatGrassLeftTime = eatGrassTime;
            isDash = false;
        }
    }

    // �ڒn����
    private void OnCollisionExit2D(Collision2D collision)
    {
        // �ڒn����itrue �� false�j
        if (judgeGround && collision.gameObject.CompareTag("Ground"))
        {
            judgeGround = false;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // �ڒn����ifalse �� true�j
        if (!judgeGround && collision.gameObject.CompareTag("Ground"))
        {
            judgeGround = true;
        }

        // ��Q���ɓ������������
        if (isDash && collision.gameObject.CompareTag("Obstacle"))
        {
            ChangeMoveType(MoveType.LOST);
            lostLeftTime = lostTime;
            velocity.x = Random.Range(2f, 7f) * -orderDirection;
            velocity.y = Random.Range(3f, 6f);
            judgeGround = false;
            isDash = false;
        }
    }

    // �摜�̔��]
    void ImageFlip()
    {
        if (!isAddDiff && moveType != MoveType.STACK && moveType != MoveType.LOST)
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
        else if (isAddDiff || moveType == MoveType.STACK && moveType != MoveType.LOST)
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
}
