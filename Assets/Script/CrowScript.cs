using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CrowScript : MonoBehaviour
{
    public string targetTag = "Child"; // �����Ώۂ�Tag��
    public Vector3 targetPos;
    public float moveSpeed = 1.0f;

    private Transform closestChild = null;
    public bool LockOn;
    // public bool takeAway;
    public enum Mode
    {
        stay,
        attak,
        leave,
        takeaway
    };
    [SerializeField] private Mode mode;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        switch (mode)
        {
            case Mode.stay:

                FindClosestChild();

                break;
            case Mode.attak:


                Attak();


                break;
            case Mode.leave:



                break;
            case Mode.takeaway:
                transform.position += new Vector3(0, moveSpeed, 0)*Time.deltaTime;
                closestChild.transform.position = transform.position;
                break;



        }

    }

    private void Attak()
    {
        Vector3 direction = targetPos - transform.position;

        // �����x�N�g���𐳋K���i������1�ɂ���j
        direction.Normalize();

        // �ڕW�ʒu�̕����Ɉ�葬�x�ňړ�
        transform.position += direction * moveSpeed * Time.deltaTime;


    }

    void FindClosestChild()
    {
        GameObject[] children = GameObject.FindGameObjectsWithTag(targetTag);

        float closestDistance = float.MaxValue;

        foreach (GameObject child in children)
        {
            float distanceToChild = Vector3.Distance(transform.position, child.transform.position);

            if (distanceToChild < closestDistance)
            {
                closestDistance = distanceToChild;
                closestChild = child.transform;
            }
        }

        // closestChild��null�łȂ���΁A�ł��߂��q�I�u�W�F�N�g�������������ƂɂȂ�܂�
        if (closestChild != null)
        {
            LockOn = true;
            targetPos = closestChild.transform.position;
            // ������closestChild���g���ĉ������s�����Ƃ��ł��܂�
            // ��: closestChild.GetComponent<YourComponent>().DoSomething();
        }
        else
        {
            LockOn = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(targetTag))
        {
            mode = Mode.takeaway;
            //closestChild.transform.parent = transform;
        }
    }
}
