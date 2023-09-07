using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllChildScript : MonoBehaviour
{
    public int stackCount;

    private float diff;
    private float diffSize = 1.5f;

    void Start()
    {
        // 最初に一度だけ子オブジェクトを数える
        CountStackCount();
    }

    void Update()
    {
        //CountStackCount();
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

    public void AddChildObjects(GameObject[] children)
    {
        // 子を数える
        int childCount = 0;

        foreach (Transform child in transform)
        {
            children[childCount] = child.gameObject;
            childCount++;
        }
    }

    public void DiffInitialize()
    {
        diff = 0.5f;

        foreach (Transform child in transform)
        {
            ChildManager childManager = child.GetComponent<ChildManager>();
            if (childManager)
            {
                childManager.isAddDiff = false;
            }
        }
    }

    public float AddDiffSize()
    {
        diff += diffSize;
        return diff;
    }
}