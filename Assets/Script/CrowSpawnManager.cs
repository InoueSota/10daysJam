using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CrowSpawnManager : MonoBehaviour
{
    // CSVファイルを読み込み敵を発生させる
    TextAsset csvFile;
    List<string[]> csvDatas = new List<string[]>();
    List<int> ints = new List<int>();

    // カラスのプレハブ
    [SerializeField] private GameObject crowObj;
    // ゲーム内のカラスを数える
    private GameObject[] crowObjs;

    // 発生間隔
    private float interval;

    // カメラのオブジェクト
    public GameObject cameraObj;
    // カメラの縦幅の半分
    private float halfHeight;

    void Start()
    {
        LoadEnemyData();
        halfHeight = Camera.main.ScreenToWorldPoint(new(0f, Screen.height, 0f)).y;
    }

    void Update()
    {
        // ゲーム内のカラスを数える
        crowObjs = GameObject.FindGameObjectsWithTag("Crow");

        if (interval <= 0f)
        {
            // カラスがゲーム内に出すぎないようにする
            if (crowObjs.Length <= 1)
            {
                // もしもcsvファイルの中身を全て消費したら再度使えるようにリサイクルする
                bool useUp = true;
                for (int i = 0; i < csvDatas.Count; i++)
                {
                    if (ints[i] == 1)
                    {
                        useUp = false;
                        break;
                    }
                }
                if (useUp)
                {
                    for (int i = 0; i < csvDatas.Count; i++)
                    {
                        ints[i] = 1;
                    }
                }

                for (int i = 0; i < csvDatas.Count; i++)
                {
                    if (ints[i] == 1)
                    {
                        // 座標をcsvファイルから読み込む
                        Vector3 position = new(float.Parse(csvDatas[i][0]), 0f, 0f);
                        // Y座標を画面外から登場させるために高くする
                        position.y += halfHeight;
                        // 発生間隔をcsvファイルから読み込む
                        interval = float.Parse(csvDatas[i][1]);

                        GameObject crow = Instantiate(crowObj, position, Quaternion.identity);

                        ints[i] = 0;
                        break;
                    }
                }
            }
        }
        else
        {
            // 発生間隔時間を減らす
            interval -= Time.deltaTime;

            // カラスが一羽もいなかったら発生間隔を無くす
            if (crowObjs.Length == 0)
            {
                interval = 0f;
            }
        }
    }

    void LoadEnemyData()
    {
        csvFile = Resources.Load("Crow") as TextAsset;
        StringReader reader = new StringReader(csvFile.text);

        while (reader.Peek() != -1)
        {
            string line = reader.ReadLine();
            csvDatas.Add(line.Split(','));
            ints.Add(1);
        }
    }
}
