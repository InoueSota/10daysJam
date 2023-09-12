using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StageObjectManager : MonoBehaviour
{
    // カメラのオブジェクト
    private GameObject cameraObj;
    // 画面の横幅の半分
    private float halfWidth;

    // 大きさを変える時間
    private float scaleTime;
    private float scaleLeftTime;
    // 変わる大きさの始終
    private Vector3 startScale;
    private Vector3 endScale;
    // 高さを調整する値
    private float baseHeight;
    // 小 →大 完了フラグ
    private bool isToBigClear;

    void Start()
    {
        cameraObj = GameObject.FindGameObjectWithTag("MainCamera");
        halfWidth = Camera.main.ScreenToWorldPoint(new(Screen.width, 0f, 0f)).x;

        scaleTime = 0.8f;
        scaleLeftTime = scaleTime;
        startScale = new(transform.localScale.x, 0f, 0f);
        endScale = transform.localScale;
        transform.localScale = startScale;

        baseHeight = transform.position.y - endScale.y * 0.5f;
        isToBigClear = false;
    }

    void Update()
    {
        if (CheckEnterCamera())
        {
            scaleLeftTime -= Time.deltaTime;
            if (scaleLeftTime < 0f) { scaleLeftTime = 0f; }
            float t = scaleLeftTime / scaleTime;
            transform.localScale = Vector3.Lerp(endScale, startScale, t * t * t);
            transform.position = new(transform.position.x, baseHeight + transform.localScale.y * 0.5f, transform.position.z);
            if (scaleLeftTime <= 0f && !isToBigClear)
            {
                scaleLeftTime = scaleTime;
                endScale = startScale;
                startScale = transform.localScale;
                isToBigClear = true;
            }
        }
    }

    // カメラの中に入ってるか
    private bool CheckEnterCamera()
    {
        if (cameraObj != null)
        {
            if (!isToBigClear)
            {
                float objectPosition = transform.position.x;
                float cameraRight = cameraObj.transform.position.x + halfWidth - endScale.x * 2f;

                if (objectPosition < cameraRight)
                {
                    return true;
                }
            }
            else
            {
                float objectPosition = transform.position.x;
                float cameraLeft = cameraObj.transform.position.x - halfWidth + startScale.x * 2f;

                if (objectPosition < cameraLeft)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
