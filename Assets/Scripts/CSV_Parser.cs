using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CSV_Parser : MonoBehaviour
{
    [SerializeField] private TextAsset csvFile;
    [SerializeField] private TextAsset bonusFile;
    [SerializeField] private TextAsset linkBudgetFile;

    public static List<Vector3> Positions { get; } = new List<Vector3>();
    public static List<Vector3> Velocities { get; } = new List<Vector3>();
    public static List<float> Times { get; } = new List<float>();

    public static List<Vector3> MoonPositions { get; } = new List<Vector3>();
    public static List<float> minTimes { get; } = new List<float>();
    public static List<int> WpsaStates { get; } = new List<int>();
    public static List<float> WpsaRanges { get; } = new List<float>();
    public static List<int> DS54States { get; } = new List<int>();
    public static List<float> DS54Ranges { get; } = new List<float>();
    public static List<int> DS24States { get; } = new List<int>();
    public static List<float> DS24Ranges { get; } = new List<float>();
    public static List<int> DS34States { get; } = new List<int>();
    public static List<float> DS34Ranges { get; } = new List<float>();


    [SerializeField] private GameObject _artemisPointer;
    [SerializeField] private bool _showPoints = false;
    [SerializeField] public float INITIAL_SCALE = 0.1f;
    [SerializeField] private int _pointsEvery = 20;

    private void Awake()
    {
        Positions.Clear();
        Velocities.Clear();
        Times.Clear();
        minTimes.Clear();
        MoonPositions.Clear();
        WpsaRanges.Clear();
        WpsaStates.Clear();
        DS54States.Clear();
        DS54Ranges.Clear();
        DS24Ranges.Clear();
        DS24States.Clear();
        DS34Ranges.Clear();
        DS34Ranges.Clear();

        // messy parser for regular file
        if (csvFile != null)
        {
            var lines = csvFile.text.Split('\n');
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

                    Vector3 position = INITIAL_SCALE * new Vector3(ppx, ppz, ppy);
                    Positions.Add(position);

                    Vector3 velocity = INITIAL_SCALE * new Vector3(pvx, pvz, pvy);
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
        else
        {
            Debug.LogError("No CSV file assigned in the inspector.");
        }


        if (linkBudgetFile != null)
        {
            var lines = linkBudgetFile.text.Split('\n');
            if (lines.Length <= 1) return;

            string[] headers = lines[0].Split(',').Select(h => h.Trim()).ToArray();

            // Extract column indices for relevant fields
            int timeIndex = Array.IndexOf(headers, "MISSION ELAPSED TIME (min)");
            int wpsa = Array.IndexOf(headers, "WPSA");

            int wpsaRanges = Array.IndexOf(headers, "WPSA Range");
            int ds54 = Array.IndexOf(headers, "DS54");
            int ds54Ranges = Array.IndexOf(headers, "DS54 Range");
            int ds24 = Array.IndexOf(headers, "DS24");
            int ds24Ranges = Array.IndexOf(headers, "Range DS24");
            int ds34 = Array.IndexOf(headers, "DS34");
            int ds34Ranges = Array.IndexOf(headers, "Range DS34");

            for (int i = 1; i < lines.Length; i++)
            {
                var line = lines[i].Trim();
                if (string.IsNullOrEmpty(line)) continue;

                var values = line.Split(',');

                if (float.TryParse(values[timeIndex], out float time) &&
                    int.TryParse(values[wpsa], out int wpsaState) &&
                    float.TryParse(values[wpsaRanges], out float wpsaRange) &&
                    int.TryParse(values[ds54], out int ds54State) &&
                    float.TryParse(values[ds54Ranges], out float ds54Range) &&
                    int.TryParse(values[ds24], out int ds24State) &&
                    float.TryParse(values[ds24Ranges], out float ds24Range) &&
                    int.TryParse(values[ds34], out int ds34State) &&
                    float.TryParse(values[ds34Ranges], out float ds34Range))
                {
                    minTimes.Add(time);

                    WpsaStates.Add(wpsaState);
                    WpsaRanges.Add(wpsaRange);
                    DS54States.Add(ds54State);
                    DS54Ranges.Add(ds54Range);
                    DS24States.Add(ds24State);
                    DS24Ranges.Add(ds24Range);
                    DS34States.Add(ds34State);
                    DS34Ranges.Add(ds34Range);

                }
                else
                {
                    Debug.LogWarning($"Unable to parse data at line {i + 1}");
                }
            }
        }

        else
        {
            Debug.LogError("No Link Budget CSV file assigned in the inspector.");
        }

        if (bonusFile != null)
        {
            var lines = bonusFile.text.Split('\n');
            if (lines.Length <= 1) return;

            string[] headers = lines[0].Split(',').Select(h => h.Trim()).ToArray();

            // Extract column indices for relevant fields

            int mpxi = Array.IndexOf(headers, "MOON Rx(km)[J2000-EARTH]");
            int mpyi = Array.IndexOf(headers, "MOON Ry(km)[J2000-EARTH]");
            int mpzi = Array.IndexOf(headers, "MOON Rz(km)[J2000-EARTH]");

            for (int i = 1; i < lines.Length; i++)
            {
                var line = lines[i].Trim();
                if (string.IsNullOrEmpty(line)) continue;

                var values = line.Split(',');

                if (float.TryParse(values[mpxi], out float mpx) &&
                    float.TryParse(values[mpyi], out float mpy) &&
                    float.TryParse(values[mpzi], out float mpz))
                {

                    MoonPositions.Add(new Vector3(INITIAL_SCALE * mpx, INITIAL_SCALE * mpz, INITIAL_SCALE * mpy));
                }
            }
            print(MoonPositions.Count);
        }

    }

}
