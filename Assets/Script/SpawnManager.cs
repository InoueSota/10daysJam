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

    // ��ʊO���甭��������ہA���E�̉�ʊO����Ȃ̂��A��̉�ʊO����Ȃ̂����t���O�Ō��߂�
    [SerializeField] private bool isOutOfWidth = false;

    // �����Ԋu
    private float interval;

    // �J�����̃I�u�W�F�N�g
    public GameObject cameraObj;
    ScrollManager scrollManager;
    // �J�����̉����̔���
    private float halfWidth;
    // �J�����̏c���̔���
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
                        Vector3 position = Vector3.zero;
                        // ��ʊO�o�� - ��
                        if (isOutOfWidth)
                        {
                            // ���W��csv�t�@�C������ǂݍ���
                            position = new(0f, 1.8f, 0f);
                            // X���W�̉�ʊO����o��������
                            position.x += halfWidth * float.Parse(csvDatas[i][0]) + 1.5f + scrollManager.GetScrollValue();
                        }
                        // ��ʊO�o�� - �c
                        else
                        {
                            // ���W��csv�t�@�C������ǂݍ���
                            position = new(float.Parse(csvDatas[i][0]) + scrollManager.GetScrollValue(), 0f, 0f);
                            // Y���W����ʊO����o�ꂳ���邽�߂ɍ�������
                            position.y += halfHeight;
                        }
                        // �����Ԋu��csv�t�@�C������ǂݍ���
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
