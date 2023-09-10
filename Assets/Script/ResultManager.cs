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
    // �X�R�A�̃e�L�X�g
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
