using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResultManager : MonoBehaviour
{
    private SceneChanger sceneChanger;

    // 子ガモの数のテキスト
    [SerializeField] private TextMeshProUGUI childCountText;
    private NumberChangeManager childCountTextManager;
    public static int childCount;
    // スコアのテキスト
    [SerializeField] private TextMeshProUGUI scoreText;
    private NumberChangeManager scoreTextManager;
    public static int score;

    void Start()
    {
        sceneChanger = GameObject.FindGameObjectWithTag("SceneChanger").GetComponent<SceneChanger>();

        childCountTextManager = childCountText.GetComponent<NumberChangeManager>();
        scoreTextManager = scoreText.GetComponent<NumberChangeManager>();
    }

    void Update()
    {
        if (childCountTextManager) { childCountTextManager.SetNumber(childCount); }
        if (scoreTextManager) { scoreTextManager.SetNumber(score); }

        if (Input.GetAxisRaw("Abutton") != 0 || Input.GetAxisRaw("Start") != 0)
        {
            sceneChanger.ChangeScene("Title");
        }
    }
}
