using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIEarth : MonoBehaviour
{
    [SerializeField] private Transform Earth;
    [SerializeField] private float speed;
    [SerializeField] private float time;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Earth != null)
        {
            time += Time.deltaTime * speed;
            float angle = 0.7f;
            Earth.transform.rotation = Quaternion.Euler(new Vector3(Earth.transform.eulerAngles.x, angle, Earth.transform.rotation.eulerAngles.z));
            double earthMins = time / 60;
            while (earthMins >= 1436.06817551388)
            {
                earthMins -= 1436.06817551388;
            }

            Earth.transform.rotation = Quaternion.Euler(
            new Vector3(
                Earth.transform.eulerAngles.x,
                Earth.transform.rotation.eulerAngles.y + (float)(-0.2506844773 * earthMins),
                Earth.transform.rotation.eulerAngles.z
            ));
        }
    }

    public void startapp()
    {
        SceneManager.LoadScene(1);
    }

    public void stopapp()
    {
        SceneManager.LoadScene(0);
    }
}
