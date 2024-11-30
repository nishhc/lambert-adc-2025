using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MoonMovement : MonoBehaviour
{

    private Rigidbody _rb;
    private List<Vector3> pathPositions;
    private List<float> pathTimes;
    private AppManager manager;
    [SerializeField] private Transform _mesh;
    private float moonSimTime;
    private CSV_Parser _parser;

    void Awake()
    {
        manager = GameObject.FindGameObjectWithTag("AppManager").GetComponent<AppManager>();
        _parser = manager.gameObject.GetComponent<CSV_Parser>();

    }

    void Start()
    {
        _rb = GetComponent<Rigidbody>();

        pathPositions = new List<Vector3>(_parser.MoonPositions);
        pathTimes = new List<float>(_parser.minTimes);

        for (int i = 0; i < pathTimes.Count; i++)
        {
            pathTimes[i] *= 60;  // manually edit this
        }

        _rb.position = pathPositions[0];
    }

    void FixedUpdate()
    {
        moonSimTime = (float)manager.simulationTime;
        moonSimTime = Mathf.Clamp(moonSimTime, pathTimes[0], pathTimes[pathTimes.Count - 1]);

        int segmentIndex = FindSegment(moonSimTime);
        float segmentStartTime = pathTimes[segmentIndex];
        float segmentEndTime = pathTimes[segmentIndex + 1];

        float segmentDuration = segmentEndTime - segmentStartTime;
        float segmentProgress = (moonSimTime - segmentStartTime) / segmentDuration;

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
            for (int i = 0; i < pathPositions.Count - 1; i++)
            {
                Gizmos.DrawLine(pathPositions[i], pathPositions[i + 1]);
            }
        }
    }
}
