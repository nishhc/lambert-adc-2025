using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;

public class CSV_Parser : MonoBehaviour
{
    [SerializeField] private TextAsset csvFile;
    [SerializeField] private TextAsset bonusFile;
    [SerializeField] private TextAsset linkBudgetFile;
    [SerializeField] private TextAsset mlPath;
    [SerializeField] private TextAsset mlOlPath;


    public List<Vector3> Positions { get; } = new List<Vector3>();
    public List<Vector3> Velocities { get; } = new List<Vector3>();
    public List<float> Times { get; } = new List<float>();

    public List<Vector3> MoonPositions { get; } = new List<Vector3>();
    public List<Vector3> OffNomPos { get; } = new List<Vector3>();
    public List<float> minTimes { get; } = new List<float>();
    public List<float> offMinTimes { get; } = new List<float>();
    public List<int> WpsaStates { get; } = new List<int>();
    public List<float> WpsaRanges { get; } = new List<float>();
    public List<int> DS54States { get; } = new List<int>();
    public List<float> DS54Ranges { get; } = new List<float>();
    public List<int> DS24States { get; } = new List<int>();
    public List<float> DS24Ranges { get; } = new List<float>();
    public List<int> DS34States { get; } = new List<int>();
    public List<float> DS34Ranges { get; } = new List<float>();
    public List<string> ACTIVE { get; } = new List<string>();


    [SerializeField] private GameObject _artemisPointer;
    [SerializeField] public float INITIAL_SCALE = 0.1f;
    [SerializeField] private int _pointsEvery = 20;

    private void Awake()
    {

        string[] moonLines = bonusFile.text.Split('\n');
        string[] bonusHeaders = moonLines[0].Split(',');
        for (int i = 1; i < moonLines.Length - 1; i++)
        {

            string[] current = moonLines[i].Split(',');


            if (float.TryParse(current[Array.IndexOf(bonusHeaders, "MOON Rx(km)[J2000-EARTH]")], out float mpx) &&
                float.TryParse(current[Array.IndexOf(bonusHeaders, "MOON Ry(km)[J2000-EARTH]")], out float mpy) &&
                float.TryParse(current[Array.IndexOf(bonusHeaders, "MOON Rz(km)[J2000-EARTH]")], out float mpz))
            {

                MoonPositions.Add(INITIAL_SCALE * new Vector3(mpx, mpz, mpy));
            }
            // caused due to an error being thrown somewhere



        }

        string[] onLines = mlOlPath.text.Split('\n');
        string[] onHeaders = onLines[0].Split(',');
        for (int i = 1; i < onLines.Length - 1; i++)
        {

            string[] current = onLines[i].Split(',');


            if (float.TryParse(current[Array.IndexOf(onHeaders, "MISSION ELAPSED TIME (min)")], out float time) && float.TryParse(current[1], out float ppx) &&
float.TryParse(current[2], out float ppy) &&
float.TryParse(current[3], out float ppz))
            {
                offMinTimes.Add(time);

                OffNomPos.Add(INITIAL_SCALE * new Vector3(ppx, ppz, ppy));
            }
            // caused due to an error being thrown somewhere



        }


        string[] smoothLines = mlPath.text.Split('\n');
        string[] smoothHeaders = smoothLines[0].Split(',');
        for (int i = 1; i < smoothLines.Length - 1; i++)
        {


            string[] current = smoothLines[i].Split(',');
            if (float.TryParse(current[1], out float ppx) &&
                float.TryParse(current[2], out float ppy) &&
                float.TryParse(current[3], out float ppz))

            {
                Times.Add(float.Parse(current[Array.IndexOf(smoothHeaders, "PRECISE MISSION TIME (min)")]));
                Positions.Add(INITIAL_SCALE * new Vector3(ppx, ppz, ppy));


            }

            // INITIAL SCALE provided to avoid floating point precision errors, do NOT remove anynone else who is working



        }

        string[] mainLines = csvFile.text.Split('\n');
        string[] mainHeaders = mainLines[0].Split(',');
        string[] linkbudgetLines = linkBudgetFile.text.Split('\n');
        string[] linkBudgetHeaders = linkbudgetLines[0].Split(',');
        for (int i = 1; i < linkbudgetLines.Length - 1; i++)
        {

            string[] current = linkbudgetLines[i].Split(',');
            string[] vel = mainLines[i].Split(',');

            if (float.TryParse(current[Array.IndexOf(linkBudgetHeaders, "MISSION ELAPSED TIME (min)")], out float missionElapsedTime) &&
                int.TryParse(current[Array.IndexOf(linkBudgetHeaders, "WPSA")], out int wpsaState) &&
                float.TryParse(current[Array.IndexOf(linkBudgetHeaders, "WPSA Range")], out float wpsaRange) &&
                int.TryParse(current[Array.IndexOf(linkBudgetHeaders, "DS54")], out int ds54State) &&
                float.TryParse(current[Array.IndexOf(linkBudgetHeaders, "DS54 Range")], out float ds54Range) &&
                int.TryParse(current[Array.IndexOf(linkBudgetHeaders, "DS24")], out int ds24State) &&
                float.TryParse(current[Array.IndexOf(linkBudgetHeaders, "Range DS24")], out float ds24Range) &&
                int.TryParse(current[Array.IndexOf(linkBudgetHeaders, "DS34")], out int ds34State) &&
                float.TryParse(current[8], out float ds34Range) // for some reason search Range DS34 doesn't work I couldnt figure out why so i just put hardcoded value of 8 
                && float.TryParse(vel[4], out float pvx) &&
                float.TryParse(vel[5], out float pvy) &&
                float.TryParse(vel[6], out float pvz))
            {
                ACTIVE.Add(current[9]);
                minTimes.Add(missionElapsedTime);
                WpsaStates.Add(wpsaState);
                WpsaRanges.Add(wpsaRange);
                DS54States.Add(ds54State);
                DS54Ranges.Add(ds54Range);
                DS24States.Add(ds24State);
                DS24Ranges.Add(ds24Range);
                DS34States.Add(ds34State);
                DS34Ranges.Add(ds34Range);
                Velocities.Add(INITIAL_SCALE * new Vector3(pvx, pvz, pvy));
            }
        }



    }

}
