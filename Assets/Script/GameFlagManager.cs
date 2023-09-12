using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GameFlagManager : MonoBehaviour
{
    // チュートリアルの管理をするオブジェクト
    [SerializeField] private GameObject tutorialManagerObj;
    private TutorialManager tutorialManager;
    // チュートリアルクリアフラグ
    private bool clearTutorial;
    // かかしの親オブジェクト
    [SerializeField] private GameObject dummyParentObj;

    // ↓ チュートリアルクリア後
    // ゲーム開始するフラグ
    private bool isStart;
    // 子ガモを守り切る指示を書いたオブジェクト
    [SerializeField] private GameObject orderSaveChildrenObj;
    // スタート地点からゴール地点までのオブジェクト
    [SerializeField] private GameObject miniProgressObj;

    // ゲーム終了フラグ
    private bool isFinish;
    // シーンを変えるオブジェクト
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
