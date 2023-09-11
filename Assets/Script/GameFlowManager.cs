using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameFlowManager : MonoBehaviour
{
    // ゲームのフラグ関係をまとめているオブジェクト
    [SerializeField] private GameObject gameFlagManagerObj;
    private GameFlagManager gameFlagManager;

    // 開始までのカウントダウン
    private float countDownTime;
    // カウントダウンを行うテキスト
    [SerializeField] private TextMeshProUGUI countDownText;
    private NumberChangeManager countDownTextManager;

    // ゲーム内の制限時間
    private float timeLimit;
    // 制限時間を計測するテキスト
    [SerializeField] private TextMeshProUGUI timeLimitText;
    private NumberChangeManager timeLimitTextManager;

    // スコア
    private int score;
    // スコアのテキスト
    [SerializeField] private TextMeshProUGUI scoreLetterText;
    private NumberChangeManager scoreLetterTextManager;
    [SerializeField] private TextMeshProUGUI scoreText;
    private NumberChangeManager scoreTextManager;
    // スコアをゲーム内で描画するため
    public GameObject canvas;
    public GameObject scoreIngamePrefab;

    // シーンを変えるオブジェクト
    private SceneChanger sceneChanger;

    void Start()
    {
        gameFlagManager = gameFlagManagerObj.GetComponent<GameFlagManager>();

        countDownTime = 3f;
        countDownTextManager = countDownText.GetComponent<NumberChangeManager>();

        timeLimit = 60f;
        timeLimitTextManager = timeLimitText.GetComponent<NumberChangeManager>();

        scoreLetterTextManager = scoreLetterText.GetComponent<NumberChangeManager>();
        scoreTextManager = scoreText.GetComponent<NumberChangeManager>();

        sceneChanger = GameObject.FindGameObjectWithTag("SceneChanger").GetComponent<SceneChanger>();
    }

    void Update()
    {
        if (gameFlagManager && gameFlagManager.GetClearTutorial() && !gameFlagManager.GetIsStart())
        {
            // カウントダウンを行う
            CountDown();
            // 残り時間を描画する
            if (countDownTextManager) { countDownTextManager.SetNumber((int)Mathf.Ceil(countDownTime)); }
        }

        if (gameFlagManager && gameFlagManager.GetClearTutorial() && gameFlagManager.GetIsStart())
        {
            // 制限時間を計測する
            TimeLimit();
            // 制限時間を描画する
            if (timeLimitTextManager) { timeLimitTextManager.SetNumber((int)Mathf.Ceil(timeLimit)); }

            // スコアを描画する
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
            timeLimitText.gameObject.SetActive(true);
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

    private void TimeLimit()
    {
        timeLimit -= Time.deltaTime;
        if (timeLimit < 0f)
        {
            sceneChanger.ChangeScene("Result");
        }
    }

    public void AddScore(int addValue)
    {
        score += addValue;
    }
}
