using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZanzoesManager : MonoBehaviour
{
    // Start is called before the first frame update

    private GameObject Zanzoes;
    [SerializeField] private ZanzoScript zanzo;
    private Sprite sprite;

    void Start()
    {
        Zanzoes = GameObject.FindGameObjectWithTag("Zanzoes");

        //sprite = GetComponent<SpriteRenderer>().sprite;
        //ZanzoScript z = Instantiate(zanzo, this.transform.position, this.transform.rotation);
        //z.SetSprite(sprite);
        //z.transform.parent = Zanzoes.transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
