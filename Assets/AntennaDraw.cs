using Unity.VisualScripting;
using UnityEngine;

public class AntennaDraw : MonoBehaviour
{

    private Transform player;
    private LineRenderer lineRenderer;
    public bool valid = false;
    [SerializeField] private LayerMask celestialbodies;
    public float dist { get; private set; } = 0;
    private CSV_Parser parser;

    void Start()
    {
        parser = GameObject.FindGameObjectWithTag("AppManager").GetComponent<CSV_Parser>();

        player = GameObject.FindGameObjectWithTag("Rocket").GetComponent<RocketMovement>().offnomRocket;
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

        RaycastHit hit;
        if (Physics.Raycast(transform.position, (player.position - transform.position).normalized, out hit, 1000000f, celestialbodies))
        {
            Debug.Log(name + " hit " + hit.collider.name);
            valid = false;
        }
        else
        {
            Debug.Log(name + " did not hit anything.");
            valid = true;
            dist = Vector3.Distance(transform.position * (1 / parser.INITIAL_SCALE), player.position * (1 / parser.INITIAL_SCALE));
        }        //lineRenderer.SetPosition(0, transform.position);
        //lineRenderer.SetPosition(1, player.position);

    }
}
