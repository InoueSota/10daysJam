using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageObjectManager : MonoBehaviour
{
    // カメラのオブジェクト
    private GameObject cameraObj;
    // 画面の横幅の半分
    private float halfWidth;

    void Start()
    {
        cameraObj = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameObject>();
        halfWidth = Camera.main.ScreenToWorldPoint(new(Screen.width, 0f, 0f)).x;
    }

    void Update()
    {
        
    }
}
