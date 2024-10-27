using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketTransformLerp : MonoBehaviour
{

    private Vector3 targetPos;
    private int index = 0;

    [SerializeField] private float movementSpeed;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float pointUpdateDistanceThreshold;

    [SerializeField] private PointManager pointManager;

    [SerializeField] private LineManager pathLineManager;
    [SerializeField] private LineManager trackingLineManager;

    private void Start()
    {
        index = 0;
        targetPos = pointManager.points[index];
    }

    void Update()
    {
        UpdatePos();
        UpdateRot();
    }

    public void UpdatePos()
    {
        if (Vector3.Distance(transform.position, targetPos) != 0)
        {
            transform.position = Vector3.Lerp(transform.position, targetPos, (movementSpeed * Time.deltaTime) / Vector3.Distance(transform.position, targetPos));
            if (Vector3.Distance(transform.position, targetPos) <= pointUpdateDistanceThreshold)
            {
                SetPoint();
            }
        }
        else if (index < (pointManager.points.Length - 1))
        {
            Debug.LogWarning("Distance to the point is 0- cannot divide speed by the distance if it is 0 so running code without speed compensation");
            transform.position = Vector3.Lerp(transform.position, targetPos, (movementSpeed * Time.deltaTime) / Vector3.Distance(transform.position, targetPos));
            if (Vector3.Distance(transform.position, targetPos) <= pointUpdateDistanceThreshold)
            {
                SetPoint();
            }
        }

        if (pathLineManager != null)
        {
            pathLineManager.DrawDynamicLine(transform.position);
        }

        if (trackingLineManager != null)
        {
            trackingLineManager.DrawTracker(transform.position);
        }

    }

    public void UpdateRot()
    {
        transform.LookAt(Vector3.Lerp(new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z),
            new Vector3(targetPos.x, targetPos.y, targetPos.z), rotateSpeed * Time.deltaTime));
    }

    public void SetPoint()
    {
        if (index < (pointManager.points.Length - 1))
        {
            index += 1;
            targetPos = pointManager.points[index];
        }
    }
}
