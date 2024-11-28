using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class VelocityOnly : MonoBehaviour
{

    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.position = CSV_Parser.Positions[1];
        rb.velocity = CSV_Parser.Velocities[1];
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Watch this cube move using constant acceleration!
        Time.timeScale = 5;
        // its off because precision errors and non constant acceleration etc etc so we just use positions
        // These accelerations are ONLY between 8.23 and 9.23 seconds, I did not implement calculations just yet
        Debug.Log($"Cube: {rb.velocity * 10}");
        rb.velocity = new Vector3(rb.velocity.x + -0.000296079781667f * Time.deltaTime, rb.velocity.y - 0.000536218818333f * Time.deltaTime, rb.velocity.z + -0.000696307965f * Time.deltaTime);
    }
}
