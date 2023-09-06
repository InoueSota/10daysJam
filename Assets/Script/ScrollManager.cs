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
        // 現在のカメラのX座標とプレイヤーのX座標の差分を取得する
        float diffX = playerTransform.position.x - transform.position.x;

        // 前提条件
        if (!isScroll && playerTransform.position.x > 0)
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
        if (isScroll)
        {
            scrollLeftTime -= Time.deltaTime;
            float t = scrollLeftTime / scrollTime;

            transform.position = Vector3.Lerp(scrollEndPosition, scrollStartPosition, t * t * t);
            if (scrollLeftTime < 0f) { isScroll = false; }
        }
    }
}
