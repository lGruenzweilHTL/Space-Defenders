using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canvas_CancelRotation : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;
    void Update()
    {
        transform.position = target.position + offset;
        transform.rotation = Quaternion.Euler(0, 0, -target.rotation.z);
    }
}