using UnityEngine;
using TMPro;


    /// <summary>
    /// Controla la verificación de comas colocadas correctamente en un texto usando espacios válidos.
    /// </summary>
public class TextoConComasTMP : MonoBehaviour
{
    // Referencia al componente TextMeshProUGUI que contiene el texto con espacios para comas
    public TextMeshProUGUI textoTMP;

    // Texto para mostrar retroalimentación al usuario (correcto o incorrecto)
    public TextMeshProUGUI textoRetroalimentacion;

    // Array con los espacios (posiciones) válidos donde se deben colocar las comas (RectTransform de UI)
    public RectTransform[] espaciosValidos;

    // Referencia para manejar el avance entre preguntas o escenas
    public Paneles1 Paneles1;

    // Audio que se reproduce al verificar las comas
    public AudioClip sonidoBoton;

    // Texto original que contiene espacios donde irán las comas (representados con [   ])
    private string textoOriginal = "Compré plátanos [   ] manzanas [   ] uvas [   ] peras y pan.";

    // Método llamado al iniciar el script
    void Start()
    {
        // Mostramos el texto original con los espacios para las comas
        textoTMP.text = textoOriginal;

        // Si el texto de retroalimentación está asignado, lo ocultamos al iniciar
        if (textoRetroalimentacion != null)
            textoRetroalimentacion.gameObject.SetActive(false);
    }

    // Método que verifica si las comas han sido colocadas correctamente en todos los espacios válidos
    public void VerificarComas()
    {
        // Reproducimos el sonido al verificar
        AudioManager.instancia.ReproducirSonido(sonidoBoton);

        // Contador de espacios donde se haya detectado una coma correctamente posicionada
        int espaciosOcupados = 0;

        // Buscamos todos los objetos que tengan el componente ComaDrag (las comas que se arrastran)
        ComaDrag[] todasLasComas = FindObjectsOfType<ComaDrag>();

        // Recorremos cada espacio válido para verificar si alguna coma está suficientemente cerca
        foreach (var espacio in espaciosValidos)
        {
            foreach (var coma in todasLasComas)
            {
                // Si la distancia entre la coma y el espacio es menor a 0.1 unidades, consideramos que la coma está en el lugar correcto
                if (Vector3.Distance(coma.transform.position, espacio.position) < 0.1f)
                {
                    espaciosOcupados++;
                    break;  // Salimos del loop para la siguiente posición válida, porque ya encontramos una coma aquí
                }
            }
        }

        // Si el texto de retroalimentación está asignado, lo mostramos
        if (textoRetroalimentacion != null)
            textoRetroalimentacion.gameObject.SetActive(true);

        // Verificamos si el número de comas colocadas coincide con la cantidad de espacios válidos
        if (espaciosOcupados == espaciosValidos.Length)
        {
            // Mensaje en consola para debug (puedes eliminarlo si quieres)
            Debug.Log("¡Comas correctamente colocadas!");

            // Actualizamos el texto para mostrarlo con las comas correctas ya colocadas
            textoTMP.text = "Compré plátano, manzana, uvas, peras y pan.";

            // Mostramos retroalimentación positiva
            if (textoRetroalimentacion != null)
                textoRetroalimentacion.text = "¡Muy bien! Has colocado correctamente las comas.";

            // Incrementamos puntaje de respuestas correctas
            IncrementarPuntajeIntermedioCorrecto();
        }
        else
        {
            // Si no están todas las comas bien colocadas, mostramos retroalimentación negativa
            textoRetroalimentacion.text = "Incorrecto. No has colocado todas las comas correctamente.";

            // Incrementamos puntaje de respuestas incorrectas
            IncrementarPuntajeIntermedioIncorrecto();
        }

        // Eliminamos todas las comas (objetos ComaDrag) de la escena para evitar que queden tras la verificación
        foreach (var coma in todasLasComas)
        {
            Destroy(coma.gameObject);
        }

        // Llamamos para pasar a la siguiente pregunta o escena después de 2 segundos
        Invoke("PasarASiguienteTrivia", 2f);
    }

    // Método para avanzar a la siguiente trivia o escena usando el controlador Paneles1
    void PasarASiguienteTrivia()
    {
        if (Paneles1 != null)
        {
            Paneles1.SiguienteTrivia();
        }
    }

    // Incrementa el contador de respuestas correctas en PlayerPrefs (persistencia de datos)
    void IncrementarPuntajeIntermedioCorrecto()
    {
        int puntajeCorrectoIntermedio = PlayerPrefs.GetInt("PuntajeCorrectoIntermedio", 0);
        PlayerPrefs.SetInt("PuntajeCorrectoIntermedio", ++puntajeCorrectoIntermedio);
    }

    // Incrementa el contador de respuestas incorrectas en PlayerPrefs
    void IncrementarPuntajeIntermedioIncorrecto()
    {
        int puntajeIncorrectoIntermedio = PlayerPrefs.GetInt("PuntajeIncorrectoIntermedio", 0);
        PlayerPrefs.SetInt("PuntajeIncorrectoIntermedio", ++puntajeIncorrectoIntermedio);
    }
}
