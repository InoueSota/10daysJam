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

    // シーンを変えるオブジェクト
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
