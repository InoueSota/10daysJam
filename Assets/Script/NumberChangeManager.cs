using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class NumberChangeManager : MonoBehaviour
{
    TextMeshProUGUI numberText;
    private int number;

    void Start()
    {
        numberText = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (numberText)
        {
            if (number > 1000000)
            {
                numberText.text = string.Format("{0:0000000}", number);
            }
            else if (number > 100000)
            {
                numberText.text = string.Format("{0:000000}", number);
            }
            else if (number > 10000)
            {
                numberText.text = string.Format("{0:00000}", number);
            }
            else if (number > 1000)
            {
                numberText.text = string.Format("{0:0000}", number);
            }
            else if (number > 100)
            {
                numberText.text = string.Format("{0:000}", number);
            }
            else if (number > 10)
            {
                numberText.text = string.Format("{0:00}", number);
            }
            else
            {
                numberText.text = string.Format("{0:0}", number);
            }
        }
    }

    public void SetNumber(int number_)
    {
        number = number_;
    }
}
