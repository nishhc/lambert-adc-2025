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
    [SerializeField] private TextMeshProUGUI elapsedTime;
    [SerializeField] private TMP_InputField _timeMultiplierText;
    [SerializeField] private GameObject Earth;
    // Update is called once per frame
    void Update()
    {

        // not super accurate but performant :D
        float angle = -129.4742f;
        Earth.transform.rotation = Quaternion.Euler(new Vector3(Earth.transform.eulerAngles.x, angle, Earth.transform.rotation.eulerAngles.z));
        double earthMins = simulationTime / 60;
        while (earthMins >= 1436.06817551388)
        {
            earthMins -= 1436.06817551388;
        }
        for (float i = 0; i < earthMins; i++)
        {
            Earth.transform.rotation = Quaternion.Euler(
            new Vector3(
                Earth.transform.eulerAngles.x,
                Earth.transform.rotation.eulerAngles.y + 0.2506844773f,
                Earth.transform.rotation.eulerAngles.z
            )
        );

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
        elapsedTime.text = $"Elapsed Time: {Math.Round(simulationTime / 60, 2)} mins";
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isPaused = !isPaused;
        }

        if (!isPaused && !tempPause)
        {
            slider.value = (float)simulationTime / 60;
            simulationTime += Time.deltaTime * timeMultiplier;
        }

        if (isPaused)
        {
            simulationTime = slider.value * 60;
            if (Input.GetKey(KeyCode.RightArrow))
            {
                StepForward();
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                StepBackward();
            }
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

    public void NotDragging()
    {
        tempPause = false;
        Debug.Log("notDragging");

    }
}
