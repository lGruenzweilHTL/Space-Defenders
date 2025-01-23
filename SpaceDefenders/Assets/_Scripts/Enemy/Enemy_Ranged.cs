using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Ranged : MonoBehaviour
{
    private Transform player;

    [SerializeField] private float speed;
    [SerializeField] private float minDist;
    [SerializeField] private float maxDist;
    [SerializeField] private int damage;

    [SerializeField] private float timeBetweenShots;
    private float nextTimeToShoot;

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float bulletForce;

    void Start()
    {
        if (GameObject.Find("Player") != null) player = GameObject.Find("Player").transform;
    }

    void Update()
    {
        //look at player
        Vector2 lookDir = player.position - transform.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        //if too close to player
        if (Vector2.Distance(transform.position, player.position) < minDist)
        {
            //move away from player
            transform.position = Vector2.MoveTowards(transform.position, player.position, -speed * Time.deltaTime);
        }

        //if too far away from player
        if(Vector2.Distance(transform.position, player.position) > maxDist)
        {
            //move towards player
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }

        //if at the wanted distance to the player
        if(Vector2.Distance(transform.position, player.position) > minDist && Vector2.Distance(transform.position, player.position) < maxDist)
        {
            if (Time.time >= nextTimeToShoot)
            {
                //shoot at player
                Shoot();
                nextTimeToShoot = Time.time + timeBetweenShots;
            }
        }
    }

    private void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bullet.GetComponent<Bullet_Enemy>().damage = damage;
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);
    }
}