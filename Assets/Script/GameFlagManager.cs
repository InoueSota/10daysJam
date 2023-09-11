using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFlagManager : MonoBehaviour
{
    // チュートリアルクリアフラグ
    private bool clearTutorial;
    // かかしの親オブジェクト
    [SerializeField] private GameObject dummyParentObj;
    // かかしを倒す指示を書いたオブジェクト
    [SerializeField] private GameObject orderKillDummyObj;

    // ↓ チュートリアルクリア後
    // ゲーム開始するフラグ
    private bool isStart;
    // 子ガモを守り切る指示を書いたオブジェクト
    [SerializeField] private GameObject orderSaveChildrenObj;
    // カラスの発生を制御するオブジェクト
    [SerializeField] private GameObject crowSpawnManagerObj;
    // ネコの発生を制御するオブジェクト
    [SerializeField] private GameObject catSpawnManagerObj;

    // ゲーム終了フラグ
    private bool isFinish;
    // シーンを変えるオブジェクト
    private SceneChanger sceneChanger;

    void Start()
    {
        clearTutorial = false;
        isStart = false;

        isFinish = false;
        sceneChanger = GameObject.FindGameObjectWithTag("SceneChanger").GetComponent<SceneChanger>();
    }

    void Update()
    {
        if (!clearTutorial && dummyParentObj && CountChildObjects(dummyParentObj) == 1)
        {
            Destroy(dummyParentObj);
            Destroy(orderKillDummyObj);
            orderSaveChildrenObj.SetActive(true);
            clearTutorial = true;
        }

        if (isStart && crowSpawnManagerObj && !crowSpawnManagerObj.activeSelf && !catSpawnManagerObj.activeSelf)
        {
            crowSpawnManagerObj.SetActive(true);
            catSpawnManagerObj.SetActive(true);
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
