using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointManager : MonoBehaviour
{
    public Vector3[] points;
    [SerializeField] private LineManager lineManager;

    private void Start()
    {
        lineManager.DrawStaticLine(points);
    }
}
