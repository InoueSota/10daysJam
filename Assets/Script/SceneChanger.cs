using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class SceneChanger : MonoBehaviour
{
    private Image banner = null;

    [SerializeField] private float changeTime = 5.0f;
    [SerializeField] private float ct = 0.0f;

    [SerializeField] private bool isChanging = true;
    [SerializeField] private string nextScene = "";

    void Awake()
    {
        banner = transform.Find("Canvas/Banner").gameObject.GetComponent<Image>();
        DontDestroyOnLoad(this.gameObject);
    }

    public void init()
    {
        isChanging = false;
        ct = 0.0f;
        banner.fillOrigin = 0;
        banner.fillAmount = 0;
    }

    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {

        if(ct >= changeTime)
        {
            init();
        }

        if (ct > 0.0f)
        {
            isChanging = true;
        }

        if (isChanging == true)
        {
            float halfTime = changeTime * 0.5f;

            float ta = 1;
            if (ct < halfTime)
            {
                float t = ct / halfTime;

                ta = t < 0.5 ? 8 * t * t * t * t : 1 - Mathf.Pow(-2 * t + 2, 4) / 2; ;


            }
            else if (ct <= changeTime)
            {
                banner.fillOrigin = 1;
                float t = (ct - halfTime) / halfTime;

                ta = t < 0.5 ? 8 * t * t * t * t : 1 - Mathf.Pow(-2 * t + 2, 4) / 2; ;
                ta = 1.0f - ta;
            }
            ta = Mathf.Clamp(ta, 0.0f, 1.0f);

            if(ta == 1.0f)
            {
                if(SceneManager.GetActiveScene().name != nextScene)
                {
                    SceneManager.LoadScene(nextScene);
                }
            }

            banner.fillAmount = ta;

            ct += Time.deltaTime;
        }
    }

    public void ChangeScene(string nextSceneName)
    {
        nextScene = nextSceneName;
        isChanging = true;
    }
}
