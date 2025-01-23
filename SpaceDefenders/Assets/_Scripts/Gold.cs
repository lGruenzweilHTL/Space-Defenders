using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gold : MonoBehaviour
{
    [HideInInspector] public int dropAmmt;
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private TMPro.TMP_Text dropDisplayText;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.transform.tag == "Player")
        {
            playerStats.goldCount += dropAmmt;
            Destroy(gameObject);
        }
    }

    public void DisplayDropText()
    {
        dropDisplayText.text = dropAmmt.ToString();
    }
}