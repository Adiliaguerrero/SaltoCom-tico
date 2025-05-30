using UnityEngine;
using UnityEngine.EventSystems;

public class ComaDrag : MonoBehaviour, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector3 originalPosition;
    private bool yaClonada = false;  

    public RectTransform[] dropAreas;
    public Transform padreContenedor;  

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        originalPosition = transform.position;

        if (padreContenedor == null)
            padreContenedor = transform.parent;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.position = eventData.position;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        bool colocado = false;

        foreach (var area in dropAreas)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(area, eventData.position, eventData.pressEventCamera))
            {
                rectTransform.position = area.position;
                colocado = true;

             if (!yaClonada)
{
    GameObject clon = Instantiate(gameObject, padreContenedor);
    clon.transform.position = originalPosition;
    clon.transform.localScale = Vector3.one;

    var clonScript = clon.GetComponent<ComaDrag>();
    clonScript.originalPosition = clon.transform.position;  
    clonScript.yaClonada = false;

    yaClonada = true;
}


                break;
            }
        }

        if (!colocado)
        {
            rectTransform.position = originalPosition;
        }
    }
}
