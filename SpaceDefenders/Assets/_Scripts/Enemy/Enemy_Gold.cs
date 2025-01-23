using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Gold : MonoBehaviour
{
    private Transform player;

    [SerializeField] private float speed;
    [SerializeField] private float minDist;
    [SerializeField] private float maxDist;

    void Start()
    {
        if (GameObject.Find("Player") != null) player = GameObject.Find("Player").transform;
    }

    //gold is getting dropped through the health script
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

        //if too far from player
        if(Vector2.Distance(transform.position, player.position) > maxDist)
        {
            //move towards player
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }
    }
}
