using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VelocityUI : MonoBehaviour
{
  [Header("UI")]
  [SerializeField] private Transform arrow;
  [SerializeField] private TextMeshProUGUI velocityText;

  [Header("Rocket")]
  [SerializeField] private Transform rocketArrow;

  [Header("Debug")]
  [SerializeField] private Vector3 velocity;

  private AppManager manager;
  private float time;
  private List<float> pathTimes;
  private List<Vector3> pathVelocities;
  public GameObject obj;


  void Awake()
  {
    manager = GameObject.FindGameObjectWithTag("AppManager").GetComponent<AppManager>();
  }

  void Start()
  {
    pathVelocities = CSV_Parser.Velocities;
    pathTimes = CSV_Parser.Times;
  }

  private void FixedUpdate()
  {
    time = (float)manager.simulationTime;
    int segmentIndex = FindSegment(time);
    velocity = pathVelocities[segmentIndex];
    float magnitude = velocity.magnitude;//0.9f + (velocity.magnitude / 11 * 0.2f);
    velocityText.text = "Velocity: " + (magnitude * 10).ToString("F4") + " km/s";
    /*
    Debug.Log("Velocity X: " + velocity.x.ToString("F4") + " km/s");
    Debug.Log("Velocity Y: " + velocity.y.ToString("F4") + " km/s");
    Debug.Log("Velocity Z: " + velocity.z.ToString("F4") + " km/s");
    Debug.Log("Velocity: " + magnitude.ToString("F4") + " km/s");
    */
    Vector3 directionToNextPoint = (new Vector3(-velocity.x, velocity.y, -velocity.z) * 100).normalized;
    rocketArrow.localScale = new Vector3(0.5f, 0.5f, 2f + (velocity.magnitude / 11 * 2f));
    arrow.rotation = Quaternion.LookRotation(directionToNextPoint, Vector3.up);
  }

  int FindSegment(float time)
  {
    for (int i = 0; i < pathTimes.Count - 1; i++)
    {
      if (time >= pathTimes[i] && time <= pathTimes[i + 1])
      {
        return i;
      }
    }
    return pathTimes.Count - 2;
  }
}
