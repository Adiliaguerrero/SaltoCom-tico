// Importa el espacio de nombres principal de Unity (acceso a componentes como Transform, GameObject, etc.)
using UnityEngine;

// Importa las interfaces para manejo de eventos del sistema UI (como arrastrar y soltar)
using UnityEngine.EventSystems;

// Esta clase permite arrastrar un objeto UI, soltarlo en áreas específicas y clonar el objeto si es soltado correctamente
public class ComaDrag : MonoBehaviour, IDragHandler, IEndDragHandler
{
    // Referencia al componente RectTransform del objeto (necesario para mover objetos UI)
    private RectTransform rectTransform;

    // Grupo de configuración de unity para controlar el raycasts (esto ara permitir o bloquear clics/interacciones)
    private CanvasGroup canvasGroup;

    // Guarda la posición original del objeto antes de arrastrarlo
    private Vector3 originalPosition;

    // Evita que el objeto se clone más de una vez al ser soltado 
    private bool yaClonada = false;

    // Áreas o zonas válidas donde  las comas podran ser soltadas correc3ctamente
    public RectTransform[] dropAreas;

    // Contenedor que servira como obejto padre donde se instanciarán o crearan mas comas
    public Transform padreContenedor;

    // Método Start se ejecuta una vez al comenzar la escena o al activarse el objeto
    void Start()
    {
        // Se obtiene el componente RectTransform del objeto actual
        rectTransform = GetComponent<RectTransform>();

        // Se obtiene el CanvasGroup para controlar si bloquea raycasts (clics)
        canvasGroup = GetComponent<CanvasGroup>();

        // Guarda la posición original del objeto para poder volver si no se suelta la coma en una area valida
        originalPosition = transform.position;

        // Si no se ha asignado un contenedor , se toma el padre actual
        if (padreContenedor == null)
            padreContenedor = transform.parent;
    }

    // Método OnDrag se ejecuta mientras se esta arrastrando la coma (requerido por IDragHandler)
    public void OnDrag(PointerEventData eventData)
    {
        // Actualiza la posición del objeto al seguir la posición del puntero o dedo en pantalla
        rectTransform.position = eventData.position;

        // Desactiva temporalmente los raycasts para que el objeto no interfiera con detección de áreas debajo
        canvasGroup.blocksRaycasts = false;
    }

    //  Método OnEndDrag se ejecuta cuando se suelta el objeto coma después de arrastrarlo (requerido por IEndDragHandler)
    public void OnEndDrag(PointerEventData eventData)
    {
        // Vuelve a activar los raycasts para que el objeto vuelva a responder a interacciones
        canvasGroup.blocksRaycasts = true;

        // Bandera que indica si el objeto fue soltado en una zona válida
        bool colocado = false;

        // Recorre cada zona de soltado en las areas válida
        foreach (var area in dropAreas)
        {
            // Verifica si el objeto fue soltado dentro de una posicion  de las zonas permitidas
            if (RectTransformUtility.RectangleContainsScreenPoint(area, eventData.position, eventData.pressEventCamera))
            {
                // Coloca el objeto justo en el centro de la zona de destino
                rectTransform.position = area.position;

                // Marca como colocado exitosamente
                colocado = true;

                // Si aún no se ha clonado el objeto:
                if (!yaClonada)
                {
                    // Crea una copia del objeto actual dentro del mismo contenedor padre
                    GameObject clon = Instantiate(gameObject, padreContenedor);

                    // Coloca el clon en la posición original
                    clon.transform.position = originalPosition;

                    // Asegura que la escala sea 1 para evitar distorsión visual
                    clon.transform.localScale = Vector3.one;

                    // Accede al script del clon para actualizar sus propiedades internas
                    var clonScript = clon.GetComponent<ComaDrag>();
                    clonScript.originalPosition = clon.transform.position; // Actualiza su nueva posición original
                    clonScript.yaClonada = false; // Permite que el nuevo clon pueda ser clonado también

                    // Marca el objeto original como ya clonado para no crear más copias
                    yaClonada = true;
                }

                // Ya se colocó correctamente, salimos del ciclo con un breack
                break;
            }
        }

        // Si el objeto no se soltó en una zona válida, regresa a su posición inicial
        if (!colocado)
        {
            rectTransform.position = originalPosition;
        }
    }
}
