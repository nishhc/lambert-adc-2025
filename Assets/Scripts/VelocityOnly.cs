using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class VelocityOnly : MonoBehaviour
{

    private Rigidbody rb;
    private int current = 0;
    private AppManager manager;
    private Vector3 vel;
    private CSV_Parser _parser;


    void Awake()
    {
        manager = GameObject.FindGameObjectWithTag("AppManager").GetComponent<AppManager>();
        _parser = manager.gameObject.GetComponent<CSV_Parser>();


    }
    // Start is called before the first frame update
    void Start()
    {
        while (manager.simulationTime / 60 > _parser.Times[current]) { current++; }
        rb = GetComponent<Rigidbody>();
        rb.position = _parser.Positions[current];
        vel = _parser.Velocities[current];
    }

    // Update is called once per frame
    void Update()
    {

        if (manager.simulationTime > _parser.Times[current])
        {
            current++;
        }

        float segDur = _parser.Times[current] - _parser.Times[current - 1];
        float ax = (_parser.Velocities[current].x - _parser.Velocities[current - 1].x) / segDur;
        float ay = (_parser.Velocities[current].y - _parser.Velocities[current - 1].y) / segDur;
        float az = (_parser.Velocities[current].z - _parser.Velocities[current - 1].z) / segDur;
        Vector3 a = new Vector3(ax, ay, az);

        vel += a * Time.deltaTime;
        rb.MovePosition(rb.position + (vel * Time.deltaTime));
    }
}