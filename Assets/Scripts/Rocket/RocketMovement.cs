using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.UIElements;

public class RocketMovement : MonoBehaviour
{

    [SerializeField][ReadOnlyField] private Vector3 _velocity;
    [SerializeField][ReadOnlyField] private Vector3 _positions;
    [SerializeField] private float _timeBetween = 1;
    private int _i = 0;
    private Rigidbody _rb;


    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.mass = 60129.7f;
    }

    void Update()
    {

        _rb.velocity = _velocity;

        _timeBetween -= Time.deltaTime;
        if (_timeBetween <= 0)
        {
            _timeBetween = 0f;
            //SetVelocity(CSV_Parser.Velocities[_i]);
            SetPosition(CSV_Parser.Positions[_i]);
            Debug.Log($"{_i}, {CSV_Parser.Velocities.Count}");
            _i++;
        }
    }

    public void SetVelocity(Vector3 velocity)
    {
        _velocity = velocity; // * 1000 as the datasheet uses km, not m
    }

    public void SetPosition(Vector3 position)
    {
        _rb.position = new Vector3(position.x, position.z, position.y) / 1000;
    }
}
