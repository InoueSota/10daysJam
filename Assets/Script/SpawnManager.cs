using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    // CSVファイルを読み込み敵を発生させる
    TextAsset csvFile;
    List<string[]> csvDatas = new List<string[]>();
    List<int> ints = new List<int>();

    // 発生させる対象の敵の名前
    [SerializeField] private string spawnObj;

    // 対象の敵のプレハブ
    [SerializeField] private GameObject enemyObj;
    // ゲーム内の対象の敵を数える
    private GameObject[] enemyObjs;

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
        // ゲーム内の対象の敵を数える
        enemyObjs = GameObject.FindGameObjectsWithTag(spawnObj);

        if (interval <= 0f)
        {
            // 対象の敵がゲーム内に出すぎないようにする
            if (enemyObjs.Length <= 1)
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

                        GameObject crow = Instantiate(enemyObj, position, Quaternion.identity);

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

            // 対象の敵が１体もいなかったら発生間隔を無くす
            if (enemyObjs.Length == 0)
            {
                interval = 0f;
            }
        }
    }

    void LoadEnemyData()
    {
        csvFile = Resources.Load(spawnObj) as TextAsset;
        StringReader reader = new StringReader(csvFile.text);

        while (reader.Peek() != -1)
        {
            string line = reader.ReadLine();
            csvDatas.Add(line.Split(','));
            ints.Add(1);
        }
    }
}
