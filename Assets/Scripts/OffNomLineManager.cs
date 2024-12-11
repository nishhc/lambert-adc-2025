using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class OffNomLineManager : MonoBehaviour
{
    [SerializeField] private CSV_Parser csvParser;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private float updateInterval = 0.1f;
    [SerializeField] private int skipRate = 5;
    [SerializeField] private Toggle offnomPath;

    private float lastUpdateTime;
    private int lastPositionCount;

    void Start()
    {
        lineRenderer.positionCount = csvParser.OffNomPos.Count;

        DrawLine();
    }

    void Update()
    {
        if (csvParser.OffNomPos.Count != lastPositionCount || Time.time - lastUpdateTime > updateInterval)
        {
            lastPositionCount = csvParser.OffNomPos.Count;
            lastUpdateTime = Time.time;

            DrawLine();
        }

        lineRenderer.enabled = offnomPath.isOn;
    }

    private void DrawLine()
    {
        int rate = skipRate;
        int count = (int)Mathf.Ceil((float)csvParser.OffNomPos.Count / (float)rate);

        lineRenderer.positionCount = count;

        for (int i = 0; i < count; i++)
        {
            int sampledIndex = i * rate;
            if (sampledIndex < csvParser.OffNomPos.Count)
            {
                lineRenderer.SetPosition(i, csvParser.OffNomPos[sampledIndex]);
            }
        }
    }
}
