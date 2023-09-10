using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStatus : MonoBehaviour
{
    // ダミーの体力
    [SerializeField] private int HP = 0;
    [SerializeField] private int maxHP = 10;
    // HP表示用UI
    [SerializeField] private GameObject HPUI;
    // HP表示用スライダー
    private Slider hpSlider;

    void Start()
    {
        HP = maxHP;
        hpSlider = HPUI.transform.Find("HPBar").GetComponent<Slider>();
        hpSlider.value = 1f;
    }

    void Update()
    {
        if (hpSlider)
        {
            hpSlider.value = (float)HP / (float)maxHP;
        }
    }

    public void Damage(int damage)
    {
        if (HP > 0)
        {
            HP -= damage;
        }
    }

    public int GetHP()
    {
        return HP;
    }
}
