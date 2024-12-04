using UnityEngine;
using UnityEngine.EventSystems;

public class MinimapControls : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private bool hover = false;
    [SerializeField] private Camera minimapCam;
    [SerializeField] private float zoomAmount;

    private void Update()
    {
        if (hover)
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");

            if (scroll > 0f)
            {
                if (minimapCam.orthographicSize - zoomAmount > 0)
                {
                    minimapCam.orthographicSize -= zoomAmount;
                    minimapCam.orthographicSize = Mathf.Max(minimapCam.orthographicSize, 20);


                }
            }
            else if (scroll < 0f)
            {
                minimapCam.orthographicSize += zoomAmount;
                minimapCam.orthographicSize = Mathf.Min(minimapCam.orthographicSize, 5000);
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hover = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hover = false;
    }
}
