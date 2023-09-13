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

    // �I�[���X�N���[���t���O
    bool isAutoScroll = false;
    // �X�N���[���l
    [SerializeField] float scrollValue = 0f;
    // �X�N���[���̋���
    [SerializeField] float scrollPower;

    //�w�i�Y
    [SerializeField] BackGroundScript[] backGroundScript;
    private int backGroundCont = 0;
    private BackGroundScript[] backGrounds;
    private float[] backGroundSize;
    [SerializeField] private float[] backGroundposY;
    [SerializeField] private float[] backGroundScrollPower;

    void Start()
    {
        playerTransform = playerObj.transform;

        width = Camera.main.ScreenToWorldPoint(new(Screen.width, 0f, 0f)).x * 2f;
        halfWidth = width * 0.5f;

        scrollLeftTime = scrollTime;
        scrollStartPosition = transform.position;

        BackGroundInit();
    }

    private void LateUpdate()
    {
        //CheckPlayerPosition();
        MoveCamera();
        BackGround();
    }

    void CheckPlayerPosition()
    {
        // ���݂̃J������X���W�ƃv���C���[��X���W�̍������擾����
        float diffX = playerTransform.position.x - transform.position.x;

        // �O�����
        if (!isScroll)
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
        if (isScroll && !isAutoScroll)
        {
            scrollLeftTime -= Time.deltaTime;
            float t = scrollLeftTime / scrollTime;

            transform.position = Vector3.Lerp(scrollEndPosition, scrollStartPosition, t * t * t);
            if (scrollLeftTime < 0f) { isScroll = false; }
        }
        else if (!isScroll && isAutoScroll)
        {
            float deltaScrollPower = scrollPower * Time.deltaTime;
            scrollValue += deltaScrollPower;

            transform.position = new(scrollValue, transform.position.y, transform.position.z);
        }
    }

    public void SetAutoScrollStart()
    {
        isAutoScroll = true;
    }

    public float GetScrollValue()
    {
        return scrollValue;
    }

    void BackGroundInit()
    {

        backGroundCont = backGroundScript.Length;
        backGrounds = new BackGroundScript[backGroundCont * 2];
        backGroundSize = new float[backGroundCont];
        Debug.Log(backGroundCont);

        Vector3 pos = Vector3.zero;


        for (int i = 0; i < backGroundCont; i++)
        {
            pos = this.transform.position;
            pos.y = 8.05f;
            pos.z = 0;

            backGrounds[i] = Instantiate(backGroundScript[i], pos, Quaternion.identity);
            backGroundSize[i] = backGroundScript[i].gameObject.GetComponent<SpriteRenderer>().bounds.size.x;
        }

        for (int i = backGroundCont; i < backGroundCont * 2; i++)
        {
            pos = this.transform.position;
            pos.y = backGroundposY[i - backGroundCont];
            pos.z = 0;
            pos.x += backGroundSize[i - backGroundCont];

            backGrounds[i] = Instantiate(backGroundScript[i - backGroundCont], pos, Quaternion.identity);
        }


    }

    void BackGround()
    {
        for (int i = 0; i < backGroundCont; i++)
        {
            Vector3 pos = Vector3.zero;
            pos.y = backGroundposY[i];
            pos.z = 0;
            

            float x = -((scrollValue * backGroundScrollPower[i]) % (backGroundSize[i] * 2)) ;
            if(x < -backGroundSize[i])
            {
                x += backGroundSize[i] * 2.0f;
            }
            pos.x = x + scrollValue;
            backGrounds[i].transform.position = pos;

            x = -((scrollValue * backGroundScrollPower[i]) + backGroundSize[i]) % (backGroundSize[i] * 2);
            if (x < -backGroundSize[i])
            {
                x += backGroundSize[i] * 2.0f;
            }
            pos.x = x + scrollValue;
            backGrounds[i + backGroundCont].transform.position = pos;
        }
    }
}

