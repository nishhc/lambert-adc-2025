using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class VelocityOnly : MonoBehaviour
{

    private Rigidbody rb;
    private int current = 0;
    private AppManager manager;

    void Awake()
    {
        manager = GameObject.FindGameObjectWithTag("AppManager").GetComponent<AppManager>();
    }
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.position = CSV_Parser.Positions[1];
        rb.velocity = CSV_Parser.Velocities[1];
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
        print($"{CSV_Parser.Velocities[current].x} - {CSV_Parser.Velocities[current - 1].x}/{segDur}");
        float ay = (CSV_Parser.Velocities[current].y - CSV_Parser.Velocities[current - 1].y) / segDur;
        float az = (CSV_Parser.Velocities[current].z - CSV_Parser.Velocities[current - 1].z) / segDur;

        //Debug.Log($"{current} {CSV_Parser.Times[current] / 60} {ax} {ay} {az}");
        //Debug.Log($"Cube: {rb.velocity * 10}");

        rb.velocity = new Vector3(rb.velocity.x + ax * Time.deltaTime, rb.velocity.y + ay * Time.deltaTime, rb.velocity.z + az * Time.deltaTime);
    }
}
