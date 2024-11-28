using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tempvel : MonoBehaviour
{

    // TESTING, Do not use

    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.position = CSV_Parser.Positions[1];
    }

    // Update is called once per frame
    void FixedUpdate()
    {


        rb.velocity = new Vector3(CSV_Parser.Velocities[1].x + -0.00296079781667f, CSV_Parser.Velocities[1].y + 0.00536218818333f, CSV_Parser.Velocities[1].z + -0.00696307965f);
    }
}
