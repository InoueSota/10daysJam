using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextChangeNumber : MonoBehaviour
{
    TextMeshProUGUI childCount;
    public GameObject allChildObj;
    AllChildScript allChildScript;

    void Start()
    {
        childCount = GetComponent<TextMeshProUGUI>();
        allChildScript = allChildObj.GetComponent<AllChildScript>();
    }

    void Update()
    {
        if (allChildScript)
        {
            int childrenCount = allChildScript.ChildrenCount();

            // �q�K����10�̈ȏア��Ȃ�
            if (childrenCount >= 10)
            {
                childCount.text = string.Format("{0:00}", allChildScript.ChildrenCount());
            }
            // �q�K����10�̖����Ȃ�
            else
            {
                childCount.text = string.Format("{0:0}", allChildScript.ChildrenCount());
            }
        }
    }
}
