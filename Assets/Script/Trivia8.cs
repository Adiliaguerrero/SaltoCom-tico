using UnityEngine;              
using TMPro;
using UnityEngine.UI;


/// <summary>
/// Controla una pregunta de opción única en el nivel avanzado del juego, evaluando el uso correcto de la coma.
/// </summary>
public class OpcionUnica3 : MonoBehaviour
{

    /// <summary>
    /// Referencia al controlador de paneles encargado de mostrar la siguiente trivia.
    /// </summary>
    public Paneles1 Paneles1;

    /// <summary>
    /// Texto donde se muestra la pregunta planteada al jugador.
    /// </summary>
    public TMP_Text preguntaTexto;


    /// <summary>
    /// Arreglo de botones que contienen las opciones disponibles como respuesta.
    /// </summary>
    public Button[] botonesOpciones;

    /// <summary>
    /// Texto donde se muestra la retroalimentación sobre la respuesta seleccionada.
    /// </summary>
    public TextMeshProUGUI retroalimentacionTexto;

    /// <summary>
    /// Clip de audio que se reproduce al seleccionar una opción.
    /// </summary>
    public AudioClip sonidoBoton;

    // Pregunta a mostrar
    private string pregunta = " ¿Dónde deberías empezar a colocar una coma en la siguiente oración: Traje lápices bolígrafos marcadores y borradores";

    // Opciones de respuesta que se asignan a cada botón
    private string[] opciones = { "A. Después de lápices", "B.Después de bolígrafos", "C.Después de marcadores.", "D.Después de borradores." };

    // Índice que representa la opción correcta (en este caso la opción A, índice 0)
    private int indiceRespuestaCorrecta = 0;

    // Método que se ejecuta al iniciar el script
    void Start()
    {
        // Muestra la pregunta en el texto correspondiente
        preguntaTexto.text = pregunta;

        // Limpia la retroalimentación para que no muestre nada inicialmente
        retroalimentacionTexto.text = "";

        // Asigna las opciones de texto a cada botón del array
        for (int i = 0; i < botonesOpciones.Length; i++)
        {
            TextMeshProUGUI textoBoton = botonesOpciones[i].GetComponentInChildren<TextMeshProUGUI>();
            if (textoBoton != null)
            {
                textoBoton.text = opciones[i];
            }
        }
    }


    /// <summary>
    /// Método invocado al seleccionar la opción A (índice 0).
    /// </summary>
    public void SeleccionarOpcion0() => VerificarRespuesta(0);

    /// <summary>
    /// Método invocado al seleccionar la opción B (índice 1).
    /// </summary>
    public void SeleccionarOpcion1() => VerificarRespuesta(1);

    /// <summary>
    /// Método invocado al seleccionar la opción C (índice 2).
    /// </summary>
    public void SeleccionarOpcion2() => VerificarRespuesta(2);

    /// <summary>
    /// Método invocado al seleccionar la opción D (índice 3).
    /// </summary>
    public void SeleccionarOpcion3() => VerificarRespuesta(3);

    // Método que verifica si la opción seleccionada es la correcta
    void VerificarRespuesta(int indiceSeleccionado)
    {
        // Reproduce el sonido al presionar un botón
        AudioManager.instancia.ReproducirSonido(sonidoBoton);

        // Compara la opción seleccionada con la correcta
        if (indiceSeleccionado == indiceRespuestaCorrecta)
        {
            // Si es correcta, muestra mensaje de éxito
            retroalimentacionTexto.text = "¡Correcto!";

            // Incrementa el puntaje de respuestas correctas avanzadas
            IncrementarPuntajeAvanzadoCorrecto();
        }
        else
        {
            // Si es incorrecta, muestra mensaje de error
            retroalimentacionTexto.text = "Incorrecto.";

            // Incrementa el puntaje de respuestas incorrectas avanzadas
            IncrementarPuntajeAvanzadoIncorrecto();
        }

        // Desactiva la interacción de todos los botones para evitar múltiples respuestas
        foreach (Button b in botonesOpciones)
        {
            b.interactable = false;
        }

        // Después de 2 segundos, pasa a la siguiente pregunta/trivia
        Invoke("PasarASiguienteTrivia", 2f);
    }

    // Método que llama al controlador de paneles para avanzar a la siguiente trivia
    void PasarASiguienteTrivia()
    {
        Paneles1.SiguienteTrivia();
    }

    void IncrementarPuntajeAvanzadoCorrecto()
    {
        int puntajeCorrectoAvanzado = PlayerPrefs.GetInt("PuntajeCorrectoAvanzado", 0);
        PlayerPrefs.SetInt("PuntajeCorrectoAvanzado", ++puntajeCorrectoAvanzado);
    }

    void IncrementarPuntajeAvanzadoIncorrecto()
    {
        int puntajeIncorrectoAvanzado = PlayerPrefs.GetInt("PuntajeIncorrectoAvanzado", 0);
        PlayerPrefs.SetInt("PuntajeIncorrectoAvanzado", ++puntajeIncorrectoAvanzado);
    }
}
