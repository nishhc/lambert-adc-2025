using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class CameraControl : MonoBehaviour
{
    [SerializeField] float movementSpeed;
    [SerializeField] float sprintMultiplier;
    [SerializeField] float cameraSensitivity;

    [SerializeField] private Camera cam;
    [SerializeField] private Transform velShip;
    [SerializeField] private TMP_Text ToggleMovmement;
    [SerializeField] private TMP_Text SnapUnsnap;

    private Vector3 anchorPoint;
    private Quaternion anchorRot;
    private Vector3 prevMousePosition;

    private bool canMove = true;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            canMove = !canMove;
            Debug.Log($"Can Move: {canMove}");
            if (canMove)
                ToggleMovmement.SetText("Disable movement");
            else
            {
                ToggleMovmement.SetText("Enable movement");
            }
        }

        if (Input.GetKeyDown(KeyCode.H) && velShip != null)
        {
            if (transform.parent == null)
            {
                transform.SetParent(velShip);
                SnapUnsnap.SetText("Unsnap from ship");
                transform.position = velShip.position;
            }
            else
            {
                transform.SetParent(null);
                SnapUnsnap.SetText("Snap to ship");
            }
        }


        Vector3 move = Vector3.zero;

        float speed = movementSpeed * (Input.GetKey(KeyCode.LeftShift) ? sprintMultiplier : 1f) * Time.deltaTime * 9.1f;
        move += Vector3.forward * Input.mouseScrollDelta.y * 5;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            move += Vector3.forward * speed;
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            move -= Vector3.forward * speed;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            move += Vector3.right * speed;
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
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
        if (Input.GetMouseButtonDown(2))
        {
            prevMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(2))
        {
            Vector2 mov = (((prevMousePosition - Input.mousePosition))) * Time.deltaTime;
            move += new Vector3(Mathf.Clamp(mov.x, -10, 10), Mathf.Clamp(mov.y, -10, 10), 0);
            prevMousePosition = Input.mousePosition;

        }


        transform.Translate(move, Space.Self);


    }
}