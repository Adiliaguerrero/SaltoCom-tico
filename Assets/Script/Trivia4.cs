using UnityEngine;
using TMPro;                 
using UnityEngine.UI;

    /// <summary>
    /// Controla una pregunta de opción única con retroalimentación al jugador.
    /// </summary>
public class OpcionUnica2 : MonoBehaviour
{
        /// <summary>
        /// Referencia al controlador de paneles para avanzar entre preguntas o escenas.
        /// </summary>
    public Paneles1 Paneles1;

        /// <summary>
        /// Texto que muestra la pregunta en pantalla.
        /// </summary>
    public TMP_Text preguntaTexto;

        /// <summary>
        /// Botones que representan las diferentes opciones de respuesta.
        /// </summary>
    public Button[] botonesOpciones;

        /// <summary>
        /// Texto que muestra la retroalimentación sobre la respuesta del jugador.
        /// </summary>
    public TextMeshProUGUI retroalimentacionTexto;

        /// <summary>
        /// Clip de sonido que se reproduce al pulsar un botón.
        /// </summary>
    public AudioClip sonidoBoton;

    private string pregunta = "¿Dónde se debería colocar la primera coma?\n\"Compré manzanas peras uvas y plátanos.\"";

    private string[] opciones = {
        "A.Después de 'compré'.",
        "B.Después de 'manzanas' y 'peras'.",
        "C.Después de 'uvas'.",
        "D.Después de 'peras' y 'uvas'."
    };

    private int indiceRespuestaCorrecta = 1;

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

        /// <summary>
        /// Selecciona la primera opción (índice 0) y la verifica.
        /// </summary>
    public void SeleccionarOpcion0() => VerificarRespuesta(0);

        /// <summary>
        /// Selecciona la segunda opción (índice 1) y la verifica.
        /// </summary>
    public void SeleccionarOpcion1() => VerificarRespuesta(1);

        /// <summary>
        /// Selecciona la tercera opción (índice 2) y la verifica.
        /// </summary>
    public void SeleccionarOpcion2() => VerificarRespuesta(2);

    
        /// <summary>
        /// Selecciona la cuarta opción (índice 3) y la verifica.
        /// </summary>
    public void SeleccionarOpcion3() => VerificarRespuesta(3);

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
