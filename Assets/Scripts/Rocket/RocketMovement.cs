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
    private LineManager lineManager;

    // Target position and velocity
    private Vector3 _targetPosition;
    private Vector3 _targetVelocity;

    [SerializeField] private bool _directTrack = false;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.mass = 60129.7f; // Set the mass (if necessary)
        _timeBetween = 0;
        _rb.position = CSV_Parser.Positions[_i];
        lineManager = GetComponent<LineManager>();

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
                Debug.Log($"Actual Velocity: {new Vector3(_rb.velocity.x, _rb.velocity.y, _rb.velocity.z)} Point Velocity (from sheet): {CSV_Parser.Velocities[_i]}");
                Debug.Log($"Distance between actual and expected (km): {Vector3.Distance(new Vector3(_rb.velocity.x, _rb.velocity.y, _rb.velocity.z), CSV_Parser.Velocities[_i])}");
                SetTargetPosition(CSV_Parser.Positions[_i]);
                SetTargetVelocity(CSV_Parser.Velocities[_i]);
            }
            else
            {
                _rb.velocity = Vector3.zero;
                _rb.position = CSV_Parser.Positions[_i];
            }
        }
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

        Vector3 proportionalVelocity = positionError.normalized * Mathf.Min(positionError.magnitude, _targetVelocity.magnitude);
        _rb.velocity = proportionalVelocity;

        float dampingFactor = Mathf.Clamp01(positionError.magnitude / 10f);
        _rb.velocity = proportionalVelocity * dampingFactor;

        if (lineManager != null)
        {
            lineManager.DrawDynamicLine(transform.position);
        }

    }
}
