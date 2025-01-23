using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SpaceDefenders.GameManagement;

public class Tower_Health : MonoBehaviour
{
    [SerializeField] private float TowerHealth;
    [SerializeField] private Slider healthSlider;

    void Update()
    {
        if(TowerHealth <= 0)
        {
            GameObject.Find("GameManager").GetComponent<DefendModeGameManager>().GameOver();
        }
        healthSlider.value = TowerHealth;
    }

    public void Tower_TakeDamage(float ammount)
    {
        TowerHealth -= ammount;
    }
}
