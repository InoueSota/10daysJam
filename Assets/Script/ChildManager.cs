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

    private enum MoveType
    {
        Follow,//親についていく
        Dash,//突撃
        Stack,//かさねる
        Stay//そのばたいき
    }

    [SerializeField] private MoveType moveType = MoveType.Follow;




    private int OrderDirection = 0;


    void Start()
    {
        halfSize = transform.localScale.x * 0.5f;
        transform.position = new Vector3(transform.position.x, halfSize, transform.position.z);
        playerManager = player.GetComponent<PlayerManager>();

        rb = this.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Move();
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

               break;
            //case MoveType.Stay:

            //    break;
        }
    }

    //動きをセットする、1度だけの処理もここでする
    public void SetMove(int type)
    {
        moveType = (MoveType)Enum.ToObject(typeof(MoveType), type);

        switch (moveType)
        {
            case MoveType.Follow:

                break;
            case MoveType.Dash:

                OrderDirection = (int)playerManager.GetDirection();

                break;
            case MoveType.Stack:

                break;
        }
    }

    //フォロー時の動き
    void MoveFollow()
    {
        GetPlayerDiffPosition();

        transform.position += new Vector3(diffPosition.x - transform.position.x, 0f, 0f) * (followValue * Time.deltaTime);
    }

    //ダッシュ時の動き
    void MoveDash()
    {

        Vector3 vec = Vector3.zero;

        vec.x = 3f;

        if (playerDirection != 0)
        {
            vec.x *= -1;
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
}
