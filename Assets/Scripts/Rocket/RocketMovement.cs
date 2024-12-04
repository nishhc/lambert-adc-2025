using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine.AI;

public class RocketMovement : MonoBehaviour
{
  private List<Vector3> _csvPositions;
  private List<Vector3> _csvVelocities;

  private List<float> _csvTimes;
  private AppManager _manager;
  private CSV_Parser _parser;
  [SerializeField] private Transform _mesh;
  private float _rocketSimTime;
  [SerializeField] private TextMeshProUGUI _totalDist;
  [SerializeField] private TextMeshProUGUI _linkBudget;

  [SerializeField][ReadOnlyField] private float _totalDistance = 0;
  public Vector3 _calculatedVelocity { get; private set; } = Vector3.zero;
  [SerializeField] private List<Transform> _satellites;
  [SerializeField] private LayerMask _ignoreSatelliteLayers;
  [SerializeField] private TextMeshProUGUI _positionUI;
  [SerializeField] private TextMeshProUGUI _numberAvailable;
  [SerializeField]
  private Color[] antennaColors;

  private void Awake()
  {
    _manager = GameObject.FindGameObjectWithTag("AppManager").GetComponent<AppManager>();
    _parser = _manager.gameObject.GetComponent<CSV_Parser>();
  }

  private void Start()
  {

    _csvPositions = new List<Vector3>(_parser.Positions);
    _csvVelocities = new List<Vector3>(_parser.Velocities);
    _csvTimes = new List<float>(_parser.Times);

    for (int i = 0; i < _csvTimes.Count; i++)
    {
      _csvTimes[i] *= 60; // manually edit the denomintaor of power to amount of objects using this system of interpolated positions
    }

    transform.position = _csvPositions[0];
  }

  private void FixedUpdate()
  {


    // failed raycasting work on later im too tired for this rn
    /*
    foreach (Transform satellite in _satellites)
    {
      RaycastHit[] hits = Physics.RaycastAll(_mesh.position, satellite.position - transform.position, (satellite.position - transform.position).magnitude + 30, _ignoreSatelliteLayers);
      string names = "";
      foreach (RaycastHit hit in hits)
      {
        names += hit.collider.name + " ";
      }
      Debug.Log(satellite.name + " " + hits.Length + " " + names);

    }
    */
    _rocketSimTime = (float)_manager.simulationTime;

    _positionUI.text = $"{transform.position.x * 10}\n{transform.position.z * 10}\n{transform.position.y * 10}";

    int segmentIndex = FindPreciseSegment(_rocketSimTime);
    float segmentProgress = (_rocketSimTime - _csvTimes[segmentIndex]) / (_csvTimes[segmentIndex + 1] - _csvTimes[segmentIndex]);
    Vector3 interpolatedPosition = Vector3.Lerp(_csvPositions[segmentIndex], _csvPositions[segmentIndex + 1], segmentProgress);

    transform.position = interpolatedPosition;

    Vector3 nextPoint = _csvPositions[segmentIndex + 1];

    Vector3 directionToNextPoint = _csvVelocities[segmentIndex].normalized;
    _mesh.rotation = Quaternion.LookRotation(directionToNextPoint, Vector3.up); //tip of rocket facing forward

    _totalDistance = 0;
    for (int i = 0; i < segmentIndex; i++)
    {
      _totalDistance += Vector3.Distance(_csvPositions[i], _csvPositions[i + 1]);
    }

    float segmentDistance = Vector3.Distance(_csvPositions[segmentIndex], _csvPositions[segmentIndex + 1]);
    _totalDistance += segmentDistance * segmentProgress;

    _totalDist.text = $"{_totalDistance * (1 / _parser.INITIAL_SCALE)} km";

    int budgetSegmentIndex = FindMinuteBasedSegment(_rocketSimTime / 60);
    //    print(_parser.minTimes[budgetSegmentIndex]);
    //print($"{_parser.WpsaStates[budgetSegmentIndex]} {_parser.DS54States[budgetSegmentIndex]} {_parser.DS24States[budgetSegmentIndex]} {_parser.DS34States[budgetSegmentIndex]}");
    Dictionary<string, double> rangeSatelliteMatches = new Dictionary<string, double>
    {
      //inssheet 0 means off 1 on, so set range t -1 if off in sheet
      {"WPSA", _parser.WpsaStates[budgetSegmentIndex] == 1 ? LinkBudget(12, _parser.WpsaRanges[budgetSegmentIndex]) : -1},
      {"DS54", _parser.DS54States[budgetSegmentIndex] == 1 ? LinkBudget(34, _parser.DS54Ranges[budgetSegmentIndex]) : -1},
      {"DS24", _parser.DS24States[budgetSegmentIndex] == 1 ? LinkBudget(34, _parser.DS24Ranges[budgetSegmentIndex]) : -1},
      {"DS34", _parser.DS34States[budgetSegmentIndex] == 1 ? LinkBudget(34, _parser.DS34Ranges[budgetSegmentIndex]) : -1},
    };

    IOrderedEnumerable<KeyValuePair<string, double>> sorted = rangeSatelliteMatches.OrderBy(kvp => kvp.Value);
    IEnumerable<KeyValuePair<string, double>> reversed = sorted.Reverse();
    string allLinkBudget = "Antenna Availability (most optimal to least optimal)";
    int numAvail = 0;
    foreach (KeyValuePair<string, double> kvp in reversed)
    {
      if (kvp.Value != -1)
      {
        numAvail += 1;
        allLinkBudget += $"\n{kvp.Key}: {Math.Round(kvp.Value, 4)} kbps";
      }
      else { allLinkBudget += $"\n{kvp.Key}: OFF"; }
    }
    _numberAvailable.text = $"Antennas Available: {numAvail}";
    _numberAvailable.color = antennaColors[numAvail];
    _linkBudget.text = allLinkBudget;

  }

  private int FindPreciseSegment(float time)
  {
    for (int i = 0; i < _csvTimes.Count - 1; i++)
    {
      if (time >= _csvTimes[i] && time <= _csvTimes[i + 1])
        return i;
    }
    return _csvTimes.Count - 2;
  }

  private int FindMinuteBasedSegment(float time)
  {
    for (int i = 0; i < _parser.minTimes.Count - 1; i++)
    {
      if (time >= _parser.minTimes[i] && time <= _parser.minTimes[i + 1])
        return i;
    }
    return _parser.minTimes.Count - 2;
  }

  private void OnDrawGizmos()
  {
    Gizmos.color = Color.red;
    foreach (Transform s in _satellites)
    {
      Gizmos.DrawLine(transform.position, s.position);
    }
    //draw path in unity editor window
    Gizmos.color = Color.green;
    if (_csvPositions != null && _csvPositions.Count > 1)
    {
      for (int i = 0; i < _csvPositions.Count - 1; i++)
      {
        Gizmos.DrawLine(_csvPositions[i], _csvPositions[i + 1]);
      }
    }
  }

  private double LinkBudget(double dr, double R)
  {
    double Pt = 10.0;
    double Gt = 9.0;
    double L = 19.430;
    double nR = 0.550;
    double W = 0.1363636360;
    double kb = -228.60;
    double Ts = 22.0;

    if (double.IsNaN(R))
    {
      return float.NaN;
    }

    double term1 = Pt + Gt - L;
    double term2 = 10 * Math.Log10(nR * Math.Pow((Math.PI * dr / W), 2));
    double term3 = -20 * Math.Log10((4000 * Math.PI * R) / W);
    double term4 = -kb - 10 * Math.Log10(Ts);

    double Bn = Math.Pow(10, ((term1 + term2 + term3 + term4) * 0.1)) / 1000;

    return Math.Min((float)Bn, 10000);
  }
}

