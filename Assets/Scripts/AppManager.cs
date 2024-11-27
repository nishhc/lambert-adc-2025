using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppManager : MonoBehaviour
{

    [Range(0.0f, 778990.1988f)]
    [SerializeField] public double simulationTime = 0f;
    [SerializeField] public float timeMultiplier = 1f;
    [SerializeField] public float timeStep = 1f;
    private bool isPaused = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isPaused = !isPaused;
        }

        if (!isPaused)
        {
            simulationTime += Time.deltaTime * timeMultiplier;
            Debug.Log(simulationTime);
        }

        if (isPaused)
        {
            if (Input.GetKey(KeyCode.RightArrow))
            {
                StepForward();
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                StepBackward();
            }
        }
    }

    public void StepForward()
    {
        simulationTime += timeStep;
    }

    public void StepBackward()
    {
        simulationTime -= timeStep;
    }
}
