using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class AppManager : MonoBehaviour
{

    [Range(0.0f, 778990.1988f)]
    [SerializeField] public double simulationTime = 0f;
    [SerializeField] public float timeMultiplier = 1f;
    [SerializeField] public float timeStep = 1f;
    public bool isPaused { get; private set; } = false;
    private bool tempPause = false;
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI dys;
    [SerializeField] private TextMeshProUGUI hrs;
    [SerializeField] private TextMeshProUGUI mins;
    [SerializeField] private TextMeshProUGUI secs;
    [SerializeField] private TextMeshProUGUI justMins;
    [SerializeField] private TMP_InputField _timeMultiplierText;
    [SerializeField] private GameObject _colorkey;
    [SerializeField] private GameObject _controls;
    [SerializeField] private GameObject _help;
    [SerializeField] private GameObject Earth;
    [SerializeField] private GameObject nonControlUI;
    [SerializeField] private Image playbutton;
    [SerializeField] private Sprite[] playpause = new Sprite[2];
    // Update is called once per frame
    void Update()
    {
        simulationTime = Math.Clamp(simulationTime, 0, 778990.1988);
        // not super accurate but performant :D
        float angle = 0.7f;
        Earth.transform.rotation = Quaternion.Euler(new Vector3(Earth.transform.eulerAngles.x, angle, Earth.transform.rotation.eulerAngles.z));
        double earthMins = simulationTime / 60;
        while (earthMins >= 1436.06817551388)
        {
            earthMins -= 1436.06817551388;
        }

        Earth.transform.rotation = Quaternion.Euler(
        new Vector3(
            Earth.transform.eulerAngles.x,
            Earth.transform.rotation.eulerAngles.y + (float)(-0.2506844773 * earthMins),
            Earth.transform.rotation.eulerAngles.z
        ));

        if (Input.GetKeyDown(KeyCode.U))
        {
            nonControlUI.SetActive(!nonControlUI.activeSelf);
        }

        if (float.TryParse(_timeMultiplierText.text, out float result))
            timeMultiplier = result;
        else
        {
            timeMultiplier = 1;
            _timeMultiplierText.text = $"{1}";
        }

        if (simulationTime > 778990.1988f)
            simulationTime = 778990.1988f;
        int totalSeconds = (int)simulationTime;
        int days = totalSeconds / (24 * 3600);
        totalSeconds %= (24 * 3600);
        int hours = totalSeconds / 3600;
        totalSeconds %= 3600;
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;

        dys.text = days.ToString();
        justMins.text = $"Elapsed Time: {Math.Round(simulationTime / 60, 2)} mins";
        hrs.text = hours <= 9 ? $"0{hours.ToString()}" : hours.ToString();
        mins.text = minutes <= 9 ? $"0{minutes.ToString()}" : minutes.ToString();
        secs.text = seconds <= 9 ? $"0{seconds.ToString()}" : seconds.ToString();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Pause();
        }

        if (!isPaused && !tempPause)
        {
            slider.value = (float)simulationTime / 60;
            simulationTime += Time.deltaTime * timeMultiplier;
        }

        if (isPaused)
        {
            simulationTime = slider.value * 60;
            /*
            if (Input.GetKey(KeyCode.RightArrow))
            {
                StepForward();
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                StepBackward();
            }
            */
        }

        if (tempPause)
            simulationTime = slider.value * 60;


    }

    public void StepForward()
    {
        simulationTime += timeStep;
        slider.value = (float)simulationTime / 60;

    }

    public void StepBackward()
    {
        simulationTime -= timeStep;
        slider.value = (float)simulationTime / 60;

    }

    public void IsDragging()
    {
        tempPause = true;
        Debug.Log("isDragging");
    }

    public void Pause()
    {
        isPaused = !isPaused;
        if (isPaused)
        {
            playbutton.sprite = playpause[1];
        }
        else
        {
            playbutton.sprite = playpause[0];

        }
    }
    public void NotDragging()
    {
        tempPause = false;
        Debug.Log("notDragging");

    }

    public void Help()
    {
        print("Help");
        _help.SetActive(!_help.activeSelf);
    }

    public void Controls()
    {
        print("Controls");
        _controls.SetActive(!_controls.activeSelf);
    }

    public void ColorKey()
    {
        print("Keys");
        _colorkey.SetActive(!_colorkey.activeSelf);
    }



}
