using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Unity.VisualScripting;

public class LineManager : MonoBehaviour
{
    [SerializeField] private CSV_Parser csvParser;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private float updateInterval = 0.1f;
    [SerializeField] private int skipRate = 5;
    [SerializeField] private bool trailingLine = false;
    private List<float> _csvTimes;
    private Vector3[] pos;
    private AppManager manager;
    private Transform rocket;
    private float lastUpdateTime;
    private int lastPositionCount;
    private bool showPath = false;
    private bool showStaticPath = true;
    [SerializeField] private Toggle dynamicPath;
    [SerializeField] private Toggle staticPath;

    void Start()
    {
        lineRenderer.positionCount = csvParser.Positions.Count;
        _csvTimes = new List<float>(csvParser.Times);
        manager = GameObject.FindGameObjectWithTag("AppManager").GetComponent<AppManager>();
        rocket = GameObject.FindGameObjectWithTag("Rocket").transform;

        pos = csvParser.Positions.ToArray();

        if (!trailingLine)
            DrawLine(pos);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K) && trailingLine)
        {
            showPath = !showPath;
            dynamicPath.isOn = showPath;
        }

        else if ((Input.GetKeyDown(KeyCode.P) && !trailingLine && !gameObject.CompareTag("MinimapLineRenderer")))
        {
            showStaticPath = !showStaticPath;
            staticPath.isOn = showStaticPath;
        }

        showStaticPath = staticPath.isOn;
        lineRenderer.enabled = showStaticPath || trailingLine;

        showPath = dynamicPath.isOn;


    }

    void FixedUpdate()
    {

        if (trailingLine)
        {
            List<Vector3> poslis = new List<Vector3>();
            int seg = FindPreciseSegment((float)manager.simulationTime / 60);
            seg = seg > 0 && showPath ? seg : 0;
            poslis.Add(pos[0]);

            for (int i = 1; i < seg; i += 10)
            {
                poslis.Add(pos[i]);
            }
            if (showPath)
                poslis.Add(rocket.position);
            Vector3[] pos2 = poslis.ToArray();
            DrawLine(pos2);
        }
    }

    private void DrawLine(Vector3[] lis)
    {
        int rate = skipRate;
        int count = (int)Mathf.Ceil((float)lis.Length / (float)rate);

        lineRenderer.positionCount = count;

        for (int i = 0; i < count; i++)
        {
            int sampledIndex = i * rate;
            if (sampledIndex < lis.Length)
            {
                lineRenderer.SetPosition(i, lis[sampledIndex]);
            }
        }
    }

    private int FindPreciseSegment(float time)
    {
        for (int i = 0; i < _csvTimes.Count - 1; i++)
        {
            if (time >= _csvTimes[i] && time <= _csvTimes[i + 1])
                return i;
        }
        return _csvTimes.Count - 2;
    }

}
