using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class TitleManagerScript : MonoBehaviour
{

    [SerializeField] private float background0PosY = 2.25f;
    [SerializeField] private float background1PosY = 2.25f;
    [SerializeField] private float background2PosY = 2.25f;
    [SerializeField] private float background3PosY = 2.25f;
    [SerializeField] private float background4PosY = 2.25f;

    [SerializeField] private TitleBackgroundScript Background0 = null;
    [SerializeField] private TitleBackgroundScript Background1 = null;
    [SerializeField] private TitleBackgroundScript Background2 = null;
    [SerializeField] private TitleBackgroundScript Background3 = null;
    [SerializeField] private TitleBackgroundScript Background4 = null;

    [SerializeField] private float multiSpeed = 1.0f;

    [SerializeField] private float BackGround0ScrollSpeed = 2;
    [SerializeField] private float BackGround1ScrollSpeed = 1;
    [SerializeField] private float BackGround2ScrollSpeed = 0.5f;
    [SerializeField] private float BackGround3ScrollSpeed = 0.25f;
    [SerializeField] private float BackGround4ScrollSpeed = 0.25f;

    private SceneChanger changer;

    // Start is called before the first frame update
    void Start()
    {

        changer = GameObject.FindGameObjectWithTag("SceneChanger").GetComponent<SceneChanger>();

        float width = Background0.GetComponent<SpriteRenderer>().bounds.size.x;

        TitleBackgroundScript bg = Instantiate(Background0,new Vector3(0.0f, background0PosY, 0.0f),Quaternion.identity);
        bg.SetSpeed(BackGround0ScrollSpeed);
        bg = Instantiate(Background0, new Vector3(width, background0PosY, 0.0f), Quaternion.identity);
        bg.SetSpeed(BackGround0ScrollSpeed);

        width = Background1.GetComponent<SpriteRenderer>().bounds.size.x;

        bg = Instantiate(Background1, new Vector3(0.0f, background1PosY, 0.0f), Quaternion.identity);
        bg.SetSpeed(BackGround1ScrollSpeed);
        bg = Instantiate(Background1, new Vector3(width, background1PosY, 0.0f), Quaternion.identity);
        bg.SetSpeed(BackGround1ScrollSpeed);

        width = Background2.GetComponent<SpriteRenderer>().bounds.size.x;

        bg = Instantiate(Background2, new Vector3(0.0f, background2PosY, 0.0f), Quaternion.identity);
        bg.SetSpeed(BackGround2ScrollSpeed);
        bg = Instantiate(Background2, new Vector3(width, background2PosY, 0.0f), Quaternion.identity);
        bg.SetSpeed(BackGround2ScrollSpeed);

        width = Background3.GetComponent<SpriteRenderer>().bounds.size.x;

        bg = Instantiate(Background3, new Vector3(0.0f, background3PosY, 0.0f), Quaternion.identity);
        bg.SetSpeed(BackGround3ScrollSpeed);
        bg = Instantiate(Background3, new Vector3(width, background3PosY, 0.0f), Quaternion.identity);
        bg.SetSpeed(BackGround3ScrollSpeed);

        width = Background4.GetComponent<SpriteRenderer>().bounds.size.x;

        bg = Instantiate(Background4, new Vector3(0.0f, background4PosY, 0.0f), Quaternion.identity);
        bg.SetSpeed(BackGround4ScrollSpeed);
        bg = Instantiate(Background4, new Vector3(width, background4PosY, 0.0f), Quaternion.identity);
        bg.SetSpeed(BackGround4ScrollSpeed);

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetAxisRaw("Abutton") != 0 || Input.GetAxisRaw("Start") != 0)
        {

            changer.ChangeScene("GameScene");

        }

        if (Input.GetKeyDown(KeyCode.R))
        {

            var bg = GameObject.FindGameObjectsWithTag("BackGround");

            foreach (var backGround in bg)
            {
                backGround.GetComponent<TitleBackgroundScript>().SetMultiSpeed(multiSpeed);
            }
        }
    }
}
