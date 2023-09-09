using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyManager : MonoBehaviour
{
    private int HP = 10;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void Damage()
    {
        if (HP > 0)
        {
            HP--;
        }
    }

    public int GetHP()
    {
        return HP;
    }
}
