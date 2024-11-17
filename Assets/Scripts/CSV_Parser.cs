using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CSV_Parser : MonoBehaviour
{
    [SerializeField] private TextAsset csvFile;
    private List<Dictionary<string, double>> flightData = new List<Dictionary<string, double>>();
    public static List<Vector3> Positions { get; } = new List<Vector3>();
    public static List<Vector3> Velocities { get; } = new List<Vector3>();
    public static List<float> Times { get; } = new List<float>();
    private static string[] headers;
    [SerializeField] private GameObject _pointer;
    [SerializeField] private bool _showPoints = false;
    [SerializeField] private float _initialScale = 0.1f;

    private void Awake()
    {
        Positions.Clear();
        Velocities.Clear();
        Times.Clear();

        if (csvFile != null)
        {
            flightData = GetDataFromCSV(csvFile);
            PrintFlightData();
            DataList();
        }
        else
        {
            Debug.LogError("No CSV file assigned in the inspector.");
        }
    }

    private List<Dictionary<string, double>> GetDataFromCSV(TextAsset csvData)
    {
        var dataLines = csvData.text.Split('\n');
        if (dataLines.Length <= 1) return null;

        headers = dataLines[0].Split(',').Select(h => h.Trim()).ToArray();
        var data = new List<Dictionary<string, double>>();

        for (int i = 1; i < dataLines.Length; i++)
        {
            var line = dataLines[i].Trim();
            if (string.IsNullOrEmpty(line)) continue;

            var values = line.Split(',');
            var dataDict = new Dictionary<string, double>();

            for (int j = 0; j < headers.Length && j < values.Length; j++)
            {
                var header = headers[j].Trim();
                if (double.TryParse(values[j], out double doubleValue))
                {
                    dataDict[header] = doubleValue;
                }

                /* A lot of these are empty, so this just floods the console with warnings. not exactly sure what to do with this.
                most likely using machine learning later for data cleaning
                that'll be done by proj supervisors, not reg members unless experience shown 
                
                else 
                {
                    Debug.LogWarning($"Unable to parse '{values[j]}' as double in row {i + 1}, column '{headers[j]}'.");
                }
                */
            }

            data.Add(dataDict);
        }

        return data;
    }

    private void PrintFlightData()
    {
        foreach (var row in flightData)
        {
            string rowString = string.Join(", ", row.Select(kvp => $"{kvp.Key}: {kvp.Value}"));
            Debug.Log(rowString);
        }
    }

    private void DataList()
    {
        foreach (var row in flightData)
        {
            if (row.ContainsKey(headers[0]) &&
                row.ContainsKey("Rx(km)[J2000-EARTH]") &&
                row.ContainsKey("Rz(km)[J2000-EARTH]") &&
                row.ContainsKey("Ry(km)[J2000-EARTH]") &&
                row.ContainsKey("Vx(km/s)[J2000-EARTH]") &&
                row.ContainsKey("Vz(km/s)[J2000-EARTH]") &&
                row.ContainsKey("Vy(km/s)[J2000-EARTH]"))
            {
                float time = (float)row[headers[0]];
                Vector3 position = _initialScale * new Vector3((float)row["Rx(km)[J2000-EARTH]"], (float)row["Rz(km)[J2000-EARTH]"], (float)row["Ry(km)[J2000-EARTH]"]);
                Vector3 velocity = _initialScale * new Vector3((float)row["Vx(km/s)[J2000-EARTH]"], (float)row["Vz(km/s)[J2000-EARTH]"], (float)row["Vy(km/s)[J2000-EARTH]"]);
                Positions.Add(position);
                if (_showPoints) Instantiate(_pointer, position, transform.rotation);
                Velocities.Add(velocity);
                Times.Add(time);
            }
        }
    }
}
