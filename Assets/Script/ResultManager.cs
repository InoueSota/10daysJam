using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;

public class ResultManager : MonoBehaviour
{
    private SceneChanger sceneChanger;

    // �����z���̍s���e�L�X�g
    [SerializeField] private GameObject movingEndObj;
    private TextMeshProUGUI movingEndText;

    // �q�K���̐��̃e�L�X�g
    [SerializeField] private TextMeshProUGUI childCountText;
    private NumberChangeManager childCountTextManager;
    public static int childCount;

    // �X�R�A�̃e�L�X�g
    [SerializeField] private TextMeshProUGUI scoreText;
    private NumberChangeManager scoreTextManager;
    public static int score;
    
    // �q�ǂ�����
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
                movingEndText.text = string.Format("���S�����z������");
                movingEndText.color = Color.yellow;
            }
            else if (childCount > 0)
            {
                movingEndText.text = string.Format("�����z������");
                movingEndText.color = Color.white;
            }
            else
            {
                movingEndText.text = string.Format("�����z�����s");
                movingEndText.color = Color.red;
            }
        }

        if (Input.GetAxisRaw("Abutton") != 0 || Input.GetAxisRaw("Start") != 0)
        {
            sceneChanger.ChangeScene("Title");
        }
    }
}
