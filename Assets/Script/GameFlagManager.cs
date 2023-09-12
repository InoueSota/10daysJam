using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFlagManager : MonoBehaviour
{
    // �`���[�g���A���N���A�t���O
    private bool clearTutorial;
    // �������̐e�I�u�W�F�N�g
    [SerializeField] private GameObject dummyParentObj;
    // ��������|���w�����������I�u�W�F�N�g
    [SerializeField] private GameObject orderKillDummyObj;

    // �� �`���[�g���A���N���A��
    // �Q�[���J�n����t���O
    private bool isStart;
    // �q�K�������؂�w�����������I�u�W�F�N�g
    [SerializeField] private GameObject orderSaveChildrenObj;
    // �J���X�̔����𐧌䂷��I�u�W�F�N�g
    [SerializeField] private GameObject crowSpawnManagerObj;
    // �l�R�̔����𐧌䂷��I�u�W�F�N�g
    [SerializeField] private GameObject catSpawnManagerObj;

    // �Q�[���I���t���O
    private bool isFinish;
    // �V�[����ς���I�u�W�F�N�g
    private SceneChanger sceneChanger;

    void Start()
    {
        clearTutorial = false;
        isStart = false;

        isFinish = false;
        sceneChanger = GameObject.FindGameObjectWithTag("SceneChanger").GetComponent<SceneChanger>();
    }
        
    void Update()
    {
        if (!clearTutorial && dummyParentObj && CountChildObjects(dummyParentObj) == 1)
        {
            Destroy(dummyParentObj);
            Destroy(orderKillDummyObj);
            orderSaveChildrenObj.SetActive(true);
            clearTutorial = true;
        }

        if (isStart && crowSpawnManagerObj && !crowSpawnManagerObj.activeSelf && !catSpawnManagerObj.activeSelf)
        {
            crowSpawnManagerObj.SetActive(true);
            catSpawnManagerObj.SetActive(true);
        }

        if (isFinish)
        {
            sceneChanger.ChangeScene("Result");
        }
    }

    public bool GetClearTutorial()
    {
        return clearTutorial;
    }

    public void SetStart()
    {
        isStart = true;
    }

    public bool GetIsStart()
    {
        return isStart;
    }

    public void SetFinish()
    {
        isFinish = true;
    }

    private int CountChildObjects(GameObject parentObj)
    {
        int count = 0;
        foreach (Transform child in parentObj.transform)
        {
            count++;
        }
        return count;
    }
}