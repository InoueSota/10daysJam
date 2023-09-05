using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllChildScript : MonoBehaviour
{
   public int childCount;
    Transform parentTransform;
    // Start is called before the first frame update
    void Start()
    {
        // 親オブジェクトのTransformコンポーネントを取得
        parentTransform = transform;

        // 子オブジェクトの数を数える
        childCount = parentTransform.childCount;


    }

    // Update is called once per frame
    void Update()
    {
        // 親オブジェクトのTransformコンポーネントを取得
        parentTransform = transform;

        // 子オブジェクトの数を数える
        childCount = parentTransform.childCount;
        // 結果をコンソールに出力
        Debug.Log("子オブジェクトの数: " + childCount);

        for(int i = 0; i < childCount; i++)
        {
           
        }
    }
}
