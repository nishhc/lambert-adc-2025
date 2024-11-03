using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PositionVelocity
{
    public Vector3 position;
    public Vector3 velocity;

    public PositionVelocity(Vector3 _position, Vector3 _velocity)
    {
        position = _position;
        velocity = _velocity;
    }
}

public class PointManager : MonoBehaviour
{
    public PositionVelocity[] points;
    [SerializeField] private LineManager lineManager;
    [HideInInspector] public Vector3[] positionPoints;
    [HideInInspector] public Vector3[] velocityPoints;

    private void Start()
    {
        positionPoints = new Vector3[points.Length];
        velocityPoints = new Vector3[points.Length];
        
        for (int i = 0; i < points.Length; i++)
        {
            positionPoints[i] = points[i].position;
            velocityPoints[i] = points[i].velocity;
        }

        lineManager.DrawStaticLine(positionPoints);
    }
}