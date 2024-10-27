using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineManager : MonoBehaviour
{
    private Vector3[] staticPoints;
    private List<Vector3> dynamicPoints = new List<Vector3>();
    private List<Vector3> trackingPoints = new List<Vector3>();
    private LineRenderer lineRenderer;

    private void Start()
    {
        dynamicPoints.Clear();
        trackingPoints.Clear();
        lineRenderer = GetComponent<LineRenderer>();
    }

    public void DrawStaticLine(Vector3[] staticPoints)
    {
        lineRenderer.positionCount = staticPoints.Length;
        this.staticPoints = staticPoints;

        for (int i = 0; i < staticPoints.Length; i++)
        {
            lineRenderer.SetPosition(i, staticPoints[i]);
        }
    }

    public void DrawDynamicLine(Vector3 position)
    {
        dynamicPoints.Add(position);
        lineRenderer.positionCount = dynamicPoints.Count;
        for (int i = 0; i < dynamicPoints.Count; i++)
        {
            lineRenderer.SetPosition(i, dynamicPoints[i]);
        }
    }

    public void DrawTracker(Vector3 position)
    {
        trackingPoints.Clear();
        trackingPoints.Add(position);
        lineRenderer.positionCount = trackingPoints.Count;
        for (int i = 0; i < trackingPoints.Count; i++)
        {
            lineRenderer.SetPosition(i, trackingPoints[i]);
        }
    }
}