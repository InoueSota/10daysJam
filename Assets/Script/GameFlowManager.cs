using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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

    void Start()
    {
        gameFlagManager = gameFlagManagerObj.GetComponent<GameFlagManager>();

        countDownTime = 3f;
        countDownTextManager = countDownText.GetComponent<NumberChangeManager>();
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
    }

    private void CountDown()
    {
        countDownTime -= Time.deltaTime;
        if (countDownTime < 0f) 
        { 
            countDownText.gameObject.SetActive(false);
            gameFlagManager.SetStart(); 
        }
        else
        {
            countDownText.gameObject.SetActive(true);
        }
    }
}
