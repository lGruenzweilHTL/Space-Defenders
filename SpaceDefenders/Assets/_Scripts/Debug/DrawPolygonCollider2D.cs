using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(PolygonCollider2D))]
public class DrawPolygonCollider2D : MonoBehaviour
{
    [SerializeField] private GameObject linePrefab;
    LineRenderer lineRenderer;
    PolygonCollider2D polygonCollider2D;

    [HideInInspector] public bool isShowingHitboxes = false;

    void Start()
    {
        lineRenderer = Instantiate(linePrefab).GetComponent<LineRenderer>();
        lineRenderer.transform.SetParent(transform);
        lineRenderer.transform.localPosition = Vector3.zero;
        polygonCollider2D = GetComponent<PolygonCollider2D>();

    }

    void Update()
    {
        if( isShowingHitboxes)
        {
            var points = polygonCollider2D.GetPath(0); // dumb assumption for demo -- only one path

            Vector3[] positions = new Vector3[points.Length];
            for (int i = 0; i < points.Length; i++)
            {
                positions[i] = transform.TransformPoint(points[i]);
            }
            lineRenderer.positionCount = points.Length;
            lineRenderer.SetPositions(positions);
        }
        else
        {
            lineRenderer.positionCount = 0;
        }
    }
}