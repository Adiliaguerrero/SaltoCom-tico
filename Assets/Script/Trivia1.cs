using UnityEngine;               
using UnityEngine.UI;          
using TMPro;

    /// <summary>
    /// Controla una pregunta de tipo Verdadero/Falso en una trivia.
    /// </summary>
public class Trivia1 : MonoBehaviour
{
        /// <summary>
        /// Referencia al controlador de paneles para avanzar entre preguntas.
        /// </summary>
    public Paneles1 Paneles1;
   
    /// <summary>
    /// Componente de texto donde se mostrará la pregunta.
    /// </summary>
    public TextMeshProUGUI preguntaTexto;


    /// <summary>
    /// Botón para seleccionar "Verdadero".
    /// </summary>
    public Button botonVerdadero;

    /// <summary>
    /// Botón para seleccionar "Falso".
    /// </summary>
    public Button botonFalso;

    /// <summary>
    /// Texto donde se mostrará la retroalimentación (correcto o incorrecto).
    /// </summary>
    public TextMeshProUGUI retroalimentacionTexto;


    /// <summary>
    /// Clip de audio que se reproduce al pulsar cualquier botón.
    /// </summary>
    public AudioClip sonidoBoton;

    /// <summary>
    /// Texto que contiene la pregunta de la trivia.
    /// </summary>
    public string pregunta = "¿El sol es una estrella?";

    /// <summary>
    /// Valor booleano que indica cuál es la respuesta correcta.
    /// </summary>
    public bool respuestaCorrecta = true;

    void Start()
    {
        MostrarPregunta();
    }

    void MostrarPregunta()
    {
        preguntaTexto.text = pregunta;

        retroalimentacionTexto.text = "";

        botonVerdadero.interactable = true;
        botonFalso.interactable = true;
    }

        /// <summary>
        /// Método público que se llama cuando el usuario presiona el botón "Verdadero".
        /// </summary>
    public void PresionarVerdadero()
    {
        SeleccionarRespuesta(true);
    }

        /// <summary>
        /// Método público que se llama cuando el usuario presiona el botón "Falso".
        /// </summary>
    public void PresionarFalso()
    {
        SeleccionarRespuesta(false);
    }

    void SeleccionarRespuesta(bool respuestaUsuario)
    {
        AudioManager.instancia.ReproducirSonido(sonidoBoton);

        if (respuestaUsuario == respuestaCorrecta)
        {
            retroalimentacionTexto.text = "¡Correcto!";

            IncrementarPuntajeBasicoCorrecto();
        }
        else
        {
            retroalimentacionTexto.text = "Incorrecto.";

            IncrementarPuntajeBasicoIncorrecto();
        }

        botonVerdadero.interactable = false;
        botonFalso.interactable = false;

        Invoke("PasarASiguienteTrivia", 2f);
    }

    void PasarASiguienteTrivia()
    {
        // Llama al método SiguienteTrivia del controlador Paneles1
        Paneles1.SiguienteTrivia();
    }

    void IncrementarPuntajeBasicoCorrecto()
    {
        int puntaje = PlayerPrefs.GetInt("PuntajeCorrectoBasico", 0);

        puntaje++;

        PlayerPrefs.SetInt("PuntajeCorrectoBasico", puntaje);
    }

    void IncrementarPuntajeBasicoIncorrecto()
    {
        int puntaje = PlayerPrefs.GetInt("PuntajeIncorrectoBasico", 0);

        puntaje++;

        PlayerPrefs.SetInt("PuntajeIncorrectoBasico", puntaje);
    }
}
