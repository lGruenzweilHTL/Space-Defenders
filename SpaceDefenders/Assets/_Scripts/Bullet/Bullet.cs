using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
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

        if (collision.gameObject.GetComponent<Enemy_Health>() != null) collision.gameObject.GetComponent<Enemy_Health>().Enemy_TakeDamage(damage);
    }
}
