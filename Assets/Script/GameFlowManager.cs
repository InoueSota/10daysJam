using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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
            // カウントダウンを行う
            CountDown();
            // 残り時間を描画する
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
