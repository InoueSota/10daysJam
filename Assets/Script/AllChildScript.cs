using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllChildScript : MonoBehaviour
{
    public int stackCount;

    private float diff;
    private float diffSize = 1.5f;

    public void AddChildObjects(GameObject[] children)
    {
        // éqÇêîÇ¶ÇÈ
        int childCount_ = 0;

        foreach (Transform child in transform)
        {
            children[childCount_] = child.gameObject;
            childCount_++;
            ResultManager.childCount = childCount_;
        }
    }

    public int ChildrenCount()
    {
        // éqÇêîÇ¶ÇÈ
        int childCount_ = 0;

        foreach (Transform child in transform)
        {
            childCount_++;
        }

        return childCount_;
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

    public void SubtractDiffSize()
    {
        diff -= diffSize;
    }
}