using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageObjectManager : MonoBehaviour
{
    // �J�����̃I�u�W�F�N�g
    private GameObject cameraObj;
    // ��ʂ̉����̔���
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
