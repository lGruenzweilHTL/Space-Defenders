using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using FirstGearGames.SmoothCameraShaker;

public class Enemy_Health : MonoBehaviour
{
    public int EnemyHealth;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private GameObject goldPref;
    public int goldDropAmmt;
    [SerializeField] private GameObject explosion;
    private AudioSource explosionSource;
    [SerializeField] private ShakeData camShakeData;
    [SerializeField] private PlayerStats playerStats;

    //get a reference to the explosionSoundPlayer in the scene
    void Start() { if (GameObject.Find("ExplosionSoundPlayer") != null) explosionSource = GameObject.Find("ExplosionSoundPlayer").GetComponent<AudioSource>(); }

    void Update()
    {
        if (EnemyHealth <= 0)
        {
            if (goldDropAmmt > 0)
            {
                DropGold();
            }

            explosionSource.Play();
            if (playerStats.cameraShakeEnabled) CameraShakerHandler.Shake(camShakeData);
            ParticleSystem explosionParticles = Instantiate(explosion, transform.position, Quaternion.identity).GetComponent<ParticleSystem>();
            explosionParticles.Play();
            Destroy(explosionParticles, 0.5f);
            if (playerStats.vibrationsEnabled) Handheld.Vibrate();

            Destroy(gameObject);
        }
        healthSlider.value = EnemyHealth;
    }

    public void Enemy_TakeDamage(int ammount)
    {
        EnemyHealth -= ammount;
    }

    private void DropGold()
    {
        GameObject goldGO = Instantiate(goldPref, transform.position, transform.rotation);
        goldGO.GetComponent<Gold>().dropAmmt = goldDropAmmt;
        goldGO.GetComponent<Gold>().DisplayDropText();
    }
}