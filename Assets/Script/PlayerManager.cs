using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    float halfSize = 0f;

    // --- 基本移動 --- //
    // 入力された速度を格納する（最大１）
    Vector2 inputMove = Vector2.zero;
    // 移動速度
    [SerializeField] float moveSpeed = 0f;
    // 左右どちらを向いているか
    public enum DIRECTION {
        LEFT,
        RIGHT
    }
    DIRECTION direction = DIRECTION.LEFT;

    void Start()
    {
        halfSize = transform.localScale.x * 0.5f;
        transform.position = new (transform.position.x + halfSize, transform.position.y + halfSize, transform.position.z);
    }

    void Update()
    {
        InputMove();
        Move();
    }

    void InputMove()
    {
        inputMove = Vector2.zero;

        // X軸移動
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
