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

    // �S�[���n�_�̃I�u�W�F�N�g
    [SerializeField] private GameObject goalObj;
    // �S�[���n�_��X���W�̒l
    private float goalPositionX;
    // ��ʂ̉����̔���
    private float halfWidth;
    // �X�N���[���l���i�[���Ă���I�u�W�F�N�g
    [SerializeField] private GameObject scrollManagerObj;
    private ScrollManager scrollManager;
    // ���݈ʒu���㕔�ɕ`�悷��UI
    [SerializeField] private GameObject miniDuckUI;
    // �X�^�[�g�n�_��UI
    [SerializeField] private GameObject miniStartUI;
    // �S�[���n�_��UI
    [SerializeField] private GameObject miniGoalUI;

    // �X�R�A
    private int score;
    // �X�R�A�̃e�L�X�g
    [SerializeField] private TextMeshProUGUI scoreLetterText;
    private NumberChangeManager scoreLetterTextManager;
    [SerializeField] private TextMeshProUGUI scoreText;
    private NumberChangeManager scoreTextManager;
    // �X�R�A���Q�[�����ŕ`�悷�邽��
    public GameObject canvas;
    public GameObject scoreIngamePrefab;


    void Start()
    {
        gameFlagManager = gameFlagManagerObj.GetComponent<GameFlagManager>();

        countDownTime = 3f;
        countDownTextManager = countDownText.GetComponent<NumberChangeManager>();

        goalPositionX = goalObj.transform.position.x;
        halfWidth = Camera.main.ScreenToWorldPoint(new(Screen.width, 0f, 0f)).x;
        scrollManager = scrollManagerObj.GetComponent<ScrollManager>();

        scoreLetterTextManager = scoreLetterText.GetComponent<NumberChangeManager>();
        scoreTextManager = scoreText.GetComponent<NumberChangeManager>();
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
            if (IsGoal() || ResultManager.childCount <= 0)
            {
                gameFlagManager.SetFinish();
            }
            UpdateMiniProgress();

            // �X�R�A��`�悷��
            if (scoreTextManager) { scoreTextManager.SetNumber(score); }
            ResultManager.score = score;
        }
    }

    private void CountDown()
    {
        countDownTime -= Time.deltaTime;
        if (countDownTime < 0f) 
        { 
            countDownText.gameObject.SetActive(false);
            scoreLetterText.gameObject.SetActive(true);
            scoreText.gameObject.SetActive(true);
            score = 0;
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ScrollManager>().SetAutoScrollStart();
            gameFlagManager.SetStart(); 
        }
        else
        {
            countDownText.gameObject.SetActive(true);
        }
    }

    private bool IsGoal()
    {
        float cameraLeft = scrollManager.GetScrollValue() - halfWidth;
        if (cameraLeft > goalPositionX)
        {
            return true;
        }
        return false;
    }

    private void UpdateMiniProgress()
    {
        float t = scrollManager.GetScrollValue() / (goalPositionX - halfWidth);
        miniDuckUI.transform.position = Vector3.Lerp(miniStartUI.transform.position, miniGoalUI.transform.position, t);
    }

    public void AddScore(int addValue)
    {
        score += addValue;
    }

}