using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSV_Parser : MonoBehaviour
{
    List<Dictionary<string, int>> flightData = new List<Dictionary<string, int>>();
    void Awake()
    {
        flightData = GetDataFromCSV('csv_data.csv')
    }

    //Returns a list of dictionaries that stores all flight data
    //Each entry of the the list is a dictionary that stores all information about the flight at at a given time
    List<Dictionary<string, int>> GetDataFromCSV(filename)
    {
        var dataFile = Resources.Load<TextAsset>('csv_data.csv'); //Loads data from file
        var dataLines = dataset.text.Split('\n'); //Returns list where each entry is a row of data
        dataLines.removeAt(0) //Removes header information from data
        var data = new List<Dictionary<string, int>>() //Creates empty list of data
        foreach (String line in dataLines)
        {
            var values = line.Split(',');
            var data_dict = new Dictionary<string, int> {
                {'MISSION_ELAPSED_TIME' : values[0]},
                { 'ROTATION_X' : values[1]},
                { 'ROTATION_Y' : values[2]},
                { 'ROTATION_Z' : values[3]},
                { 'VELOCITY_X' : values[4]},
                { 'VELOCITY_Y' : values[5]},
                { 'VELOCITY_Z' : values[6]},
                { 'MASS' : values[7]},
                { 'EARTH_ROTATION_X' : values[8]},
                { 'EARTH_ROTATION_Y' : values[9]},
                { 'EARTH_ROTATION_X' : values[10]},
                { 'EARTH_VELOCITY_X' : values[11]},
                { 'EARTH_VELOCITY_Y' : values[12]},
                { 'EARTH_VELOCITY_Z' : values[13]},
                { 'MOON_ROTATION_X' : values[14]},
                { 'MOON_ROTATION_Y' : values[15]},
                { 'MOON_ROTATION_X' : values[16]},
                { 'MOON_VELOCITY_X' : values[17]},
                { 'MOON_VELOCITY_Y' : values[18]},
                { 'MOON_VELOCITY_Z' : values[19]},
            };
    data.Add(data_line);
        }
return data;
    }
}
