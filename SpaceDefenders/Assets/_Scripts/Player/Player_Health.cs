using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using SpaceDefenders.GameManagement;

public class Player_Health : MonoBehaviour
{
    [SerializeField] private GameObject assignedManager;
    public int playerHealthMax;
    [HideInInspector] public int PlayerHealth;
    private int healthLastFrame;
    [SerializeField] private Slider healthSlider;
    [HideInInspector] public bool playerDead;

    [SerializeField] private AudioSource hurtSound;

    void Awake()
    {
        PlayerHealth = playerHealthMax;
        healthLastFrame = PlayerHealth;
    }

    void Update()
    {
        if(PlayerHealth != healthLastFrame)
        {
            hurtSound.Play();
        }

        if (PlayerHealth <= 0)
        {
            if(assignedManager.TryGetComponent<DefendModeGameManager>(out DefendModeGameManager manager))
            {
                manager.RespawnPlayer();
            }
            else
            {
                if (assignedManager.TryGetComponent<SurvivalModeGameManager>(out SurvivalModeGameManager Manager))
                {
                    Manager.GameOver();
                }
                else if (assignedManager.TryGetComponent<TimeModeGameManager>(out TimeModeGameManager TManager))
                {
                    TManager.RespawnPlayer();
                }
            }
        }
        healthSlider.value = PlayerHealth;

        healthLastFrame = PlayerHealth;
    }

    public void Player_TakeDamage(int ammount)
    {
        PlayerHealth -= ammount;
    }
}
