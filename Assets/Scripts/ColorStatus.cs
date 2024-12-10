using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ColorStatus : MonoBehaviour
{

    [SerializeField] private Color orbitingEarth;
    [SerializeField] private Color toMoon;
    [SerializeField] private Color returnToEarth;
    [SerializeField] private Color EDL;

    [SerializeField] private float[] maxTimesStageOrder = new float[4];
    [SerializeField] private TextMeshProUGUI status;
    private AppManager manager;

    void Awake()
    {
        manager = GameObject.FindGameObjectWithTag("AppManager").GetComponent<AppManager>();

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        double time = manager.simulationTime;
        if (time < maxTimesStageOrder[1])
        {
            status.text = "STAGE 1: ORBITING EARTH";
            status.color = orbitingEarth;
        }
        else

        {
            status.text = "STAGE 2: TO THE MOON";

            status.color = toMoon;
            if (time > maxTimesStageOrder[2])
            {
                status.text = "STAGE 3: RETURN TO EARTH";

                status.color = returnToEarth;
                if (time > maxTimesStageOrder[3])
                {
                    status.text = "STAGE 4: ENTRY DECENT AND LANDING";

                    status.color = EDL;
                }
            }
        }
    }
}
