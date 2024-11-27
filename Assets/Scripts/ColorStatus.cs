using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ColorStatus : MonoBehaviour
{

    [SerializeField] private Color orbitingEarth;
    [SerializeField] private Color toMoon;
    [SerializeField] private Color returnToEarth;
    [SerializeField] private Color EDL;
    [SerializeField] private RawImage status;
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
        if (time < 90000)
        {
            status.color = orbitingEarth;
        }
        else
        {
            status.color = toMoon;
            if (time > 435000)
            {
                status.color = returnToEarth;
                if (time > 778950)
                {
                    status.color = EDL;
                }
            }
        }
    }
}
