using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZanzoesManager : MonoBehaviour
{
    // Start is called before the first frame update

    private GameObject Particles;
    [SerializeField] private ZanzoScript zanzo;
    private Vector3 scale;
    private Sprite sprite;
    private SpriteRenderer spriteRenderer;


    private bool isRunning = false;
    [SerializeField] private float destroyTime  = 0.2f;

    void Start()
    {
        Particles = GameObject.FindGameObjectWithTag("Particles");
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        scale = this.transform.localScale;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isRunning)
        {
            sprite = GetComponent<SpriteRenderer>().sprite;
            ZanzoScript z = Instantiate(zanzo, this.transform.position, this.transform.rotation);
            z.SetSprite(sprite,destroyTime, spriteRenderer.flipX, scale);
            z.transform.parent = Particles.transform;
        }
    }

    public void SetRunning(bool run)
    {
        isRunning = run;
    }
}
