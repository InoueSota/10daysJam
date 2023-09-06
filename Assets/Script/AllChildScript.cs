using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllChildScript : MonoBehaviour
{
   public int childCount;
    Transform parentTransform;
    public int PineNum;
    // Start is called before the first frame update
    void Start()
    {
        //// 親オブジェクトのTransformコンポーネントを取得
        //parentTransform = transform;
        //// 子オブジェクトの数を数える
        //childCount = parentTransform.childCount;
        // 最初に一度だけ子オブジェクトを数える
        CountPineNum();

    }

    // Update is called once per frame
    void Update()
    {
        CountPineNum();
    }

    void CountPineNum()
    {
        PineNum = 0;

        // 子オブジェクトをループして条件をチェック
        foreach (Transform child in transform)
        {
            ChildManager childManager = child.GetComponent<ChildManager>();
            if (childManager != null && childManager.isPileUpped)
            {
                PineNum++;
            }
        }

        // 結果をコンソールに出力
        Debug.Log("PineNum: " + PineNum);
    }






}
