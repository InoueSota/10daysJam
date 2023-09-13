using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class GrassManager : MonoBehaviour
{
    private bool isEaten;

    void Start()
    {
        isEaten = false;
    }

    void Update()
    {
        
    }

    public bool GetIsEaten()
    {
        return isEaten;
    }

    public void SetIsEaten()
    {
        isEaten = true;
    }
}
