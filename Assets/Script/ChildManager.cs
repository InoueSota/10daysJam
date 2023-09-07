using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ChildManager : MonoBehaviour
{
    private Rigidbody2D rb;
    float halfSize = 0f;

    // �e�K����ǂ�������
    public GameObject player;
    PlayerManager playerManager;
    // �ǂꂭ�炢�̍���
    [SerializeField] float diffValue = 0f;
    Vector2 diffPosition = Vector2.zero;
    float diff = 0f;
    // �ǂ������鋭��
    [SerializeField] float followPower = 0f;

    // �v���C���[�̌���
    private int playerDirection = 1;

    private enum MoveType
    {
        FOLLOW,     // �e�ɂ��Ă���
        DASH,       // ���˖Ґi
        STACK,      // �ς�
        STACKATTACK,// �ς܂ꂽ��Ԃ̍U��
        STAY,       // ���̏�ɑҋ@
        ATTACKCROW, // �J���X�ɍU��
    }
    [SerializeField] private MoveType moveType = MoveType.FOLLOW;

    // �Ґi���x
    [SerializeField] float dashSpeed = 8f;

    // ��{�ړ����x
    Vector3 velocity = Vector3.zero;

    // ���x������ɉ����ĕω�������
    private int orderDirection = 0;
    const int kLeft = -1;
    const int kRight = 1;

    // �ςݏグ�̍������J�E���g�ŕς���
    private static int stackCount = 0;
    private int stackIndex = 0;
    // �ςݏグ�t���O
    public bool isPiledUp = false;
    // �ςݏグ���W
    private Vector3 stackPos = Vector3.zero;

    // �J���X�̍��W
    private Vector3 nearCrawPos = Vector3.zero;
    // �P�������邽�߂̃t���O
    private bool isThrow = false;
    // �J���X�ɓ����������t���O
    private bool isCrawHit = false;
    // �J���X�ɘA���ꂽ���t���O
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
        if (moveType == MoveType.STACK)
        {
            if (playerManager.orderRight == true)
            {
                orderDirection = kRight;

                velocity.x = dashSpeed * orderDirection;
                velocity.y = dashSpeed;
                stackCount = 0;
                isPiledUp = false;
                SetMove(3);
            }
            if (playerManager.orderLeft == true)
            {
                orderDirection = kLeft;

                velocity.x = dashSpeed * orderDirection;
                velocity.y = dashSpeed;
                stackCount = 0;
                isPiledUp = false;
                SetMove(3);
            }
        }
        else
        {
            if (playerManager.orderRight == true)
            {
                orderDirection = kRight;
                SetMove(1);
            }

            if (playerManager.orderLeft == true)
            {
                orderDirection = kLeft;
                SetMove(1);
            }

            if (playerManager.orderStack == true)
            {
                stackIndex = stackCount;
                stackPos.x = playerManager.transform.position.x;
                stackPos.y = (playerManager.transform.position.y + playerManager.transform.localScale.y * 0.5f) + transform.localScale.y * 0.5f;
                isPiledUp = true;
                SetMove(2);
                stackCount++;
            }
        }
        if (playerManager.orderDown == true)
        {
            isPiledUp = false;
            stackCount = 0;
            SetMove(0);
        }

        Move();

        ImageFlip();
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

                MoveAttackCraw();

                break;
        }
    }

    //�������Z�b�g����
    public void SetMove(int type)
    {
        moveType = (MoveType)Enum.ToObject(typeof(MoveType), type);
    }

    //�t�H���[���̓���
    void MoveFollow()
    {
        GetPlayerDiffPosition();
        
        transform.position += new Vector3(diffPosition.x - transform.position.x, 0f, 0f) * (followPower * Time.deltaTime);

        velocity.y -= 3.0f * Time.deltaTime * 9.81f;
        rb.velocity = velocity;
    }

    //�_�b�V�����̓���
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
        velocity.x -= 3.0f * Time.deltaTime ;
        
        rb.velocity = velocity;
    }

    void MoveAttackCraw()
    {
        if(isCrawHit == true)
        {
            velocity.x = 0;
            velocity.y -= 8.0f * Time.deltaTime * 9.81f;

            if (this.transform.position.y < 0.55f)
            {
                isCrawHit = false;
                SetMove(0);
            }
        }

        rb.velocity = velocity;
    }

    void GetPlayerDiffPosition()
    {
        // ���������Ă���Ƃ�
        if (playerDirection == 0)
        {
            diff = diffValue;
        }
        // �E�������Ă���Ƃ�
        else
        {
            diff = -diffValue;
        }

        diffPosition = new(player.transform.position.x + diff, player.transform.position.y);
    }

    void ImageFlip()
    {
        // �摜�̔��]�����͐e�K���ˑ��Ȃ̂ŁA�w�����󂯂��甽�]���Ȃ��悤�ɂ���

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Crow")
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
        nearCrawPos = playerManager.GetNearCrawPos();
        Vector3 playerPos = playerManager.transform.position;

        this.transform.position = playerPos;

        velocity = Vector3.Normalize(nearCrawPos - playerPos) * 16.0f;

        isThrow = true;
        SetMove(5);
    }
}
