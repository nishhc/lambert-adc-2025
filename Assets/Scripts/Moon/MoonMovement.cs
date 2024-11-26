using System.Collections.Generic;
using UnityEngine;

public class MoonMovement : MonoBehaviour
{

    private Rigidbody _rb;
    private List<Vector3> pathPositions;
    private List<float> pathTimes;
    private AppManager manager;
    [SerializeField] private Transform _mesh;


    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        manager = GameObject.FindGameObjectWithTag("AppManager").GetComponent<AppManager>();

        pathPositions = CSV_Parser.MoonPositions;
        pathTimes = CSV_Parser.Times;

        for (int i = 0; i < pathTimes.Count; i++)
        {
            pathTimes[i] *= 60f;
        }

        _rb.position = pathPositions[0];
        UpdateStateAtTime(manager.simulationTime);
    }

    void FixedUpdate()
    {

        UpdateStateAtTime(manager.simulationTime);

    }

    void UpdateStateAtTime(float time)
    {
        manager.simulationTime = Mathf.Clamp(time, pathTimes[0], pathTimes[pathTimes.Count - 1]);

        int segmentIndex = FindSegment(manager.simulationTime);
        float segmentStartTime = pathTimes[segmentIndex];
        float segmentEndTime = pathTimes[segmentIndex + 1];

        float segmentDuration = segmentEndTime - segmentStartTime;
        float segmentProgress = (manager.simulationTime - segmentStartTime) / segmentDuration;

        Vector3 interpolatedPosition = Vector3.Lerp(pathPositions[segmentIndex], pathPositions[segmentIndex + 1], segmentProgress);

        _rb.MovePosition(interpolatedPosition);
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

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        if (pathPositions != null && pathPositions.Count > 1)
        {
            for (int i = 0; i < pathPositions.Count - 2; i++)
            {
                Gizmos.DrawLine(pathPositions[i], pathPositions[i + 1]);
            }
        }
    }
}
