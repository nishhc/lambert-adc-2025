using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreserveMinimapIcons : MonoBehaviour
{
    [SerializeField] private Camera minimapCam;
    private Vector3 initialScale;
    private float initialLineWidth;

    void Start()
    {
        initialScale = transform.localScale;

        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer != null)
        {
            initialLineWidth = lineRenderer.startWidth;
        }
    }

    void Update()
    {
        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer != null)
        {
            lineRenderer.startWidth = initialLineWidth / (30f / minimapCam.orthographicSize);
            lineRenderer.endWidth = initialLineWidth / (30f / minimapCam.orthographicSize);
        }
        else
        {
            transform.localScale = initialScale / (30f / minimapCam.orthographicSize);
        }
    }
}