using System;
using UnityEditor.Callbacks;
using UnityEngine;

public class RocketMovement : MonoBehaviour
{
    [SerializeField][ReadOnlyField] private float timer;
    [SerializeField] private float _timeBetween = 1;
    [Range(0.0f, 100f)][SerializeField] private float timeScale = 1f;
    private int _i = 1;
    private Rigidbody _rb;

    // Target position and velocity
    private Vector3 _targetPosition;
    private Vector3 _targetVelocity;

    [SerializeField] private bool _directTrack = false;
    [SerializeField] private GameObject obj;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.mass = 60129.7f; // Set the mass (if necessary)
        _timeBetween = 0;
        _rb.position = CSV_Parser.Positions[_i];

        if (!_directTrack)
        {
            SetTargetPosition(CSV_Parser.Positions[_i]);
            SetTargetVelocity(CSV_Parser.Velocities[_i]);
        }

    }

    void Update()
    {
        Time.timeScale = timeScale;

        timer += Time.deltaTime;
        _timeBetween -= Time.deltaTime / timeScale;

        if (_timeBetween <= 0)
        {
            float _curTimeBtw = _timeBetween;
            _timeBetween = 60f / timeScale + _curTimeBtw;

            _i++;
            if (!_directTrack)
            {
                Debug.Log($"{new Vector3(_rb.velocity.x, _rb.velocity.z, _rb.velocity.y)} {CSV_Parser.Velocities[_i]}");
                Debug.Log(Vector3.Distance(new Vector3(_rb.velocity.x, _rb.velocity.z, _rb.velocity.y), CSV_Parser.Velocities[_i]));
                SetTargetPosition(CSV_Parser.Positions[_i]);
                SetTargetVelocity(CSV_Parser.Velocities[_i]);
            }
            else
            {
                _rb.velocity = Vector3.zero;
                _rb.position = CSV_Parser.Positions[_i];
            }
        }
        Instantiate(obj, transform.position, transform.rotation);
        MoveToTarget();
    }

    private void SetTargetPosition(Vector3 position)
    {
        _targetPosition = position;
    }

    private void SetTargetVelocity(Vector3 velocity)
    {
        _targetVelocity = velocity;
    }

    private void MoveToTarget()
    {
        Vector3 positionError = _targetPosition - _rb.position;

        if (positionError.magnitude < 0.01f)
        {
            _rb.velocity = Vector3.zero;
        }

        Vector3 direction = positionError.normalized;
        _rb.velocity = _targetVelocity;
    }
}
