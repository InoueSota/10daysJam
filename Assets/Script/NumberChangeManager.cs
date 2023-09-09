using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NumberChangeManager : MonoBehaviour
{
    TextMeshProUGUI numberText;
    [SerializeField]private int number;

    void Start()
    {
        numberText = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (numberText)
        {
            numberText.text = string.Format("{0:0}", number);
        }
    }

    public void SetNumber(int number_)
    {
        number = number_;
    }
}
