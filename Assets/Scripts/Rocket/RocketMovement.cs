using System.Collections.Generic;
using UnityEngine;

public class RocketMovement : MonoBehaviour
{
    [SerializeField] private float speed = 1f;
    [SerializeField] private float acceleration = 5f; // Controls how fast the rocket accelerates
    private int currentSegment = 0;
    private float interpolationParameter = 0f;
    private Rigidbody _rb;
    private LineManager lineManager;

    private List<Vector3> pathPositions;
    private List<Vector3> pathVelocities;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.mass = 60129.7f; // Set the mass (if necessary)

        pathPositions = CSV_Parser.Positions;
        pathVelocities = CSV_Parser.Velocities;
        lineManager = GetComponent<LineManager>();
        _rb.position = pathPositions[1];
    }

    void Update()
    {
        if (currentSegment < pathPositions.Count - 3)
        {
            Vector3 targetPosition = GetCatmullRomPosition(interpolationParameter, pathPositions[currentSegment], pathPositions[currentSegment + 1], pathPositions[currentSegment + 2], pathPositions[currentSegment + 3]);
            Vector3 direction = (targetPosition - _rb.position).normalized;

            Vector3 targetVelocity = direction * speed;

            _rb.velocity = Vector3.Lerp(_rb.velocity, targetVelocity, Time.deltaTime * acceleration);

            interpolationParameter += Time.deltaTime * (speed / (targetPosition - _rb.position).magnitude);

            if (interpolationParameter >= 1f)
            {
                interpolationParameter = 0f;
                currentSegment++;
            }
        }

        if (lineManager != null)
        {
            lineManager.DrawDynamicLine(_rb.position);
        }
    }

    private Vector3 GetCatmullRomPosition(float parameter, Vector3 point0, Vector3 point1, Vector3 point2, Vector3 point3)
    {
        float parameterSquared = parameter * parameter;
        float parameterCubed = parameterSquared * parameter;

        return 0.5f * (
            (2 * point1) +
            (-point0 + point2) * parameter +
            (2 * point0 - 5 * point1 + 4 * point2 - point3) * parameterSquared +
            (-point0 + 3 * point1 - 3 * point2 + point3) * parameterCubed
        );
    }
}
