using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GameFlagManager : MonoBehaviour
{
    // �`���[�g���A���̊Ǘ�������I�u�W�F�N�g
    [SerializeField] private GameObject tutorialManagerObj;
    private TutorialManager tutorialManager;
    // �`���[�g���A���N���A�t���O
    private bool clearTutorial;
    // �������̐e�I�u�W�F�N�g
    [SerializeField] private GameObject dummyParentObj;

    // �� �`���[�g���A���N���A��
    // �Q�[���J�n����t���O
    private bool isStart;
    // �q�K�������؂�w�����������I�u�W�F�N�g
    [SerializeField] private GameObject orderSaveChildrenObj;
    // �X�^�[�g�n�_����S�[���n�_�܂ł̃I�u�W�F�N�g
    [SerializeField] private GameObject miniProgressObj;

    // �Q�[���I���t���O
    private bool isFinish;
    // �V�[����ς���I�u�W�F�N�g
    private SceneChanger sceneChanger;

    void Start()
    {
        tutorialManager = tutorialManagerObj.GetComponent<TutorialManager>();
        clearTutorial = false;
        isStart = false;

        isFinish = false;
        sceneChanger = GameObject.FindGameObjectWithTag("SceneChanger").GetComponent<SceneChanger>();
    }

    void Update()
    {
        if (!clearTutorial && dummyParentObj && CountChildObjects(dummyParentObj) == 1 && tutorialManager)
        {
            tutorialManager.SetClearAttack();
            Destroy(dummyParentObj);
        }
        if (tutorialManager && tutorialManager.GetIsClearTutorial())
        {
            orderSaveChildrenObj.SetActive(true);
            miniProgressObj.SetActive(true);
            clearTutorial = true;
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
