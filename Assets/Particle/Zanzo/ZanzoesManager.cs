using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZanzoesManager : MonoBehaviour
{
    // Start is called before the first frame update

    private GameObject Zanzoes;
    [SerializeField] private ZanzoScript zanzo;
    private Sprite sprite;
    private SpriteRenderer spriteRenderer;


    private bool isRunning = false;
    [SerializeField] private float destroyTime  = 0.2f;

    void Start()
    {
        Zanzoes = GameObject.FindGameObjectWithTag("Zanzoes");
        spriteRenderer = this.GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isRunning)
        {
            sprite = GetComponent<SpriteRenderer>().sprite;
            ZanzoScript z = Instantiate(zanzo, this.transform.position, this.transform.rotation);
            z.SetSprite(sprite,destroyTime, spriteRenderer.flipX);
            z.transform.parent = Zanzoes.transform;
        }
    }

    public void SetRunning(bool run)
    {
        isRunning = run;
    }
}
