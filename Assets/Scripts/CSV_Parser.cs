using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSV_Parser : MonoBehaviour
{
    List<Dictionary<string, int>> flightData = new List<Dictionary<string, int>>();
    void Awake()
    {
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
        }
return data;
    }
}
