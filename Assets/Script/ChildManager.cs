using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildManager : MonoBehaviour
{
    float halfSize = 0f;

    // �v���C���[�Ƃ̍������W��ڊ|����
    public GameObject player;
    PlayerManager playerManager;
    [SerializeField] float diffValue = 0f;
    [SerializeField] float followValue = 0f;
    float diff = 0f;
    Vector2 diffPosition = Vector2.zero;

    void Start()
    {
        halfSize = transform.localScale.x * 0.5f;
        transform.position = new Vector3(transform.position.x, halfSize, transform.position.z);
        playerManager = player.GetComponent<PlayerManager>();
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        GetPlayerDiffPosition();

        transform.position += new Vector3(diffPosition.x - transform.position.x, 0f, 0f) * (followValue * Time.deltaTime);
    }

    void GetPlayerDiffPosition()
    {
        // ���������Ă���Ƃ�
        if (playerManager.GetDirection() == 0)
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
}
