using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret_Shoot : MonoBehaviour
{
    private Vector3 targetPos;
    private GameObject[] targets;

    [SerializeField] private Transform firePoint;
    public int damage;
    [SerializeField] private float bulletForce;
    [SerializeField] private GameObject bulletPrefab;

    private float nextTimeToShoot;
    public float timeBetweenShots;

    private float angle;

    void Update()
    {
        //setting the target
        targets = GameObject.FindGameObjectsWithTag("Enemy");
        if (targets.Length > 0) targetPos = new Vector2(targets[0].transform.position.x, targets[0].transform.position.y);

        //looking at the target
        Vector2 lookDir = targetPos - transform.position;
        angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        if(targets.Length > 0 && Time.time >= nextTimeToShoot)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bullet.GetComponent<Bullet>().damage = damage;
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);

        nextTimeToShoot = Time.time + timeBetweenShots;
    }
}