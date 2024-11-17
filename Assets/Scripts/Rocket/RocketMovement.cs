using System.Collections.Generic;
using UnityEngine;

public class RocketMovement : MonoBehaviour
{
    [SerializeField] private float timeMultiplier = 1f;
    private float maxTimeMultiplier;
    private int currentSegment = 0;
    private Rigidbody _rb;

    private List<Vector3> pathPositions;
    private List<float> pathTimes;

    [SerializeField][ReadOnlyField] private float simulationTime = 0f;
    [SerializeField][ReadOnlyField] private float missionElapsedTime = 0f;

    public void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.mass = 60129.7f;

        pathPositions = CSV_Parser.Positions;
        pathTimes = CSV_Parser.Times;

        float smallestSegmentDuration = float.MaxValue;
        for (int i = 0; i < pathTimes.Count - 1; i++)
        {
            float segmentDuration = pathTimes[i + 1] - pathTimes[i];
            if (segmentDuration < smallestSegmentDuration)
            {
                smallestSegmentDuration = segmentDuration;
            }
        }

        maxTimeMultiplier = smallestSegmentDuration * 3600f * 0.9f;

        timeMultiplier = Mathf.Clamp(timeMultiplier, 0f, maxTimeMultiplier);

        if (pathPositions.Count > 0)
        {
            _rb.position = pathPositions[0];
        }
        else
        {
            Debug.LogError("No positions loaded from CSV!");
        }
    }

    public void OnValidate()
    {
        if (pathTimes != null && pathTimes.Count > 1)
        {
            float smallestSegmentDuration = float.MaxValue;
            for (int i = 0; i < pathTimes.Count - 1; i++)
            {
                float segmentDuration = pathTimes[i + 1] - pathTimes[i];
                if (segmentDuration < smallestSegmentDuration)
                {
                    smallestSegmentDuration = segmentDuration;
                }
            }
            maxTimeMultiplier = (smallestSegmentDuration * 3600f) * 0.9f;
        }

        timeMultiplier = Mathf.Clamp(timeMultiplier, 0f, maxTimeMultiplier);
    }

    public void FixedUpdate()
    {
        timeMultiplier = Mathf.Clamp(timeMultiplier, 0f, maxTimeMultiplier);

        simulationTime += Time.deltaTime * timeMultiplier;
        missionElapsedTime = simulationTime / 60f;

        if (currentSegment < pathPositions.Count - 1)
        {
            if (missionElapsedTime >= pathTimes[currentSegment + 1])
            {
                currentSegment++;
                if (currentSegment < pathPositions.Count - 1)
                {
                    _rb.position = pathPositions[currentSegment];
                }
            }
            else
            {
                float segmentStartTime = pathTimes[currentSegment];
                float segmentEndTime = pathTimes[currentSegment + 1];
                float segmentDuration = segmentEndTime - segmentStartTime;

                float segmentProgress = Mathf.Clamp01((missionElapsedTime - segmentStartTime) / segmentDuration);

                Vector3 interpolatedPosition = Vector3.Lerp(
                    pathPositions[currentSegment],
                    pathPositions[currentSegment + 1],
                    segmentProgress
                );

                _rb.MovePosition(interpolatedPosition);
            }
        }
        else
        {
            _rb.velocity = Vector3.zero;
        }
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
