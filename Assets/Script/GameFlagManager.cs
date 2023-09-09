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
    [SerializeField] private GameObject crowSpawnManagerObj;

    void Start()
    {
        clearTutorial = false;
        isStart = false;
    }

    void Update()
    {
        if (!clearTutorial && dummyParentObj && CountChildObjects(dummyParentObj) == 1)
        {
            Destroy(dummyParentObj);
            Destroy(orderKillDummyObj);
            clearTutorial = true;
        }

        if (isStart && crowSpawnManagerObj && !crowSpawnManagerObj.activeSelf)
        {
            crowSpawnManagerObj.SetActive(true);
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
