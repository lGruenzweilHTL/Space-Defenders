using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shield_Health : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private float ShieldHealth;
    void Update()
    {
        if(ShieldHealth <= 0)
        {
            gameObject.SetActive(false);
        }
        healthSlider.value = ShieldHealth;
    }

    public void Shield_TakeDamage(float ammt)
    {
        ShieldHealth -= ammt;
    }

    public void SetDefaultHealth(float amt) {
        ShieldHealth = amt;
        healthSlider.maxValue = amt;
        healthSlider.value = amt;
    }
}
