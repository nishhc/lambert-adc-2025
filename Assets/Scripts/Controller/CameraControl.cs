using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//test pull request
public class CameraControl : MonoBehaviour
{
    [SerializeField] float movementSpeed;
    [SerializeField] float sprintMultiplier;
    [SerializeField] float cameraSensitivity;

    [SerializeField] private Camera camera;
    private Vector3 anchorPoint;
    private Quaternion anchorRot;

    /*private void Start()
    {
        camera = GetComponent<Camera>();
    }*/

    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            Vector3 move = Vector3.zero;

            float speed = movementSpeed * (Input.GetKey(KeyCode.LeftShift) ? sprintMultiplier : 1f) * Time.deltaTime * 9.1f;
            if (Input.GetKey(KeyCode.W))
            {
                move += Vector3.forward * speed;
            }
            if (Input.GetKey(KeyCode.S))
            {
                move -= Vector3.forward * speed;
            }
            if (Input.GetKey(KeyCode.D))
            {
                move += Vector3.right * speed;
            }
            if (Input.GetKey(KeyCode.A))
            {
                move -= Vector3.right * speed;
            }
            if (Input.GetKey(KeyCode.E))
            {
                move += Vector3.up * speed;
            }
            if (Input.GetKey(KeyCode.Q))
            {
                move -= Vector3.up * speed;
            }
                
            transform.Translate(move);
        }

        if (Input.GetMouseButtonDown(1))
        {
            anchorPoint = new Vector3(Input.mousePosition.y, -Input.mousePosition.x);
            anchorRot = transform.rotation;
            Cursor.lockState = CursorLockMode.Confined;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
        if (Input.GetMouseButton(1))
        {
            Quaternion rot = anchorRot;

            Vector3 dif = anchorPoint - new Vector3(Input.mousePosition.y, -Input.mousePosition.x);
            rot.eulerAngles += dif * cameraSensitivity;
            transform.rotation = rot;
        }
    }
}
