using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using UnityEngine.UI;


public class RocketMovement : MonoBehaviour
{
  private List<Vector3> _csvPositions;
  private List<Vector3> _csvVelocities;
  [SerializeField] public Transform offnomRocket;
  [SerializeField] private OffNomLineManager offnomManager;

  private List<float> _csvTimes;
  private List<float> _csvMinTimes;
  private AppManager _manager;
  private CSV_Parser _parser;
  [SerializeField] private Transform _mesh;
  private float _rocketSimTime;
  [SerializeField] private TextMeshProUGUI _totalDist;
  [SerializeField] private TextMeshProUGUI _linkBudget;

  [SerializeField][ReadOnlyField] private float _totalDistance = 0;
  public Vector3 _calculatedVelocity { get; private set; } = Vector3.zero;
  [SerializeField] private List<Transform> _satellites;
  [SerializeField] private List<AntennaDraw> _satelliteData;
  private List<string> active;
  [SerializeField] private LayerMask _ignoreSatelliteLayers;
  [SerializeField] private TextMeshProUGUI _positionUI;
  [SerializeField] private TextMeshProUGUI _numberAvailable;
  [SerializeField] private TextMeshProUGUI[] _antennaTexts;
  [SerializeField] private Color[] _antennaColors = new Color[4];

  [SerializeField] private GameObject ICPS;
  [SerializeField] private GameObject serviceModule;
  [SerializeField] private Toggle antennaAvailability;
  [SerializeField] private TextMeshProUGUI firingText;



  private void Awake()
  {
    _manager = GameObject.FindGameObjectWithTag("AppManager").GetComponent<AppManager>();
    _parser = _manager.gameObject.GetComponent<CSV_Parser>();
  }

  private void Start()
  {
    active = new List<string>(_parser.ACTIVE);
    _csvPositions = new List<Vector3>(_parser.Positions);
    _csvVelocities = new List<Vector3>(_parser.Velocities);
    _csvTimes = new List<float>(_parser.Times);
    _csvMinTimes = new List<float>(_parser.offMinTimes);

    for (int i = 0; i < _csvTimes.Count; i++)
    {
      _csvTimes[i] *= 60; // manually edit the denomintaor of power to amount of objects using this system of interpolated positions
    }

    for (int i = 0; i < _csvMinTimes.Count; i++)
    {
      _csvMinTimes[i] *= 60;
    }

    transform.position = _csvPositions[0];

    foreach (Transform s in _satellites)
    {
      _satelliteData.Add(s.GetComponent<AntennaDraw>());
    }



  }

  private void FixedUpdate()
  {


    _rocketSimTime = (float)_manager.simulationTime;
    if (_rocketSimTime < 0)
    {
      _rocketSimTime = 0;
    }
    int offnomIndex = FindOffMinuteBasedSegment(_rocketSimTime);
    print(offnomManager.csvParser.OffNomPos[offnomIndex].x);
    float offNomSegmentProgress = (_rocketSimTime - _csvMinTimes[offnomIndex]) / (_csvMinTimes[offnomIndex + 1] - _csvMinTimes[offnomIndex]);
    Vector3 offNomInterpolatedPosition = Vector3.Lerp(offnomManager.csvParser.OffNomPos[offnomIndex], offnomManager.csvParser.OffNomPos[offnomIndex + 1], offNomSegmentProgress);
    offnomRocket.position = offNomInterpolatedPosition;
    Vector3 offNextPoint = offnomManager.csvParser.OffNomPos[offnomIndex + (offnomIndex < offnomManager.csvParser.OffNomPos.Count - 3 ? 2 : 1)];
    Vector3 offNomdirectionToNextPoint = offnomIndex < offnomManager.csvParser.OffNomPos.Count - 3 ? (offNextPoint - offnomRocket.position).normalized : _csvVelocities[_csvVelocities.Count - 2].normalized;
    offnomRocket.rotation = Quaternion.LookRotation(_csvMinTimes[offnomIndex] / 60 < 204f ? (offNextPoint - offnomRocket.position).normalized : offNomdirectionToNextPoint, Vector3.up); //tip of rocket facing forward



    _positionUI.text = $"{transform.position.x * (1 / _parser.INITIAL_SCALE)} km\n{transform.position.z * (1 / _parser.INITIAL_SCALE)} km\n{transform.position.y * (1 / _parser.INITIAL_SCALE)} km";

    int segmentIndex = FindPreciseSegment(_rocketSimTime);
    float segmentProgress = (_rocketSimTime - _csvTimes[segmentIndex]) / (_csvTimes[segmentIndex + 1] - _csvTimes[segmentIndex]);
    Vector3 interpolatedPosition = Vector3.Lerp(_csvPositions[segmentIndex], _csvPositions[segmentIndex + 1], segmentProgress);
    transform.position = interpolatedPosition;

    Vector3 nextPoint = _csvPositions[segmentIndex + (segmentIndex < _csvPositions.Count - 3 ? 2 : 1)];
    int budgetSegmentIndex = FindMinuteBasedSegment(_rocketSimTime / 60);

    Vector3 directionToNextPoint = segmentIndex < _csvPositions.Count - 3 ? (nextPoint - transform.position).normalized : _csvVelocities[_csvVelocities.Count - 2].normalized;
    _mesh.rotation = Quaternion.LookRotation(_csvTimes[segmentIndex] / 60 < 204f ? (nextPoint - transform.position).normalized : directionToNextPoint, Vector3.up); //tip of rocket facing forward

    _totalDistance = 0;
    for (int i = 0; i < segmentIndex; i++)
    {
      _totalDistance += Vector3.Distance(_csvPositions[i], _csvPositions[i + 1]);
    }

    float segmentDistance = Vector3.Distance(_csvPositions[segmentIndex], _csvPositions[segmentIndex + 1]);
    //Debug.Log(segmentDistance);
    _totalDistance += segmentDistance * segmentProgress;
    _totalDistance *= (1 / _parser.INITIAL_SCALE);
    if (_totalDistance > 1111286.9f)
    {
      _totalDistance = 1111286.9f;
    }
    _totalDist.text = $"{_totalDistance} km";

    //    print(_parser.minTimes[budgetSegmentIndex]);
    //print($"{_parser.WpsaStates[budgetSegmentIndex]} {_parser.DS54States[budgetSegmentIndex]} {_parser.DS24States[budgetSegmentIndex]} {_parser.DS34States[budgetSegmentIndex]}");
    print(offnomManager.offnom);
    Dictionary<string, double> rangeSatelliteMatches = !offnomManager.offnom ? new Dictionary<string, double>
    {
      //inssheet 0 means off 1 on, so set range t -1 if off in sheet
      {"WPSA", _parser.WpsaStates[budgetSegmentIndex] == 1 ? LinkBudget(12, _parser.WpsaRanges[budgetSegmentIndex]) : -1},
      {"DS54", _parser.DS54States[budgetSegmentIndex] == 1 ? LinkBudget(34, _parser.DS54Ranges[budgetSegmentIndex]) : -1},
      {"DS24", _parser.DS24States[budgetSegmentIndex] == 1 ? LinkBudget(34, _parser.DS24Ranges[budgetSegmentIndex]) : -1},
      {"DS34", _parser.DS34States[budgetSegmentIndex] == 1 ? LinkBudget(34, _parser.DS34Ranges[budgetSegmentIndex]) : -1},
    } : new Dictionary<string, double>
    {
      //inssheet 0 means off 1 on, so set range t -1 if off in sheet

      // EACH OF THESE INDICES IN SATTELITE DATA IS HARDCODED, I SHOULD FIND BETTER WAY TO DO BUT WORKS FOR NOW
      {"WPSA", _satelliteData[3].valid ? LinkBudget(12,  _satelliteData[3].dist) : -1},
      {"DS54", _satelliteData[2].valid ? LinkBudget(34,  _satelliteData[2].dist) : -1},
      {"DS24", _satelliteData[0].valid ? LinkBudget(34,  _satelliteData[0].dist) : -1},
      {"DS34", _satelliteData[1].valid ? LinkBudget(34,  _satelliteData[1].dist) : -1},
    };

    IOrderedEnumerable<KeyValuePair<string, double>> sorted = rangeSatelliteMatches.OrderBy(kvp => kvp.Value);
    IEnumerable<KeyValuePair<string, double>> reversed = sorted.Reverse();
    if (!_manager.linkBudget)
    {
      Dictionary<string, double> newSat = new Dictionary<string, double>();
      if (active[budgetSegmentIndex] != "NONE")
        newSat.Add(active[budgetSegmentIndex], rangeSatelliteMatches[active[budgetSegmentIndex]]);
      foreach (KeyValuePair<string, double> kvp in reversed)
      {
        if (kvp.Key != active[budgetSegmentIndex])
        {
          newSat.Add(kvp.Key, kvp.Value);
        }
      }
      reversed = newSat;


    }
    //string allLinkBudget = "";
    int numAvail = 0;
    int index = 0;
    foreach (KeyValuePair<string, double> kvp in reversed)
    {

      if (kvp.Value != -1)
      {
        numAvail += 1;
        // allLinkBudget += $"{kvp.Key}: {Math.Round(kvp.Value, 4)} kbps\n"; // old link budget text
        _antennaTexts[index].text = $"{kvp.Key}: {Math.Round(kvp.Value, 4)} kbps";
        _antennaTexts[index].color = antennaAvailability.isOn ? _antennaColors[index] : Color.white;
        /*
        for (int i = 0; i < _satelliteLines.Count; i++)
        {
          //print(_satelliteLines[i].gameObject.name + " " + kvp.Key + " " + _satelliteLines[i].gameObject.name.Equals(kvp.Key));
          if (_satelliteLines[i].gameObject.name.Equals(kvp.Key))
          {
            _satelliteLines[i].enabled = true;
            _satelliteLines[i].startColor = _antennaColors[index];
            _satelliteLines[i].endColor = _antennaColors[index];
            _satelliteLines[i].SetPosition(0, _satelliteLines[i].transform.position);
            _satelliteLines[i].SetPosition(1, transform.position);

          }
          else
          {

          }
        }
        */
      }
      else { _antennaTexts[index].text = $"{kvp.Key}: OFF\n"; _antennaTexts[index].color = antennaAvailability.isOn ? _antennaColors[3] : Color.white; }
      index++;

    }


    firingText.text = "";
    double rocket_mins = _rocketSimTime / 60;
    // in progress 
    if (rocket_mins > 47.639 && rocket_mins < 48.639)
    {
      print("Perigee Raise Manuevar");
      firingText.text = "BURN ICPS";

    }
    else if (rocket_mins > 99.639 && rocket_mins < 121.0944)
    {
      print("Apogee Raise Burn");
      firingText.text = "BURN ICPS";

    }
    else if (rocket_mins > 196.09447 && rocket_mins < 196.64947)
    {
      print("ICPS Detach abd burn");
      firingText.text = "ICPS SEPERATING (SECOND STAGE)";
    }
    else if (rocket_mins > 283.64947 && rocket_mins < 284.6428)
    {
      print("Seperation Burn");
      firingText.text = "BURN ICPS";

    }
    else if (rocket_mins > 792.44937 && rocket_mins < 795.28272)
    {
      print("Perigree Raise Burn");
      firingText.text = "BURN SERVICE MODULE";

    }
    else if (rocket_mins > 1486.6203 && rocket_mins < 1492.2767)
    {
      print("Escape Orbit");
      firingText.text = " 196 SERVICE MODULE";

    }

    bool perigeeMDone = false;
    bool apogee = false;
    bool icpsBreak = false;
    bool sepBur = false;
    bool perigBurn = false;
    bool escapeOrbit = false;
    bool serviceOff = false;
    // finished 
    if (rocket_mins > 48.639)
    {
      perigeeMDone = true;
      print("Perigee Raise Done");
    }
    if (rocket_mins > 121.0944)
    {
      apogee = true;
      print("Apogee Raise Burn Done");

    }
    if (rocket_mins > 196.64947)
    {
      icpsBreak = true;

      print("ICPS Detach and burn");
    }
    if (rocket_mins > 284.6428)
    {
      sepBur = true;
      print("Seperation Burn done");
    }
    if (rocket_mins > 795.28272)
    {
      perigBurn = true;
      print("Perigree Raise Burn Done");
    }
    if (rocket_mins > 1486.6203 && rocket_mins < 1492.2767)
    {
      escapeOrbit = true;
      print("Escape Orbit");
    }

    if (rocket_mins > 12969.95)
    {
      serviceOff = true;
    }
    ICPS.SetActive(!icpsBreak);
    serviceModule.SetActive(!serviceOff);

    //allLinkBudget = $"Antennas Available: {numAvail}\n" + allLinkBudget;
    _numberAvailable.text = $"Antennas Available: {numAvail}";


  }

  private int FindPreciseSegment(float time)
  {
    if (time == 0)
    {
      return 0;
    }

    for (int i = 0; i < _csvTimes.Count - 2; i++)
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

  private int FindOffMinuteBasedSegment(float time)
  {
    if (time == 0)
    {
      return 0;
    }

    for (int i = 0; i < _csvMinTimes.Count - 1; i++)
    {
      if (time >= _csvMinTimes[i] && time <= _csvMinTimes[i + 1])
        return i;
    }
    return _csvMinTimes.Count - 2;
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

