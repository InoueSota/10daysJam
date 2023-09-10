using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameFlowManager : MonoBehaviour
{
    // �Q�[���̃t���O�֌W���܂Ƃ߂Ă���I�u�W�F�N�g
    [SerializeField] private GameObject gameFlagManagerObj;
    private GameFlagManager gameFlagManager;

    // �J�n�܂ł̃J�E���g�_�E��
    private float countDownTime;
    // �J�E���g�_�E�����s���e�L�X�g
    [SerializeField] private TextMeshProUGUI countDownText;
    private NumberChangeManager countDownTextManager;

    // �Q�[�����̐�������
    private float timeLimit;
    // �������Ԃ��v������e�L�X�g
    [SerializeField] private TextMeshProUGUI timeLimitText;
    private NumberChangeManager timeLimitTextManager;

    // �V�[����ς���I�u�W�F�N�g
    private SceneChanger sceneChanger;

    void Start()
    {
        gameFlagManager = gameFlagManagerObj.GetComponent<GameFlagManager>();

        countDownTime = 3f;
        countDownTextManager = countDownText.GetComponent<NumberChangeManager>();

        timeLimit = 60f;
        timeLimitTextManager = timeLimitText.GetComponent<NumberChangeManager>();

        sceneChanger = GameObject.FindGameObjectWithTag("SceneChanger").GetComponent<SceneChanger>();
    }

    void Update()
    {
        if (gameFlagManager && gameFlagManager.GetClearTutorial() && !gameFlagManager.GetIsStart())
        {
            // �J�E���g�_�E�����s��
            CountDown();
            // �c�莞�Ԃ�`�悷��
            if (countDownTextManager) { countDownTextManager.SetNumber((int)Mathf.Ceil(countDownTime)); }
        }

        if (gameFlagManager && gameFlagManager.GetClearTutorial() && gameFlagManager.GetIsStart())
        {
            // �������Ԃ��v������
            TimeLimit();
            // �������Ԃ�`�悷��
            if (timeLimitTextManager) { timeLimitTextManager.SetNumber((int)Mathf.Ceil(timeLimit)); }
        }
    }

    private void CountDown()
    {
        countDownTime -= Time.deltaTime;
        if (countDownTime < 0f) 
        { 
            countDownText.gameObject.SetActive(false);
            timeLimitText.gameObject.SetActive(true);
            gameFlagManager.SetStart(); 
        }
        else
        {
            countDownText.gameObject.SetActive(true);
        }
    }

    private void TimeLimit()
    {
        timeLimit -= Time.deltaTime;
        if (timeLimit < 0f)
        {
            sceneChanger.ChangeScene("Result");
        }
    }
}
