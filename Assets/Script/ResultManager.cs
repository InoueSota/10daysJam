using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;

public class ResultManager : MonoBehaviour
{
    private SceneChanger sceneChanger;

    // 引っ越しの行方テキスト
    [SerializeField] private GameObject movingEndObj;
    private TextMeshProUGUI movingEndText;

    // 子ガモの数のテキスト
    [SerializeField] private TextMeshProUGUI childCountText;
    private NumberChangeManager childCountTextManager;
    public static int childCount;

    // スコアのテキスト
    [SerializeField] private TextMeshProUGUI scoreText;
    private NumberChangeManager scoreTextManager;
    public static int score;
    
    // 子どもたち
    [SerializeField] private GameObject[] childPrefab;

    void Start()
    {
        sceneChanger = GameObject.FindGameObjectWithTag("SceneChanger").GetComponent<SceneChanger>();

        movingEndText = movingEndObj.GetComponent<TextMeshProUGUI>();

        childCountTextManager = childCountText.GetComponent<NumberChangeManager>();
        scoreTextManager = scoreText.GetComponent<NumberChangeManager>();

        for (int i = 0; i < childCount; i++)
        {
            childPrefab[i].SetActive(true);
        }
    }

    void Update()
    {
        if (childCountTextManager) 
        {
            childCountTextManager.SetNumber(childCount);
        }

        if (scoreTextManager) 
        { 
            scoreTextManager.SetNumber(score);
        }

        if (movingEndText)
        {
            if (childCount >= 10)
            {
                movingEndText.text = string.Format("完全引っ越し成功");
                movingEndText.color = Color.yellow;
            }
            else if (childCount > 0)
            {
                movingEndText.text = string.Format("引っ越し成功");
                movingEndText.color = Color.white;
            }
            else
            {
                movingEndText.text = string.Format("引っ越し失敗");
                movingEndText.color = Color.red;
            }
        }

        if (Input.GetAxisRaw("Abutton") != 0 || Input.GetAxisRaw("Start") != 0)
        {
            sceneChanger.ChangeScene("Title");
        }
    }
}
