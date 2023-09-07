using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllChildScript : MonoBehaviour
{
    public int stackCount;

    void Start()
    {
        // 最初に一度だけ子オブジェクトを数える
        CountStackCount();
    }

    void Update()
    {
        CountStackCount();
    }

    void CountStackCount()
    {
        stackCount = 0;

        // 子オブジェクトをループして条件をチェック
        foreach (Transform child in transform)
        {
            ChildManager childManager = child.GetComponent<ChildManager>();
            if (childManager != null && childManager.isPiledUp)
            {
                stackCount++;
            }
        }

        // 結果をコンソールに出力
        Debug.Log("stackCount: " + stackCount);
    }
}