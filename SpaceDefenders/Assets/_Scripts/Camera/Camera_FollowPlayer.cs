using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_FollowPlayer : MonoBehaviour
{
    [SerializeField] private bool clampPos = true;
    [SerializeField] private bool smooth = true;
    [Space]
    [SerializeField] private Transform player;
    public float SmoothSpeed;
    [SerializeField] private Vector3 offset;
    [HideInInspector] public bool playerDead = false;

    void Update()
    {
        Vector3 desiredPos = player.position + offset;
        if (!playerDead)
        {
            desiredPos = player.position + offset;
        }
        else
        {
            desiredPos = Vector2.zero;
        }

        if (clampPos) desiredPos = new Vector3(Mathf.Clamp(desiredPos.x, -14f, 14f), Mathf.Clamp(desiredPos.y, -20f, 20f), -10);

        Vector3 smoothedPos = Vector3.Lerp(transform.position, desiredPos, SmoothSpeed * Time.deltaTime);

        transform.position = smooth ? smoothedPos : desiredPos;
    }
}
