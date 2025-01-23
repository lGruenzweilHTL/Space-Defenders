using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Enemy_Laser : MonoBehaviour
{
    private Transform tower;
    private Transform towerShield;

    [SerializeField] private float minDist;
    [SerializeField] private float speed;
    [SerializeField] private float damage;

    [SerializeField] private Transform firePoint;
    [SerializeField] private LineRenderer laserRenderer;

    void Start()
    {
        if (GameObject.Find("Tower") != null) tower = GameObject.Find("Tower").transform;
        if (tower.GetChild(3) != null) towerShield = tower.GetChild(3).transform;
    }

    void Update()
    {
        //look at tower
        Vector2 lookDir = tower.position - transform.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        if(Vector2.Distance(transform.position, tower.position) > minDist)
        {
            //move to tower
            transform.position = Vector2.MoveTowards(transform.position, tower.position, speed * Time.deltaTime);

            laserRenderer.positionCount = 0;
        }
        else
        {
            Laser();
        }
    }

    private void Laser()
    {
        laserRenderer.positionCount = 2;
        laserRenderer.SetPosition(0, firePoint.position);
        Vector3 laserDir = tower.position - firePoint.position;
        RaycastHit2D hit = Physics2D.Raycast(firePoint.position, laserDir, Mathf.Infinity);
        laserRenderer.SetPosition(1, hit.point);

        if(hit.transform.tag == "Planet")
        {
            if (towerShield.gameObject.activeSelf == false)
            {
                tower.GetComponent<Tower_Health>().Tower_TakeDamage(damage * Time.deltaTime);
            }
            else
            {
                towerShield.GetComponent<Shield_Health>().Shield_TakeDamage(damage * Time.deltaTime);
            }
        }
    }
}
