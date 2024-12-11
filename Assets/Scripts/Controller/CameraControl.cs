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
    [SerializeField] float scrollSpeed;
    [SerializeField] float cameraSensitivity;

    [SerializeField] private Camera cam;
    [SerializeField] private Transform velShip;

    [SerializeField] private MinimapControls minimap;

    private Vector3 anchorPoint;
    private Quaternion anchorRot;
    private Vector3 prevMousePosition;
    [SerializeField] private TextMeshProUGUI povText;

    private bool canMove = true;

    void Start()
    {
        povText.text = "TRACKING ORION";

    }

    void Update()
    {

        povText.text = "";

        if (transform.parent != null)
        {
            povText.text = "TRACKING ORION";
        }
        else
        {
            povText.text = "FREE";

        }

        if (Input.GetKeyDown(KeyCode.H) && velShip != null)
        {
            if (transform.parent == null)
            {
                transform.SetParent(velShip);
                transform.position = velShip.position;
                povText.text = "TRACKING ORION";

            }
            else
            {
                transform.SetParent(null);
                povText.text = "FREE";

            }
        }
        float p = 1;
        if (Input.GetKeyDown(KeyCode.T))
        {
            canMove = !canMove;
            Debug.Log($"Can Move: {canMove}");
        }

        if (!canMove)
        {
            povText.text += (" (Locked)");
            p = 0;
        }



        Vector3 move = Vector3.zero;

        float speed = movementSpeed * (Input.GetKey(KeyCode.LeftShift) ? sprintMultiplier : 1f) * Time.deltaTime * 9.1f * p;
        move += Vector3.forward * (!minimap.hover ? Input.mouseScrollDelta.y : 0) * scrollSpeed;

        if (Input.GetKey(KeyCode.W) /*|| Input.GetKey(KeyCode.UpArrow)*/)
        {
            move += Vector3.forward * speed;
        }
        if (Input.GetKey(KeyCode.S) /*|| Input.GetKey(KeyCode.DownArrow) */)
        {
            move -= Vector3.forward * speed;
        }
        if (Input.GetKey(KeyCode.D) /*|| Input.GetKey(KeyCode.RightArrow) */)
        {
            move += Vector3.right * speed;
        }
        if (Input.GetKey(KeyCode.A) /*|| Input.GetKey(KeyCode.LeftArrow)*/)
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
            Vector2 mov = (((prevMousePosition - Input.mousePosition))) * Time.deltaTime * 0.75f * p;
            move += new Vector3(Mathf.Clamp(mov.x, -10, 10), Mathf.Clamp(mov.y, -10, 10), 0);
            prevMousePosition = Input.mousePosition;

        }


        transform.Translate(move);


    }
}