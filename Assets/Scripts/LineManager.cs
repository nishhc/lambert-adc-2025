using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class LineManager : MonoBehaviour
{
    [SerializeField] private CSV_Parser csvParser;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private float updateInterval = 0.1f;
    [SerializeField] private int skipRate = 5;

    private float lastUpdateTime;
    private int lastPositionCount;

    void Start()
    {
        lineRenderer.positionCount = csvParser.Positions.Count;

        DrawLine();
    }

    void Update()
    {
        if (csvParser.Positions.Count != lastPositionCount || Time.time - lastUpdateTime > updateInterval)
        {
            lastPositionCount = csvParser.Positions.Count;
            lastUpdateTime = Time.time;
            
            DrawLine();
        }
    }

    private void DrawLine()
    {
        int rate = skipRate;
        int count = (int)Mathf.Ceil((float)csvParser.Positions.Count / (float)rate);

        lineRenderer.positionCount = count;

        for (int i = 0; i < count; i++)
        {
            int sampledIndex = i * rate;
            if (sampledIndex < csvParser.Positions.Count)
            {
                lineRenderer.SetPosition(i, csvParser.Positions[sampledIndex]);
            }
        }
    }
}
