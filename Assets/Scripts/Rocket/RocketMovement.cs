using System.Collections.Generic;
using UnityEngine;

public class RocketMovement : MonoBehaviour
{
    [SerializeField] private float timeMultiplier = 1f;
    private Rigidbody _rb;

    private List<Vector3> pathPositions;
    private List<Vector3> pathVelocities;
    private List<float> pathTimes;

    private int currentSegment = 0;
    private float segmentStartTime;
    private float segmentEndTime;

    private float simulationTime = 0f;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.mass = 60129.7f;

        pathPositions = CSV_Parser.Positions;
        pathVelocities = CSV_Parser.Velocities;
        pathTimes = CSV_Parser.Times;

        for (int i = 0; i < pathTimes.Count; i++)
        {
            pathTimes[i] *= 60f;
        }

        _rb.position = pathPositions[0];
        simulationTime = 0f;

        segmentStartTime = pathTimes[0];
        segmentEndTime = pathTimes[1];
    }

    void FixedUpdate()
    {
        float deltaTime = Time.fixedDeltaTime * timeMultiplier;
        simulationTime += deltaTime;

        while (simulationTime > segmentEndTime && currentSegment < pathTimes.Count - 2)
        {
            currentSegment++;
            segmentStartTime = pathTimes[currentSegment];
            segmentEndTime = pathTimes[currentSegment + 1];
        }

        if (currentSegment >= pathTimes.Count - 1)
        {
            simulationTime = segmentEndTime;
        }

        float segmentDuration = segmentEndTime - segmentStartTime;
        float segmentProgress = Mathf.Clamp01((simulationTime - segmentStartTime) / segmentDuration);

        Vector3 interpolatedPosition = Vector3.Lerp(pathPositions[currentSegment], pathPositions[currentSegment + 1], segmentProgress);
        Vector3 interpolatedVelocity = Vector3.Lerp(pathVelocities[currentSegment], pathVelocities[currentSegment + 1], segmentProgress);

        _rb.MovePosition(interpolatedPosition);
        _rb.velocity = interpolatedVelocity;

        Debug.Log($"Time Multiplier: {timeMultiplier}, Simulation Time: {simulationTime}, Segment: {currentSegment}, Position: {_rb.position}, Velocity: {_rb.velocity}");
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
