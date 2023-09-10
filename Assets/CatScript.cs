using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class CatScript : MonoBehaviour
{
    public float pushForce = 10.0f; // ������΂��͂̑傫��
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
    public string TargetTag = "Player";
    SpriteRenderer spriteRenderer;
    [SerializeField] Vector3 easePos;
    bool isEase;
    public Vector2 direction_;
    GameObject player;
    bool isAttack;
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
                // �Q�[���I�u�W�F�N�g�̈ʒu�𒆐S�ɁA���a5�͈͓̔��̃I�u�W�F�N�g��T��
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 10f);

                foreach (Collider2D col in colliders)
                {
                    Debug.Log("Detected Object: " + col.gameObject.name);
                    if (col.gameObject.tag == TargetTag)
                    {
                        // �͈͓��̃I�u�W�F�N�g�ւ̎Q�Ƃ��擾
                        target = col.gameObject;
                        mode = Mode.Chase;
                        // ������targetObject�𑀍�ł��܂�
                    }
                }

                break;

            case Mode.Chase:
                if (target != null) // target��null�łȂ����Ƃ��m�F
                {
                    direction_ = target.transform.position - transform.position;
                    direction_.Normalize();
                    transform.position += new Vector3(direction_.x * 5, 0, 0) * Time.deltaTime;
                    if (direction_.x > 0)
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
    private void FixedUpdate()
    {
        if (isAttack)
        {
            Vector2 forceDirection = direction_.normalized;
            target.GetComponent<Rigidbody2D>().AddForce(forceDirection * 10,ForceMode2D.Impulse);
        }
    }
    void EaseEndFanction()
    {
        mode = Mode.Hikkaku;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) // �v���C���[�I�u�W�F�N�g�Ƃ̏Փ˂����o
        {
            // Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            isAttack = true;


        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) // �v���C���[�I�u�W�F�N�g�Ƃ̏Փ˂����o
        {
            // Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            isAttack = false;
        }
    }
}
