using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllChildScript : MonoBehaviour
{
   public int childCount;
    Transform parentTransform;
    // Start is called before the first frame update
    void Start()
    {
        // �e�I�u�W�F�N�g��Transform�R���|�[�l���g���擾
        parentTransform = transform;

        // �q�I�u�W�F�N�g�̐��𐔂���
        childCount = parentTransform.childCount;


    }

    // Update is called once per frame
    void Update()
    {
        // �e�I�u�W�F�N�g��Transform�R���|�[�l���g���擾
        parentTransform = transform;

        // �q�I�u�W�F�N�g�̐��𐔂���
        childCount = parentTransform.childCount;
        // ���ʂ��R���\�[���ɏo��
        Debug.Log("�q�I�u�W�F�N�g�̐�: " + childCount);

        for(int i = 0; i < childCount; i++)
        {
           
        }
    }
}
