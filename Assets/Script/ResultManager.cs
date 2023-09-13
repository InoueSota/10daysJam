using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResultManager : MonoBehaviour
{
    private SceneChanger sceneChanger;

    // �q�K���̐��̃e�L�X�g
    [SerializeField] private TextMeshProUGUI childCountText;
    private NumberChangeManager childCountTextManager;
    public static int childCount;

    // �X�R�A�̃e�L�X�g
    [SerializeField] private TextMeshProUGUI scoreText;
    private NumberChangeManager scoreTextManager;
    public static int score;
    private bool isScoreDisplay;
    
    // �q�ǂ�����
    [SerializeField] private GameObject[] childPrefab;

    void Start()
    {
        sceneChanger = GameObject.FindGameObjectWithTag("SceneChanger").GetComponent<SceneChanger>();

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

        if (Input.GetAxisRaw("Abutton") != 0 || Input.GetAxisRaw("Start") != 0)
        {
            sceneChanger.ChangeScene("Title");
        }
    }
}
