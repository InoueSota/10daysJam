using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStatus : MonoBehaviour
{
    // �_�~�[�̗̑�
    [SerializeField] private int HP = 0;
    [SerializeField] private int maxHP = 10;
    // HP�\���pUI
    [SerializeField] private GameObject HPUI;
    // HP�\���p�X���C�_�[
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
