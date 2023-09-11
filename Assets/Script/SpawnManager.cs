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

    // 画面外から発生させる際、左右の画面外からなのか、上の画面外からなのかをフラグで決める
    [SerializeField] private bool isOutOfWidth = false;

    // 発生間隔
    private float interval;

    // カメラのオブジェクト
    public GameObject cameraObj;
    ScrollManager scrollManager;
    // カメラの横幅の半分
    private float halfWidth;
    // カメラの縦幅の半分
    private float halfHeight;

    void Start()
    {
        LoadEnemyData();
        scrollManager = cameraObj.GetComponent<ScrollManager>();
        halfWidth = Camera.main.ScreenToWorldPoint(new(Screen.width, 0f, 0f)).x;
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
                        Vector3 position = Vector3.zero;
                        // 画面外出現 - 横
                        if (isOutOfWidth)
                        {
                            // 座標をcsvファイルから読み込む
                            position = new(0f, 1.8f, 0f);
                            // X座標の画面外から出現させる
                            position.x += halfWidth * float.Parse(csvDatas[i][0]) + 1.5f + scrollManager.GetScrollValue();
                        }
                        // 画面外出現 - 縦
                        else
                        {
                            // 座標をcsvファイルから読み込む
                            position = new(float.Parse(csvDatas[i][0]) + scrollManager.GetScrollValue(), 0f, 0f);
                            // Y座標を画面外から登場させるために高くする
                            position.y += halfHeight;
                        }
                        // 発生間隔をcsvファイルから読み込む
                        interval = float.Parse(csvDatas[i][1]);

                        GameObject enemy = Instantiate(enemyObj, position, Quaternion.identity);

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
