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

    // ゴール地点のオブジェクト
    [SerializeField] private GameObject goalObj;
    // ゴール地点のX座標の値
    private float goalPositionX;
    // 画面の横幅の半分
    private float halfWidth;
    // スクロール値を格納しているオブジェクト
    [SerializeField] private GameObject scrollManagerObj;
    private ScrollManager scrollManager;
    // 現在位置を上部に描画するUI
    [SerializeField] private GameObject miniDuckUI;
    // スタート地点のUI
    [SerializeField] private GameObject miniStartUI;
    // ゴール地点のUI
    [SerializeField] private GameObject miniGoalUI;

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
            // カウントダウンを行う
            CountDown();
            // 残り時間を描画する
            if (countDownTextManager) { countDownTextManager.SetNumber((int)Mathf.Ceil(countDownTime)); }
        }

        if (gameFlagManager && gameFlagManager.GetClearTutorial() && gameFlagManager.GetIsStart())
        {
            if (IsGoal() || ResultManager.childCount <= 0)
            {
                gameFlagManager.SetFinish();
            }
            UpdateMiniProgress();

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
        float t = scrollManager.GetScrollValue() / (goalPositionX + halfWidth);
        miniDuckUI.transform.position = Vector3.Lerp(miniStartUI.transform.position, miniGoalUI.transform.position, t);
    }

    public void AddScore(int addValue)
    {
        score += addValue;
    }

}
