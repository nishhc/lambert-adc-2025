using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RocketMovement : MonoBehaviour
{
    private Rigidbody _rb;
    private List<Vector3> pathPositions;
    private List<Vector3> pathVelocities;
    private List<float> pathTimes;
    private AppManager manager;
    [SerializeField] private Transform _mesh;
    private float rocketSimTime;
    [SerializeField] private TextMeshProUGUI totalDist;
    [SerializeField][ReadOnlyField] private float totalDistance = 0;

    void Awake()
    {
        manager = GameObject.FindGameObjectWithTag("AppManager").GetComponent<AppManager>();
    }

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.mass = 60129.7f;

        pathPositions = CSV_Parser.Positions;
        pathVelocities = CSV_Parser.Velocities;
        pathTimes = CSV_Parser.Times;

        for (int i = 0; i < pathTimes.Count; i++)
        {
            pathTimes[i] *= Mathf.Pow(60, (float)1 / 2); // manually edit this
        }

        _rb.position = pathPositions[0];
        UpdateStateAtTime(rocketSimTime);
    }

    void FixedUpdate()
    {
        rocketSimTime = (float)manager.simulationTime;
        UpdateStateAtTime(rocketSimTime);
        totalDist.text = $"Total Distance Traveled: {totalDistance * 10} km";
    }

    void UpdateStateAtTime(float time)
    {
        rocketSimTime = Mathf.Clamp(time, pathTimes[0], pathTimes[pathTimes.Count - 1]);

        int segmentIndex = FindSegment(rocketSimTime);
        float segmentStartTime = pathTimes[segmentIndex];
        float segmentEndTime = pathTimes[segmentIndex + 1];

        float segmentDuration = segmentEndTime - segmentStartTime;
        float segmentProgress = (rocketSimTime - segmentStartTime) / segmentDuration;

        Vector3 interpolatedPosition = Vector3.Lerp(pathPositions[segmentIndex], pathPositions[segmentIndex + 1], segmentProgress);
        Vector3 interpolatedVelocity = Vector3.Lerp(pathVelocities[segmentIndex], pathVelocities[segmentIndex + 1], segmentProgress);

        _rb.MovePosition(interpolatedPosition);
        _rb.velocity = interpolatedVelocity;

        CalculateTotalDistance(segmentIndex, segmentProgress);
    }

    int FindSegment(float time)
    {
        for (int i = 0; i < pathTimes.Count - 1; i++)
        {
            if (time >= pathTimes[i] && time <= pathTimes[i + 1])
            {
                return i;
            }
        }
        return pathTimes.Count - 2;
    }

    void CalculateTotalDistance(int segmentIndex, float segmentProgress)
    {
        totalDistance = 0;
        for (int i = 0; i < segmentIndex; i++)
        {
            totalDistance += Vector3.Distance(pathPositions[i], pathPositions[i + 1]);
        }

        float segmentDistance = Vector3.Distance(pathPositions[segmentIndex], pathPositions[segmentIndex + 1]);
        totalDistance += segmentDistance * segmentProgress;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        if (pathPositions != null && pathPositions.Count > 1)
        {
            for (int i = 0; i < pathPositions.Count - 1; i++)
            {
                Gizmos.DrawLine(pathPositions[i], pathPositions[i + 1]);
            }
        }
    }
}
