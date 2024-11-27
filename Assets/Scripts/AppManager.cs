using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AppManager : MonoBehaviour
{

    [Range(0.0f, 778990.1988f)]
    [SerializeField] public double simulationTime = 0f;
    [SerializeField] public float timeMultiplier = 1f;
    [SerializeField] public float timeStep = 1f;
    private bool isPaused = false;
    private bool tempPause = false;
    [SerializeField] private Slider slider;
    // Update is called once per frame
    void Update()
    {

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
