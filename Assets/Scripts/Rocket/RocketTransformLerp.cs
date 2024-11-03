using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketTransformLerp : MonoBehaviour
{

    private Vector3 targetPos;
    [SerializeField] private int index = 0;

    private float velocityX;
    private float velocityY;
    private float velocityZ;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float pointUpdateDistanceThreshold;

    [SerializeField] private PointManager pointManager;

    [SerializeField] private LineManager pathLineManager;
    [SerializeField] private LineManager trackingLineManager;

    private void Start()
    {
        index = 0;
        targetPos = pointManager.positionPoints[index];

        velocityX = pointManager.velocityPoints[index].x;
        velocityY = pointManager.velocityPoints[index].y;
        velocityZ = pointManager.velocityPoints[index].z;
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
            transform.position = new Vector3(                       Mathf.Lerp(transform.position.x, targetPos.x, (velocityX * Time.deltaTime) 
                / Vector3.Distance(transform.position, targetPos)), Mathf.Lerp(transform.position.y, targetPos.y, (velocityY * Time.deltaTime)
                / Vector3.Distance(transform.position, targetPos)), Mathf.Lerp(transform.position.z, targetPos.z, (velocityZ * Time.deltaTime)
                / Vector3.Distance(transform.position, targetPos)));
            if (Vector3.Distance(transform.position, targetPos) <= pointUpdateDistanceThreshold)
            {
                SetPoint();
            }
        }
        else if (index < (pointManager.points.Length - 1))
        {
            Debug.LogWarning("Distance to the point is 0- cannot divide speed by the distance if it is 0 so running code without speed compensation");
            transform.position = Vector3.Lerp(transform.position, targetPos, (velocityX * Time.deltaTime)/* / Vector3.Distance(transform.position, targetPos)*/);
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
            targetPos = pointManager.positionPoints[index];
            velocityX = pointManager.velocityPoints[index].x;
            velocityY = pointManager.velocityPoints[index].y;
            velocityZ = pointManager.velocityPoints[index].z;
        }
    }
}
