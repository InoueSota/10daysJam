using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneScript : MonoBehaviour
{
    void Update()
    {
        // �t���X�N���[���ƃE�B���h�E�𕪂���
        if (Input.GetKeyDown(KeyCode.F11))
        {
            if (Screen.fullScreen)
            {
                Screen.fullScreen = false;
            }
            else
            {
                Screen.fullScreen = true;
            }
        }
        //// �V�[�������Z�b�g����
        //if (Input.GetKeyDown(KeyCode.R))
        //{
        //    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //}
        // �E�B���h�E�����
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
