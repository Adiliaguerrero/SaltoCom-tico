using UnityEngine;
using UnityEngine.EventSystems;

    /// <summary>
    /// Permite arrastrar y soltar un objeto coma dentro de zonas válidas. Si se suelta correctamente, se clona una nueva coma.
    /// </summary>

public class ComaDrag : MonoBehaviour, IDragHandler, IEndDragHandler
{
    /// <summary>
    /// Referencia al componente RectTransform del objeto actual.
    /// </summary>
    private RectTransform rectTransform;

    /// <summary>
    /// Componente que controla si el objeto bloquea interacciones de clic mientras se arrastra.
    /// </summary>
    private CanvasGroup canvasGroup;

    /// <summary>
    /// Posición inicial del objeto coma, para restaurarla si no se coloca correctamente.
    /// </summary>
    private Vector3 originalPosition;

    /// <summary>
    /// Evita que el objeto se clone más de una vez al ser soltado 
    /// </summary>
    private bool yaClonada = false;

    /// <summary>
    /// Áreas o zonas válidas donde  las comas podran ser soltadas correc3ctamente
    /// </summary>
    public RectTransform[] dropAreas;

    /// <summary>
    /// Contenedor que servira como obejto padre donde se instanciarán o crearan mas comas
    /// </summary>    
    public Transform padreContenedor;
 
    /// <summary>
    /// Inicializa referencias necesarias y guarda la posición original del objeto.
    /// </summary>
    // Método Start se ejecuta una vez al comenzar la escena o al activarse el objeto
    void Start()
    {
        
        // Se obtiene el componente RectTransform del objeto actual
        rectTransform = GetComponent<RectTransform>();

        // Se obtiene el CanvasGroup para controlar si bloquea raycasts (clics)
        canvasGroup = GetComponent<CanvasGroup>();

        // Guarda la posición original del objeto para poder volver si no se suelta la coma en una area valida
        originalPosition = transform.position;

        if (padreContenedor == null)
            padreContenedor = transform.parent;
    }


        /// <summary>
        /// Mueve el objeto siguiendo el puntero mientras se arrastra.
        /// </summary>
        /// <param name="eventData">Datos del evento de arrastre.
        /// </param>
    public void OnDrag(PointerEventData eventData)
    {
        // Actualiza la posición del objeto al seguir la posición del puntero o dedo en pantalla
        rectTransform.position = eventData.position;

        // Desactiva temporalmente los raycasts para que el objeto no interfiera con detección de áreas debajo
        canvasGroup.blocksRaycasts = false;
    }

        /// <summary>
        /// Se ejecuta al soltar el objeto. Verifica si fue colocado en una zona válida y, si es así, clona una nueva coma.
        /// </summary>
        /// <param name="eventData">Datos del evento al soltar el objeto.
        /// </param>
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
                    clonScript.originalPosition = clon.transform.position; // Actualiza su nueva posición original
                    clonScript.yaClonada = false; // Permite que el nuevo clon pueda ser clonado también

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
