using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;

public class AllChildScript : MonoBehaviour
{
    public int stackCount;
    [SerializeField] private int checkStackCount;

    public float diff;
    private float diffSize = 1.5f;

    [SerializeField] private GameObject playerObj;
    private PlayerManager playerManager;
    private GameObject[] children;
    [SerializeField] private float checkDiff;

    void Start()
    {
        playerManager = playerObj.GetComponent<PlayerManager>();

        children = new GameObject[30];
        children = GameObject.FindGameObjectsWithTag("Child");

        ChildrenCount();
    }

    void Update()
    {
        ChildrenCount();
    }

    public void AddChildObjects(GameObject[] children_)
    {
        // éqÇêîÇ¶ÇÈ
        int childCount_ = 0;

        foreach (Transform child in transform)
        {
            children_[childCount_] = child.gameObject;
            childCount_++;
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
        ResultManager.childCount = childCount_;
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
        if (diff < 0.5f)
        {
            diff = 0.5f;
        }
    }

    public void StackTakeOffUpdate()
    {
        for (int i = 0; i < children.GetLength(0); i++)
        {
            if (children[i])
            {
                ChildManager childManager = children[i].GetComponent<ChildManager>();
                if (childManager && childManager.isTakedAway)
                {
                    checkStackCount = childManager.stackIndex;
                    break;
                }
            }
        }

        for (int i = 0; i < children.GetLength(0); i++)
        {
            if (children[i])
            {
                ChildManager childManager = children[i].GetComponent<ChildManager>();
                if (childManager && checkStackCount < childManager.stackIndex)
                {
                    childManager.stackIndex -= 1;
                    continue;
                }
            }
        }
    }

    public void TakeOffDiffUpdate()
    {
        for (int i = 0; i < children.GetLength(0); i++)
        {
            if (children[i])
            {
                ChildManager childManager = children[i].GetComponent<ChildManager>();
                if (childManager && childManager.isTakedAway)
                {
                    checkDiff = childManager.diff;
                    break;
                }
            }
        }

        for (int i = 0; i < children.GetLength(0); i++)
        {
            if (children[i])
            {
                ChildManager childManager = children[i].GetComponent<ChildManager>();
                // ç∂å¸Ç´
                if (playerManager && (int)playerManager.GetDirection() == 0)
                {
                    if (childManager && checkDiff < childManager.diff)
                    {
                        childManager.diff -= diffSize;
                        continue;
                    }
                }
                // âEå¸Ç´
                else if (playerManager && (int)playerManager.GetDirection() == 1)
                {
                    if (childManager && checkDiff > childManager.diff)
                    {
                        childManager.diff += diffSize;
                        continue;
                    }
                }
            }
        }
    }
}