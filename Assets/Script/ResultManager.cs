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
    // スコアのテキスト
    [SerializeField] private TextMeshProUGUI scoreText;
    private NumberChangeManager scoreTextManager;


    void Start()
    {
        sceneChanger = GameObject.FindGameObjectWithTag("SceneChanger").GetComponent<SceneChanger>();
    }

    void Update()
    {

        if (Input.GetAxisRaw("Abutton") != 0 || Input.GetAxisRaw("Start") != 0)
        {
            sceneChanger.ChangeScene("Title");
        }
    }
}
