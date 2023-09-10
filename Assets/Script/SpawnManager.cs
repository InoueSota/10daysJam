using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    // CSV�t�@�C����ǂݍ��ݓG�𔭐�������
    TextAsset csvFile;
    List<string[]> csvDatas = new List<string[]>();
    List<int> ints = new List<int>();

    // ����������Ώۂ̓G�̖��O
    [SerializeField] private string spawnObj;

    // �Ώۂ̓G�̃v���n�u
    [SerializeField] private GameObject enemyObj;
    // �Q�[�����̑Ώۂ̓G�𐔂���
    private GameObject[] enemyObjs;

    // �����Ԋu
    private float interval;

    // �J�����̃I�u�W�F�N�g
    public GameObject cameraObj;
    // �J�����̏c���̔���
    private float halfHeight;

    void Start()
    {
        LoadEnemyData();
        halfHeight = Camera.main.ScreenToWorldPoint(new(0f, Screen.height, 0f)).y;
    }

    void Update()
    {
        // �Q�[�����̑Ώۂ̓G�𐔂���
        enemyObjs = GameObject.FindGameObjectsWithTag(spawnObj);

        if (interval <= 0f)
        {
            // �Ώۂ̓G���Q�[�����ɏo�����Ȃ��悤�ɂ���
            if (enemyObjs.Length <= 1)
            {
                // ������csv�t�@�C���̒��g��S�ď������ēx�g����悤�Ƀ��T�C�N������
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
                        // ���W��csv�t�@�C������ǂݍ���
                        Vector3 position = new(float.Parse(csvDatas[i][0]), 0f, 0f);
                        // Y���W����ʊO����o�ꂳ���邽�߂ɍ�������
                        position.y += halfHeight;
                        // �����Ԋu��csv�t�@�C������ǂݍ���
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
            // �����Ԋu���Ԃ����炷
            interval -= Time.deltaTime;

            // �Ώۂ̓G���P�̂����Ȃ������甭���Ԋu�𖳂���
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
