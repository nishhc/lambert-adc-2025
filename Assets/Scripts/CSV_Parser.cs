using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class CSV_Parser : MonoBehaviour
{
    [SerializeField] private TextAsset csvFile;
    private List<Dictionary<string, double>> flightData = new List<Dictionary<string, double>>();
    public static List<Vector3> Positions { get; } = new List<Vector3>();
    public static List<Vector3> Velocities { get; } = new List<Vector3>();
    public static List<float> Times { get; } = new List<float>();


    private void Awake()
    {
        Positions.Clear();
        Velocities.Clear();

        if (csvFile != null)
        {
            flightData = GetDataFromCSV(csvFile);
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
        var headers = dataLines[0].Split(',');
        var data = new List<Dictionary<string, double>>();

        for (int i = 1; i < dataLines.Length; i++)
        {
            var line = dataLines[i].Trim();
            if (string.IsNullOrEmpty(line)) continue;

            var values = line.Split(',');

            var dataDict = new Dictionary<string, double>();
            for (int j = 0; j < headers.Length && j < values.Length; j++)
            {
                if (double.TryParse(values[j], out double doubleValue))
                {
                    dataDict[headers[j]] = doubleValue;
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
            string rowString = "";
            foreach (var kvp in row)
            {
                rowString += $"{kvp.Key}: {kvp.Value}, ";
            }
            Debug.Log(rowString);
        }
    }

    private void DataList()
    {
        foreach (var row in flightData)
        {
            float time = (float)row["MISSION ELAPSED TIME (mins)"];
            Vector3 position = new Vector3((float)row["Rx(km)[J2000-EARTH]"], (float)row["Rz(km)[J2000-EARTH]"], (float)row["Ry(km)[J2000-EARTH]"]);
            Vector3 velocity = new Vector3((float)row["Vx(km/s)[J2000-EARTH]"], (float)row["Vz(km/s)[J2000-EARTH]"], (float)row["Vy(km/s)[J2000-EARTH]"]);
            Positions.Add(position);
            Velocities.Add(velocity);
            Times.Add(time);
        }
    }
}
