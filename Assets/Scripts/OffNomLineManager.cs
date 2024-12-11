using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OffNomLineManager : MonoBehaviour
{
    [SerializeField] public CSV_Parser csvParser;
    [SerializeField] private GameObject mesh;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private int skipRate = 5;
    [SerializeField] private Toggle offnomPath;
    [SerializeField] GameObject antennaavailabilitytext;
    public bool offnom { get; private set; } = false;


    private float lastUpdateTime;
    private int lastPositionCount;

    void Start()
    {
        lineRenderer.positionCount = csvParser.OffNomPos.Count;

        DrawLine();
    }

    void Update()
    {
        antennaavailabilitytext.SetActive(offnom);

        offnom = offnomPath.isOn;
        lineRenderer.enabled = offnomPath.isOn;
        mesh.SetActive(lineRenderer.enabled);
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
