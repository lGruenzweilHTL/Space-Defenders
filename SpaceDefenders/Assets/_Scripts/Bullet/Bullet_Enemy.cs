using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Enemy : MonoBehaviour
{
    [SerializeField] private GameObject hitEffect;
    [HideInInspector] public int damage;

    //if nothing is hit, destroy the bullet after 10 seconds
    void Start() { Destroy(gameObject, 10f); }

    void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
        Destroy(effect, 5f);
        Destroy(gameObject);

        if (collision.gameObject.GetComponent<Shield_Health>() != null) collision.gameObject.GetComponent<Shield_Health>().Shield_TakeDamage(damage);
        if (collision.gameObject.GetComponent<Player_Health>() != null) collision.gameObject.GetComponent<Player_Health>().Player_TakeDamage(damage);
        if (collision.gameObject.GetComponent<Tower_Health>() != null) collision.gameObject.GetComponent<Tower_Health>().Tower_TakeDamage(damage);
    }
}
