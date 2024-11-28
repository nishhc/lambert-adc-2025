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

    void Awake()
    {
        manager = GameObject.FindGameObjectWithTag("AppManager").GetComponent<AppManager>();
    }
    // Start is called before the first frame update
    void Start()
    {
        while (manager.simulationTime / 60 > CSV_Parser.Times[current]) { current++; }
        rb = GetComponent<Rigidbody>();
        rb.position = CSV_Parser.Positions[current];
        vel = CSV_Parser.Velocities[current];
    }

    // Update is called once per frame
    void Update()
    {

        if (manager.simulationTime > CSV_Parser.Times[current])
        {
            current++;
        }

        float segDur = CSV_Parser.Times[current] - CSV_Parser.Times[current - 1];
        float ax = (CSV_Parser.Velocities[current].x - CSV_Parser.Velocities[current - 1].x) / segDur;
        float ay = (CSV_Parser.Velocities[current].y - CSV_Parser.Velocities[current - 1].y) / segDur;
        float az = (CSV_Parser.Velocities[current].z - CSV_Parser.Velocities[current - 1].z) / segDur;
        Vector3 a = new Vector3(ax, ay, az);

        vel += a * Time.deltaTime;
        rb.MovePosition(rb.position + (vel * Time.deltaTime));
    }
}