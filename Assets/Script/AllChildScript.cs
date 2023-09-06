using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllChildScript : MonoBehaviour
{
   public int childCount;
    Transform parentTransform;
    public int PineNum;
    // Start is called before the first frame update
    void Start()
    {
        //// �e�I�u�W�F�N�g��Transform�R���|�[�l���g���擾
        //parentTransform = transform;
        //// �q�I�u�W�F�N�g�̐��𐔂���
        //childCount = parentTransform.childCount;
        // �ŏ��Ɉ�x�����q�I�u�W�F�N�g�𐔂���
        CountPineNum();

    }

    // Update is called once per frame
    void Update()
    {
        CountPineNum();
    }

    void CountPineNum()
    {
        PineNum = 0;

        // �q�I�u�W�F�N�g�����[�v���ď������`�F�b�N
        foreach (Transform child in transform)
        {
            ChildManager childManager = child.GetComponent<ChildManager>();
            if (childManager != null && childManager.isPileUpped)
            {
                PineNum++;
            }
        }

        // ���ʂ��R���\�[���ɏo��
        Debug.Log("PineNum: " + PineNum);
    }






}
