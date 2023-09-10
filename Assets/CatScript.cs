using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class CatScript : MonoBehaviour
{
    enum Mode
    {
        Scan,
        Chase,
        Hikkaku,
        Kuwaeru,

    }
    [SerializeField] Mode mode = Mode.Scan;
    private float distance;
    [SerializeField] Vector2 childPos;
    [SerializeField] GameObject target;
    public string TargetTag = "Child";
    SpriteRenderer spriteRenderer;
    [SerializeField] Vector3 easePos;
    bool isEase;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (mode)
        {
            case Mode.Scan:
                // ゲームオブジェクトの位置を中心に、半径5の範囲内のオブジェクトを探す
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 10f);

                foreach (Collider2D col in colliders)
                {
                    Debug.Log("Detected Object: " + col.gameObject.name);
                    if (col.gameObject.tag == "Child")
                    {
                        // 範囲内のオブジェクトへの参照を取得
                        target = col.gameObject;
                        mode = Mode.Chase;
                        // ここでtargetObjectを操作できます
                    }
                }

                break;

            case Mode.Chase:
                if (target != null) // targetがnullでないことを確認
                {
                    Vector2 direction = target.transform.position - transform.position;
                    direction.Normalize();
                    transform.position += new Vector3(direction.x * 5, 0, 0) * Time.deltaTime;
                    if (direction.x > 0)
                    {
                        spriteRenderer.flipX = true;
                    }
                    else
                    {
                        spriteRenderer.flipX = false;
                    }
                    float distance = Vector2.Distance(transform.position, target.transform.position);
                    if (distance <= 7.0f && !isEase)
                    {
                        isEase = true;
                        easePos = target.transform.position;
                        transform.DOMoveX(easePos.x, 1.0f).SetEase(Ease.InBack).OnComplete(EaseEndFanction);
                    }
                }
                break;

            case Mode.Hikkaku:
                isEase = false;
                mode = Mode.Scan;
                target = null;
                break;

            case Mode.Kuwaeru:

                break;



        }

    }
    void EaseEndFanction()
    {
        mode = Mode.Hikkaku;
    }
}
