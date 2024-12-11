using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System;

public class VelocityUI : MonoBehaviour
{
  [Header("UI")]
  [SerializeField] private Transform arrow;
  [SerializeField] private TextMeshProUGUI velocityText;

  [Header("Rocket")]
  [SerializeField] private GameObject rocketArrow;

  [Header("Debug")]
  [SerializeField] private Vector3 velocity;

  private AppManager manager;
  private float time;
  private List<float> pathTimes;
  private List<Vector3> pathVelocities;
  public GameObject obj;
  private CSV_Parser _parser;
  [SerializeField] private Material mat;
  [SerializeField] private TextMeshProUGUI vel;
  [SerializeField] private Toggle velColorCode;



  void Awake()
  {
    manager = GameObject.FindGameObjectWithTag("AppManager").GetComponent<AppManager>();
    _parser = manager.gameObject.GetComponent<CSV_Parser>();

  }

  void Start()
  {
    pathVelocities = _parser.Velocities;
    pathTimes = _parser.minTimes;
  }

  private void FixedUpdate()
  {
    time = (float)manager.simulationTime;
    int segmentIndex = FindSegment(time / 60);
    Debug.Log(pathVelocities[segmentIndex] + $" {segmentIndex}");
    velocity = pathVelocities[segmentIndex] * (1f / _parser.INITIAL_SCALE);
    vel.text = $"{Math.Round(velocity.x, 2)} km/s\n{Math.Round(velocity.z, 2)} km/s\n{Math.Round(velocity.y, 2)} km / s";
    float magnitude = velocity.magnitude;//0.9f + (velocity.magnitude / 11 * 0.2f);
    velocityText.text = "Velocity:\n" + magnitude.ToString("F4") + " km/s";
    /*
    Debug.Log("Velocity X: " + velocity.x.ToString("F4") + " km/s");
    Debug.Log("Velocity Y: " + velocity.y.ToString("F4") + " km/s");
    Debug.Log("Velocity Z: " + velocity.z.ToString("F4") + " km/s");
    Debug.Log("Velocity: " + magnitude.ToString("F4") + " km/s");
    */
    mat.color = velColorCode.isOn ? Color.HSVToRGB((110 - (magnitude * (110 / 11))) / 360, 1, 1) : Color.white;
    mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, a: 0.8f);
    Vector3 directionToNextPoint = (new Vector3(-velocity.x, velocity.y, -velocity.z) * 100).normalized;
    //rocketArrow.transform.localScale = new Vector3(0.5f, 0.5f, velocity.magnitude * 6);
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

  public void ToggleRocketVelocity()
  {
    rocketArrow.SetActive(!rocketArrow.activeSelf);
  }
}
