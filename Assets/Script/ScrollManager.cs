using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollManager : MonoBehaviour
{
    public GameObject playerObj;
    Transform playerTransform;

    // 画面の横幅
    float width = 0;
    // 画面の横幅の半分
    float halfWidth = 0;

    // スクロールフラグ
    bool isScroll = false;
    // スクロールの強度
    float scrollLeftTime = 0f;
    [SerializeField] float scrollTime = 0f;
    // スクロールの始座標
    Vector3 scrollStartPosition = Vector3.zero;
    // スクロールの終座標
    Vector3 scrollEndPosition = Vector3.zero;

    // オールスクロールフラグ
    bool isAutoScroll = false;
    // スクロール値
    [SerializeField] float scrollValue = 0f;
    // スクロールの強さ
    [SerializeField] float scrollPower;

    //背景ズ
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
        // 現在のカメラのX座標とプレイヤーのX座標の差分を取得する
        float diffX = playerTransform.position.x - transform.position.x;

        // 前提条件
        if (!isScroll)
        {
            //差分が＋なので右にスクロールする
            if (diffX > halfWidth)
            {
                scrollStartPosition = transform.position;
                scrollEndPosition = new(scrollStartPosition.x + width, scrollStartPosition.y, scrollStartPosition.z);
                scrollLeftTime = scrollTime;
                isScroll = true;
            }
            // 差分が－なので左にスクロールする
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

