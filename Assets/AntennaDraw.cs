using Unity.VisualScripting;
using UnityEngine;

public class AntennaDraw : MonoBehaviour
{

    private Transform player;
    private LineRenderer lineRenderer;

    void Start()
    {

        player = GameObject.FindGameObjectWithTag("Rocket").transform;
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
        }

        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default")); // Basic material
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;
        lineRenderer.positionCount = 2;
    }

    void Update()
    {


        //lineRenderer.SetPosition(0, transform.position);
        //lineRenderer.SetPosition(1, player.position);

    }
}
