using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
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
    [SerializeField] private TextMeshProUGUI scoreText;
    private NumberChangeManager scoreTextManager;
    // スコアをゲーム内で描画するため
    public GameObject canvas;
    public GameObject scoreIngamePrefab;

    // コンボ
    private int combo;
    // コンボのテキスト
    [SerializeField] private TextMeshProUGUI comboLetterText;
    [SerializeField] private TextMeshProUGUI comboText;
    private NumberChangeManager comboTextManager;
    // コンボのスケーリング
    private float scaleTime;
    private float scaleLeftTime;
    private float startSize;
    private float endSize;
    // 時間経過でコンボを初期化する
    private float comboTime;
    private float comboLeftTime;
    // スライダー
    [SerializeField] private GameObject comboParentObj;
    private Slider comboSlider;

    // プレイヤー
    [SerializeField] private GameObject playerObj;
    private PlayerManager playerManager;

    void Start()
    {
        gameFlagManager = gameFlagManagerObj.GetComponent<GameFlagManager>();

        countDownTime = 3f;
        countDownTextManager = countDownText.GetComponent<NumberChangeManager>();

        goalPositionX = goalObj.transform.position.x;
        halfWidth = Camera.main.ScreenToWorldPoint(new(Screen.width, 0f, 0f)).x;
        scrollManager = scrollManagerObj.GetComponent<ScrollManager>();

        scoreTextManager = scoreText.GetComponent<NumberChangeManager>();

        comboTextManager = comboText.GetComponent<NumberChangeManager>();
        comboTime = 6f;
        comboLeftTime = 0f;

        scaleTime = 0.2f;
        scaleLeftTime = 0f;
        startSize = comboText.fontSize * 1.2f;
        endSize = comboText.fontSize;

        comboSlider = comboParentObj.transform.Find("ComboSlider").GetComponent<Slider>();
        comboSlider.value = 0f;

        playerManager = playerObj.GetComponent<PlayerManager>();
    }

    void Update()
    {
        if (gameFlagManager && gameFlagManager.GetClearTutorial() && !gameFlagManager.GetIsStart())
        {
            // カウントダウンを行う
            CountDown();
            // 残り時間を描画する
            if (countDownTextManager)
            {
                score = 0;
                combo = 0;
                countDownTextManager.SetNumber((int)Mathf.Ceil(countDownTime));
            }
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

            // コンボを描画する
            if (comboTextManager) { comboTextManager.SetNumber(combo); }

            // コンボの時間経過処理
            if (comboLeftTime > 0f)
            {
                comboLeftTime -= Time.deltaTime;
            }
            else
            {
                combo = 0;
            }
            if (comboSlider)
            {
                comboSlider.value = comboLeftTime / comboTime;
            }

            // スケール処理
            if (scaleLeftTime > 0f)
            {
                scaleLeftTime -= Time.deltaTime;
                float t = scaleLeftTime / scaleTime;
                comboText.fontSize = Mathf.Lerp(endSize, startSize, t * t);
            }
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
            comboLetterText.gameObject.SetActive(true);
            comboText.gameObject.SetActive(true);
            comboSlider.gameObject.SetActive(true);
            score = 0;
            combo = 0;
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
        if (playerManager.transform.position.x > goalPositionX)
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
        score += addValue + (int)(addValue * combo * 0.1f);
    }

    public void AddCombo()
    {
        combo++;
        comboLeftTime = comboTime;
        scaleLeftTime = scaleTime;
    }

}
