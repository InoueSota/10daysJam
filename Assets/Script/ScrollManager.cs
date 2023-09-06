using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollManager : MonoBehaviour
{
    public GameObject playerObj;
    Transform playerTransform;

    // ��ʂ̉���
    float width = 0;
    // ��ʂ̉����̔���
    float halfWidth = 0;

    // �X�N���[���t���O
    bool isScroll = false;
    // �X�N���[���̋��x
    float scrollLeftTime = 0f;
    [SerializeField] float scrollTime = 0f;
    // �X�N���[���̎n���W
    Vector3 scrollStartPosition = Vector3.zero;
    // �X�N���[���̏I���W
    Vector3 scrollEndPosition = Vector3.zero;

    void Start()
    {
        playerTransform = playerObj.transform;

        width = Camera.main.ScreenToWorldPoint(new(Screen.width, 0f, 0f)).x * 2f;
        halfWidth = width * 0.5f;

        scrollLeftTime = scrollTime;
        scrollStartPosition = transform.position;
    }

    private void LateUpdate()
    {
        CheckPlayerPosition();
        MoveCamera();
    }

    void CheckPlayerPosition()
    {
        // ���݂̃J������X���W�ƃv���C���[��X���W�̍������擾����
        float diffX = playerTransform.position.x - transform.position.x;

        // �O�����
        if (!isScroll && playerTransform.position.x > 0)
        {
            //�������{�Ȃ̂ŉE�ɃX�N���[������
            if (diffX > halfWidth)
            {
                scrollStartPosition = transform.position;
                scrollEndPosition = new(scrollStartPosition.x + width, scrollStartPosition.y, scrollStartPosition.z);
                scrollLeftTime = scrollTime;
                isScroll = true;
            }
            // �������|�Ȃ̂ō��ɃX�N���[������
            else if (diffX < -halfWidth)
            {
                scrollStartPosition = transform.position;
                scrollEndPosition = new(scrollStartPosition.x - width, scrollStartPosition.y, scrollStartPosition.z);
                scrollLeftTime = scrollTime;
                isScroll = true;
            }
        }
    }

    void MoveCamera()
    {
        if (isScroll)
        {
            scrollLeftTime -= Time.deltaTime;
            float t = scrollLeftTime / scrollTime;

            transform.position = Vector3.Lerp(scrollEndPosition, scrollStartPosition, t * t * t);
            if (scrollLeftTime < 0f) { isScroll = false; }
        }
    }
}
