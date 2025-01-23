using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Player_Movement : MonoBehaviour {
    public float speed;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private FixedJoystick joystick;
    [SerializeField] private PlayerUpgrades playerUpgrades;
    [SerializeField] private Shield_Health shieldHealth;

    [SerializeField] private AudioSource source;
    [SerializeField] private ParticleSystem fireParticles;

    private void Start() {
        float h = 5 * playerUpgrades.player_shieldLevel;
        if (h > 0) {
            shieldHealth.SetDefaultHealth(h);
            shieldHealth.gameObject.SetActive(true);
        }
    }

    void FixedUpdate() {
        float currentSpeed = speed + (playerUpgrades.player_speedLevel * 0.5f);
        rb.velocity = new Vector2(joystick.Horizontal * currentSpeed, joystick.Vertical * currentSpeed);

        //if moving
        if (joystick.Horizontal != 0 || joystick.Vertical != 0) {
            //play if sound not currently playing
            if (!source.isPlaying) source.Play();

            //play fire particles
            fireParticles.Play();

            //look towards flying direction
            float rotZ = Mathf.Atan2(joystick.Horizontal, joystick.Vertical) * Mathf.Rad2Deg;
            transform.eulerAngles = new Vector3(0f, 0f, -rotZ);
        }
        //if not moving
        else {
            //stop playing the sound
            source.Stop();

            //disable fire particles
            fireParticles.Stop();
        }
    }
}