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

    private string pregunta = " ¿Dónde deberías empezar a colocar una coma en la siguiente oración: Traje lápices bolígrafos marcadores y borradores";

    private string[] opciones = { "A. Después de lápices", "B.Después de bolígrafos", "C.Después de marcadores.", "D.Después de borradores." };

    private int indiceRespuestaCorrecta = 0;

    void Start()
    {
        preguntaTexto.text = pregunta;

        retroalimentacionTexto.text = "";

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

    void VerificarRespuesta(int indiceSeleccionado)
    {
        AudioManager.instancia.ReproducirSonido(sonidoBoton);

        if (indiceSeleccionado == indiceRespuestaCorrecta)
        {
            retroalimentacionTexto.text = "¡Correcto!";

            IncrementarPuntajeAvanzadoCorrecto();
        }
        else
        {
            retroalimentacionTexto.text = "Incorrecto.";

            IncrementarPuntajeAvanzadoIncorrecto();
        }

        foreach (Button b in botonesOpciones)
        {
            b.interactable = false;
        }

        Invoke("PasarASiguienteTrivia", 2f);
    }

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
