using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CSV_Parser : MonoBehaviour
{
    [SerializeField] private TextAsset csvFile;
    [SerializeField] private TextAsset bonusFile;

    public static List<Vector3> Positions { get; } = new List<Vector3>();
    public static List<Vector3> Velocities { get; } = new List<Vector3>();
    public static List<float> Times { get; } = new List<float>();

    public static List<float> MoonPositions { get; } = new List<float>();
    public static List<float> MoonTimes { get; } = new List<float>();

    [SerializeField] private GameObject _artemisPointer;
    [SerializeField] private bool _showPoints = false;
    [SerializeField] private float _initialScale = 0.1f;
    [SerializeField] private int _pointsEvery = 20;

    private void Awake()
    {
        Positions.Clear();
        Velocities.Clear();
        Times.Clear();

        if (csvFile != null)
        {
            ProcessCSVData(csvFile);
        }
        else
        {
            Debug.LogError("No CSV file assigned in the inspector.");
        }
    }

    private void ProcessCSVData(TextAsset csvData)
    {
        var lines = csvData.text.Split('\n');
        if (lines.Length <= 1) return;

        string[] headers = lines[0].Split(',').Select(h => h.Trim()).ToArray();

        // Extract column indices for relevant fields
        int timeIndex = Array.IndexOf(headers, "PRECISE MISSION TIME (min)");
        int ppxIndex = Array.IndexOf(headers, "Ppx");
        int ppyIndex = Array.IndexOf(headers, "Ppy");
        int ppzIndex = Array.IndexOf(headers, "Ppz");
        int pvxIndex = Array.IndexOf(headers, "Pvx");
        int pvyIndex = Array.IndexOf(headers, "Pvy");
        int pvzIndex = Array.IndexOf(headers, "Pvz");

        if (timeIndex == -1 || ppxIndex == -1 || ppyIndex == -1 || ppzIndex == -1 || pvxIndex == -1 || pvyIndex == -1 || pvzIndex == -1)
        {
            Debug.LogError("One or more required columns are missing in the CSV file.");
            return;
        }

        for (int i = 1; i < lines.Length; i++)
        {
            var line = lines[i].Trim();
            if (string.IsNullOrEmpty(line)) continue;

            var values = line.Split(',');

            if (values.Length <= Math.Max(timeIndex, Math.Max(ppzIndex, pvzIndex))) continue;

            if (float.TryParse(values[timeIndex], out float time) &&
                float.TryParse(values[ppxIndex], out float ppx) &&
                float.TryParse(values[ppyIndex], out float ppy) &&
                float.TryParse(values[ppzIndex], out float ppz) &&
                float.TryParse(values[pvxIndex], out float pvx) &&
                float.TryParse(values[pvyIndex], out float pvy) &&
                float.TryParse(values[pvzIndex], out float pvz))
            {
                Times.Add(time);

                Vector3 position = _initialScale * new Vector3(ppx, ppz, ppy);
                Positions.Add(position);

                Vector3 velocity = _initialScale * new Vector3(pvx, pvz, pvy);
                Velocities.Add(velocity);

                if (_showPoints && i % _pointsEvery == 0)
                {
                    Instantiate(_artemisPointer, position, Quaternion.identity);
                }
            }
            else
            {
                Debug.LogWarning($"Unable to parse data at line {i + 1}");
            }
        }
    }
}
