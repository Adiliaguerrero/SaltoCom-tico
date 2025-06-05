// Importamos librerías básicas de Unity, UI y TextMeshPro
using UnityEngine;
using TMPro;                   // Para manejar textos con TextMeshPro
using UnityEngine.UI;          // Para componentes UI, como botones

// Clase para manejar una pregunta de opción única 
public class OpcionUnica2 : MonoBehaviour
{
    // Referencia al controlador de paneles para avanzar entre preguntas o escenas
    public Paneles1 Paneles1;

    // Texto donde se muestra la pregunta (TextMeshPro)
    public TMP_Text preguntaTexto;

    // Array de botones para las opciones de respuesta
    public Button[] botonesOpciones;

    // Texto donde se muestra la retroalimentación ("Correcto", "Incorrecto", etc.)
    public TextMeshProUGUI retroalimentacionTexto;

    // Clip de sonido que se reproduce al pulsar un botón, asignado desde Inspector
    public AudioClip sonidoBoton;

    // Texto de la pregunta que se mostrará al usuario (privado porque no es necesario en Inspector)
    private string pregunta = "¿Dónde se debería colocar la primera coma?\n\"Compré manzanas peras uvas y plátanos.\"";

    // Array con las opciones de respuesta disponibles para el usuario
    private string[] opciones = {
        "A.Después de 'compré'.",
        "B.Después de 'manzanas' y 'peras'.",
        "C.Después de 'uvas'.",
        "D.Después de 'peras' y 'uvas'."
    };

    // Índice de la opción correcta (aquí la opción correcta es la 1)
    private int indiceRespuestaCorrecta = 1;

    // Método llamado al iniciar el script
    void Start()
    {
        // Asignamos el texto de la pregunta al componente correspondiente
        preguntaTexto.text = pregunta;

        // Limpiamos el texto de retroalimentación para que empiece vacío
        retroalimentacionTexto.text = "";

        // Recorremos todos los botones para asignarles el texto de las opciones correspondientes
        for (int i = 0; i < botonesOpciones.Length; i++)
        {
            // Obtenemos el componente TextMeshProUGUI que está dentro del botón
            TextMeshProUGUI textoBoton = botonesOpciones[i].GetComponentInChildren<TextMeshProUGUI>();
            if (textoBoton != null)
            {
                // Asignamos el texto de la opción correspondiente
                textoBoton.text = opciones[i];
            }
        }
    }

    // Métodos públicos para cada opción, que llaman a la función que verifica la respuesta con el índice adecuado
    public void SeleccionarOpcion0() => VerificarRespuesta(0);
    public void SeleccionarOpcion1() => VerificarRespuesta(1);
    public void SeleccionarOpcion2() => VerificarRespuesta(2);
    public void SeleccionarOpcion3() => VerificarRespuesta(3);

    // Método para verificar si la opción seleccionada es correcta o no
    void VerificarRespuesta(int indiceSeleccionado)
    {
        // Reproducimos el sonido al pulsar el botón
        AudioManager.instancia.ReproducirSonido(sonidoBoton);

        // Comparamos el índice seleccionado con el índice de la respuesta correcta
        if (indiceSeleccionado == indiceRespuestaCorrecta)
        {
            // Si es correcto, mostramos mensaje y aumentamos puntaje correcto
            retroalimentacionTexto.text = "¡Correcto!";
            IncrementarPuntajeIntermedioCorrecto();
        }
        else
        {
            // Si es incorrecto, mostramos mensaje y aumentamos puntaje incorrecto
            retroalimentacionTexto.text = "Incorrecto.";
            IncrementarPuntajeIntermedioIncorrecto();
        }

        // Deshabilitamos la interacción de todos los botones para evitar múltiples respuestas
        foreach (Button b in botonesOpciones)
        {
            b.interactable = false;
        }

        // Llamamos para pasar a la siguiente pregunta o escena después de 2 segundos
        Invoke("PasarASiguienteTrivia", 2f);
    }

    // Método para avanzar a la siguiente trivia usando el controlador Paneles1
    void PasarASiguienteTrivia()
    {
        Paneles1.SiguienteTrivia();
    }

    // Incrementa el contador de respuestas correctas en PlayerPrefs (guardado persistente)
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
