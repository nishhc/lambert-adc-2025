using System.Collections.Generic;
using UnityEngine;

public class RocketMovement : MonoBehaviour
{
    [Range(0.0f, 778990.1988f)]
    [SerializeField] private float simulationTime = 0f;
    [SerializeField] private float timeMultiplier = 1f;
    [SerializeField] private float timeStep = 1f;
    private Rigidbody _rb;
    private List<Vector3> pathPositions;
    private List<Vector3> pathVelocities;
    private List<float> pathTimes;
    [SerializeField] private Transform _mesh;

    private bool isPaused = false;

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
        UpdateStateAtTime(simulationTime);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isPaused = !isPaused;
        }

        if (isPaused)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                StepForward();
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                StepBackward();
            }
        }
    }

    void FixedUpdate()
    {
        if (!isPaused)
        {
            simulationTime += Time.fixedDeltaTime * timeMultiplier;
        }
        UpdateStateAtTime(simulationTime);

    }

    void UpdateStateAtTime(float time)
    {
        simulationTime = Mathf.Clamp(time, pathTimes[0], pathTimes[pathTimes.Count - 1]);

        int segmentIndex = FindSegment(simulationTime);
        float segmentStartTime = pathTimes[segmentIndex];
        float segmentEndTime = pathTimes[segmentIndex + 1];

        float segmentDuration = segmentEndTime - segmentStartTime;
        float segmentProgress = (simulationTime - segmentStartTime) / segmentDuration;

        Vector3 interpolatedPosition = Vector3.Lerp(pathPositions[segmentIndex], pathPositions[segmentIndex + 1], segmentProgress);
        Vector3 interpolatedVelocity = Vector3.Lerp(pathVelocities[segmentIndex], pathVelocities[segmentIndex + 1], segmentProgress);

        _rb.MovePosition(interpolatedPosition);
        _rb.velocity = interpolatedVelocity;
    }

    int FindSegment(float time)
    {
        for (int i = 0; i < pathTimes.Count - 1; i++)
        {
            if (time >= pathTimes[i] && time <= pathTimes[i + 1])
            {
                //_mesh.transform.rotation = Quaternion.Euler(Vector3.RotateTowards(_mesh.transform.rotation.eulerAngles, pathPositions[i + 1], 1000000, 1000000000));

                return i;
            }
        }
        return pathTimes.Count - 2;
    }

    public void StepForward()
    {
        UpdateStateAtTime(simulationTime + timeStep);
    }

    public void StepBackward()
    {
        UpdateStateAtTime(simulationTime - timeStep);
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
