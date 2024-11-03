using System.Collections.Generic;
using UnityEngine;

public class CSV_Parser : MonoBehaviour
{
    [SerializeField] private TextAsset csvFile;
    private List<Dictionary<string, double>> flightData = new List<Dictionary<string, double>>();

    private void Awake()
    {
<<<<<<< HEAD
        flightData = GetDataFromCSV("csv_data.csv");
    }

    //Returns a list of dictionaries that stores all flight data
    //Each entry of the the list is a dictionary that stores all information about the flight at at a given time
    List<Dictionary<string, int>> GetDataFromCSV(string filename)
    {
        var dataFile = Resources.Load<TextAsset>(filename); //Loads data from file
        var dataLines = dataFile.text.Split("\n"); //Returns list where each entry is a row of data
        var data = new List<Dictionary<string, int>>(); //Creates empty list of data
        foreach (string line in dataLines)
        {
            var values = line.Split(",");
            var data_dict = new Dictionary<string, int> {
                { "MISSION_ELAPSED_TIME", int.Parse(values[0])},
                { "ROTATION_X", int.Parse(values[1])},
                { "ROTATION_Y", int.Parse(values[2])},
                { "ROTATION_Z", int.Parse(values[3])},
                { "VELOCITY_X", int.Parse(values[4])},
                { "VELOCITY_Y", int.Parse(values[5])},
                { "VELOCITY_Z", int.Parse(values[6])},
                { "MASS", int.Parse(values[7])},
                { "EARTH_ROTATION_X", int.Parse(values[8])},
                { "EARTH_ROTATION_Y", int.Parse(values[9])},
                { "EARTH_ROTATION_X", int.Parse(values[10])},
                { "EARTH_VELOCITY_X", int.Parse(values[11])},
                { "EARTH_VELOCITY_Y", int.Parse(values[12])},
                { "EARTH_VELOCITY_Z", int.Parse(values[13])},
                { "MOON_ROTATION_X", int.Parse(values[14])},
                { "MOON_ROTATION_Y", int.Parse(values[15])},
                { "MOON_ROTATION_X", int.Parse(values[16])},
                { "MOON_VELOCITY_X", int.Parse(values[17])},
                { "MOON_VELOCITY_Y", int.Parse(values[18])},
                { "MOON_VELOCITY_Z", int.Parse(values[19])},
            };
    data.Add(data_dict);
=======
        if (csvFile != null)
        {
            flightData = GetDataFromCSV(csvFile);
            PrintFlightData();
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
>>>>>>> 2376d63d8b65943c17bb221f70a648ba4807286e
        }
    }
}
