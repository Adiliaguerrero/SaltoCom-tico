using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;


    /// <summary>
    /// Permite al usuario unir visualmente una oración sin comas con su versión correcta (con comas).
    /// Utiliza una línea para representar la conexión entre ambas oraciones.
    /// </summary>
public class UnirFrasesInteractivo : MonoBehaviour
{
    // Referencia al controlador de paneles para avanzar en la trivia o actividades
    public Paneles1 Paneles1;

    // Texto de la frase sin comas que se muestra a la izquierda
    public TMP_Text textoIzquierdo;
    // Array de textos con opciones que contienen comas, mostrados a la derecha
    public TMP_Text[] textosDerechos;
    // Texto donde se mostrará el resultado o retroalimentación al usuario
    public TMP_Text textoResultado;
    // Imagen que representa la línea que se dibuja al unir frases
    public Image imagenLinea;
    // Punto de inicio para la línea cuando se arrastra desde el texto izquierdo
    public Transform puntoInicioManual;
    // Array de puntos de inicio para las opciones de texto derecho (alineación visual)
    public Transform[] puntosDerechos;

    // Variables internas para controlar la interacción de arrastre y unión
    private TMP_Text origenSeleccionado = null;   // Texto desde donde comienza el arrastre
    private TMP_Text destinoSeleccionado = null;  // Texto destino al que se intenta unir
    private bool arrastrando = false;              // Estado de si se está arrastrando la línea
    private bool yaUnido = false;                   // Indica si ya se realizó una unión correcta
    private Vector3 puntoInicio;                    // Posición inicial de la línea en pantalla

    // Frase original sin comas que se debe corregir uniendo con la opción correcta
    private string fraseSinComas = "Mi papá compró plátano manzana uvas peras y pan";
    // Opciones posibles con comas para elegir cuál es la correcta
    private string[] opcionesConComas = new string[] {
        "Mi papá compró plátano, manzana, uvas, peras y pan",
        "Mi papá compró plátano manzana uvas, peras y pan",
        "Mi papá compró plátano, manzana uvas peras, y pan"
    };
    // Índice que indica cuál opción es la correcta dentro del array opcionesConComas
    private int indiceCorrecto = 0;

    void Start()
    {
        // Asignar la frase sin comas al texto izquierdo
        textoIzquierdo.text = fraseSinComas;

        // Asignar cada opción de frase con comas a cada texto derecho correspondiente
        for (int i = 0; i < textosDerechos.Length; i++)
        {
            textosDerechos[i].text = opcionesConComas[i];
        }

        // Ocultar la línea inicialmente, ya que no se ha empezado a unir nada
        imagenLinea.enabled = false;

        // Limpiar texto de resultado o retroalimentación al iniciar
        textoResultado.text = "";
    }

    void Update()
    {
        // Mientras se esté arrastrando, dibujar la línea desde el punto de inicio hasta la posición actual del cursor
        if (arrastrando)
        {
            DibujarLinea(puntoInicio, Input.mousePosition);

            // Cuando se suelta el botón izquierdo del mouse, intentar detectar si se unió con algún texto destino válido
            if (Input.GetMouseButtonUp(0))
            {
                DetectarDestino();
            }
        }
    }

    // Método público que inicia el arrastre cuando se presiona un texto (desde UI)
    public void IniciarArrastreDesdeTexto(GameObject textoGO)
    {
        // Si ya se unió una frase correctamente o si ya está arrastrando, no hacer nada
        if (yaUnido || arrastrando) return;

        // Obtener el componente TMP_Text del objeto presionado
        origenSeleccionado = textoGO.GetComponent<TMP_Text>();

        // Si el texto origen es el texto izquierdo (la frase sin comas) y hay un punto de inicio manual definido
        if (origenSeleccionado == textoIzquierdo && puntoInicioManual != null)
        {
            // Usar la posición del punto manual como inicio de la línea
            puntoInicio = puntoInicioManual.position;
        }
        else
        {
            // Para los textos derechos, buscar el punto de inicio correspondiente
            for (int i = 0; i < textosDerechos.Length; i++)
            {
                // Si el texto origen coincide con uno de los textos derechos y existe un punto derecho definido para ese índice
                if (origenSeleccionado == textosDerechos[i] && i < puntosDerechos.Length && puntosDerechos[i] != null)
                {
                    // Usar la posición del punto derecho como inicio de la línea
                    puntoInicio = puntosDerechos[i].position;
                    break;
                }
                else
                {
                    // Si no hay punto derecho, usar el centro del rectángulo del texto para iniciar la línea
                    puntoInicio = ObtenerCentro(origenSeleccionado.rectTransform);
                }
            }
        }

        // Indicar que se está arrastrando la línea y activar la imagen de la línea para que sea visible
        arrastrando = true;
        imagenLinea.enabled = true;
    }
    // Método que se ejecuta cuando el usuario suelta un objeto arrastrado para detectar si hay un destino válido debajo del mouse
    void DetectarDestino()
    {
        // Se detiene el arrastre
        arrastrando = false;

        // Se detecta si hay un objeto de texto (TMP_Text) debajo del puntero del mouse
        TMP_Text posibleDestino = DetectarTextoDebajoDelMouse();

        // Si se detecta un destino válido y no es el mismo que el origen
        if (posibleDestino != null && posibleDestino != origenSeleccionado)
        {
            // Se establece el destino seleccionado
            destinoSeleccionado = posibleDestino;

            // Se obtiene la posición final donde se debe dibujar la línea
            Vector3 puntoFin = ObtenerPuntoDeDestino(destinoSeleccionado);

            // Se dibuja la línea entre el punto de inicio y el destino
            DibujarLinea(puntoInicio, puntoFin);

            // Se verifica si la respuesta fue correcta o incorrecta
            VerificarRespuesta(destinoSeleccionado);

            // Si el destino es el texto izquierdo y hay una posición de inicio manual definida
            if (destinoSeleccionado == textoIzquierdo && puntoInicioManual != null)
            {
                // Se actualiza el destino visual de la línea
                Vector3 nuevoDestinoVisual = puntoInicioManual.position;
                DibujarLinea(puntoInicio, nuevoDestinoVisual);
            }

            // Se marca que ya se hizo una unión entre origen y destino
            yaUnido = true;
        }
        else
        {
            // Si no hay destino válido, se asegura de que la imagen de la línea siga visible
            imagenLinea.enabled = true;
        }
    }

    // Método que detecta un objeto TMP_Text debajo del mouse usando raycasting de UI
    TMP_Text DetectarTextoDebajoDelMouse()
    {
        // Crea un evento de puntero con la posición actual del mouse
        PointerEventData data = new PointerEventData(EventSystem.current) { position = Input.mousePosition };

        // Lista para almacenar los resultados del raycast
        var resultados = new System.Collections.Generic.List<RaycastResult>();

        // Realiza el raycast a través del sistema de eventos de Unity
        EventSystem.current.RaycastAll(data, resultados);

        // Recorre todos los objetos que fueron impactados por el raycast
        foreach (var r in resultados)
        {
            // Intenta obtener un componente TMP_Text en el objeto impactado
            TMP_Text t = r.gameObject.GetComponent<TMP_Text>();

            // Si encuentra uno, lo devuelve
            if (t != null) return t;
        }

        // Si no se encontró ningún TMP_Text, devuelve null
        return null;
    }

    // Método que verifica si la respuesta seleccionada por el usuario es correcta
    void VerificarRespuesta(TMP_Text destino)
    {
        // Recorre todos los textos del lado derecho para verificar si el destino coincide con alguno
        for (int i = 0; i < textosDerechos.Length; i++)
        {
            // Si el destino coincide con uno de los textos de la derecha
            if (destino == textosDerechos[i])
            {
                // Verifica si el índice del destino coincide con el índice de la respuesta correcta
                if (i == indiceCorrecto)
                {
                    // Si es correcto, muestra un mensaje positivo y suma al contador de respuestas correctas
                    textoResultado.text = "¡Correcto!";
                    IncrementarPuntajeAvanzadoCorrecto();

                    // Espera 2 segundos antes de pasar a la siguiente trivia
                    Invoke("PasarASiguienteTrivia", 2f);
                }
                else
                {
                    // Si no es correcto, muestra un mensaje negativo y suma al contador de respuestas incorrectas
                    textoResultado.text = "Incorrecto.";
                    IncrementarPuntajeAvanzadoIncorrecto();

                    // También espera 2 segundos antes de avanzar
                    Invoke("PasarASiguienteTrivia", 2f);
                }
                return; // Sale del método después de validar una coincidencia
            }
        }

        // Caso adicional: si el usuario arrastra desde el texto correcto hacia el texto izquierdo
        // (posible caso de respuesta inversa permitida)
        if (origenSeleccionado == textosDerechos[indiceCorrecto] && destino == textoIzquierdo)
        {
            textoResultado.text = "¡Correcto!";
            IncrementarPuntajeAvanzadoCorrecto();
            Invoke("PasarASiguienteTrivia", 2f);
        }
        else
        {
            // Si el destino no coincide con el texto correcto, se considera incorrecto
            textoResultado.text = "Incorrecto.";
            IncrementarPuntajeAvanzadoIncorrecto();
            Invoke("PasarASiguienteTrivia", 2f);
        }
    }


    // Método que llama a la función para avanzar a la siguiente trivia desde el controlador de paneles
    void PasarASiguienteTrivia()
    {
        Paneles1.SiguienteTrivia();
    }

    // Método que dibuja una línea entre dos puntos dentro de la interfaz (UI)
    // La línea se ajusta en posición, tamaño y rotación según los puntos dados
    void DibujarLinea(Vector3 inicio, Vector3 fin)
    {
        // Convierte los puntos de mundo a coordenadas locales relativas al contenedor del rectángulo
        Vector3 localInicio = imagenLinea.rectTransform.parent.InverseTransformPoint(inicio);
        Vector3 localFin = imagenLinea.rectTransform.parent.InverseTransformPoint(fin);

        // Calcula el punto medio entre inicio y fin para posicionar la línea
        Vector3 medio = (localInicio + localFin) / 2f;
        imagenLinea.rectTransform.localPosition = medio;

        // Ajusta la longitud de la línea según la distancia entre los dos puntos
        float distancia = Vector3.Distance(localInicio, localFin);
        imagenLinea.rectTransform.sizeDelta = new Vector2(distancia, 5f); // 5f es el grosor de la línea

        // Calcula el ángulo entre los dos puntos y rota la línea para que apunte en esa dirección
        float angulo = Mathf.Atan2(localFin.y - localInicio.y, localFin.x - localInicio.x) * Mathf.Rad2Deg;
        imagenLinea.rectTransform.localRotation = Quaternion.Euler(0, 0, angulo);
    }

    // Método auxiliar que devuelve el centro (promedio de dos esquinas) de un RectTransform en coordenadas de mundo
    Vector3 ObtenerCentro(RectTransform rect)
    {
        Vector3[] corners = new Vector3[4]; // Arreglo para almacenar las esquinas del rectángulo
        rect.GetWorldCorners(corners);     // Obtiene las esquinas en coordenadas de mundo
        return (corners[0] + corners[1]) / 2f; // Calcula el centro superior del rectángulo
    }

    // Método que obtiene la posición exacta donde debe ir la línea de conexión para un destino dado
    Vector3 ObtenerPuntoDeDestino(TMP_Text destino)
    {
        // Recorre los textos de la derecha para buscar coincidencias
        for (int i = 0; i < textosDerechos.Length; i++)
        {
            // Si el destino coincide y tiene un punto asignado, devuelve la posición de ese punto
            if (destino == textosDerechos[i] && i < puntosDerechos.Length && puntosDerechos[i] != null)
            {
                return puntosDerechos[i].position;
            }
        }

        // Si no se encuentra un punto específico, se devuelve el centro del texto como punto de destino
        return ObtenerCentro(destino.rectTransform);
    }

    // Método que incrementa el contador de respuestas correctas en el modo avanzado
    // Guarda el valor actualizado en PlayerPrefs
    void IncrementarPuntajeAvanzadoCorrecto()
    {
        int puntaje = PlayerPrefs.GetInt("PuntajeCorrectoAvanzado", 0); // Obtiene el valor actual
        PlayerPrefs.SetInt("PuntajeCorrectoAvanzado", ++puntaje);       // Lo incrementa y lo guarda
    }

    // Método que incrementa el contador de respuestas incorrectas en el modo avanzado
    // Guarda el valor actualizado en PlayerPrefs
    void IncrementarPuntajeAvanzadoIncorrecto()
    {
        int puntaje = PlayerPrefs.GetInt("PuntajeIncorrectoAvanzado", 0); // Obtiene el valor actual
        PlayerPrefs.SetInt("PuntajeIncorrectoAvanzado", ++puntaje);       // Lo incrementa y lo guarda
    }
}