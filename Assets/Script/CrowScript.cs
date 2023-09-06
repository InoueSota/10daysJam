using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CrowScript : MonoBehaviour
{
    public string targetTag = "Child"; // 検索対象のTag名
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

        // 方向ベクトルを正規化（長さを1にする）
        direction.Normalize();

        // 目標位置の方向に一定速度で移動
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

        // closestChildがnullでなければ、最も近い子オブジェクトが見つかったことになります
        if (closestChild != null)
        {
            LockOn = true;
            targetPos = closestChild.transform.position;
            // ここでclosestChildを使って何かを行うことができます
            // 例: closestChild.GetComponent<YourComponent>().DoSomething();
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
