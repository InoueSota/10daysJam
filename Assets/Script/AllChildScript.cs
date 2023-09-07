using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllChildScript : MonoBehaviour
{
    public int stackCount;

    void Start()
    {
        // �ŏ��Ɉ�x�����q�I�u�W�F�N�g�𐔂���
        CountStackCount();
    }

    void Update()
    {
        CountStackCount();
    }

    void CountStackCount()
    {
        stackCount = 0;

        // �q�I�u�W�F�N�g�����[�v���ď������`�F�b�N
        foreach (Transform child in transform)
        {
            ChildManager childManager = child.GetComponent<ChildManager>();
            if (childManager != null && childManager.isPiledUp)
            {
                stackCount++;
            }
        }

        // ���ʂ��R���\�[���ɏo��
        Debug.Log("stackCount: " + stackCount);
    }

    public void AddChildObjects(GameObject[] children)
    {
        // �q�𐔂���
        int childCount = 0;

        foreach (Transform child in transform)
        {
            children[childCount] = child.gameObject;
            childCount++;
        }
    }
}