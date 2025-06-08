using UnityEngine;              
using TMPro;                  
using UnityEngine.UI;            


    /// <summary>
    /// Controla una pregunta de opción única con botones interactivos y retroalimentación.
    /// </summary>
public class OpcionUnica : MonoBehaviour
{
        /// <summary>
        /// Referencia al controlador de paneles que permite avanzar entre preguntas.
        /// </summary>
    public Paneles1 Paneles1;

        /// <summary>
        /// Componente de texto donde se mostrará la pregunta.
        /// </summary>    
    public TMP_Text preguntaTexto;

        /// <summary>
        /// Botones que representan las posibles respuestas.
        /// </summary>
    public Button[] botonesOpciones;

        /// <summary>
        /// Texto para mostrar si la respuesta fue correcta o incorrecta.
        /// </summary> 
    public TextMeshProUGUI retroalimentacionTexto;

        /// <summary>
        /// Clip de sonido que se reproduce al seleccionar una opción.
        /// </summary>
    public AudioClip sonidoBoton;

    private string pregunta = "En la oración \"Compré lápices, cuadernos, marcadores y borradores\", ¿cuántas comas hay?";

    private string[] opciones = { "A.Uno.", "B.Dos.", "C.Tres.", "D.Cuatro." };

    private int indiceRespuestaCorrecta = 1; // La opción correcta es "B.Dos."

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
        /// Método público que se llama cuando el jugador selecciona la primera opción.
        /// </summary>
    public void SeleccionarOpcion0() => VerificarRespuesta(0);

    
        /// <summary>
        /// Método público que se llama cuando el jugador selecciona la segunda opción.
        /// </summary>
    public void SeleccionarOpcion1() => VerificarRespuesta(1);

        /// <summary>
        /// Método público que se llama cuando el jugador selecciona la tercera opción.
        /// </summary>
    public void SeleccionarOpcion2() => VerificarRespuesta(2);

        /// <summary>
        /// Método público que se llama cuando el jugador selecciona la cuarta opción.
        /// </summary>
    public void SeleccionarOpcion3() => VerificarRespuesta(3);

    void VerificarRespuesta(int indiceSeleccionado)
    {
        AudioManager.instancia.ReproducirSonido(sonidoBoton);

        if (indiceSeleccionado == indiceRespuestaCorrecta)
        {
            retroalimentacionTexto.text = "¡Correcto!";

            IncrementarPuntajeBasicoCorrecto();
        }
        else
        {
            retroalimentacionTexto.text = "Incorrecto.";

            IncrementarPuntajeBasicoIncorrecto();
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

    void IncrementarPuntajeBasicoCorrecto()
    {
        int puntajeCorrectoBasico = PlayerPrefs.GetInt("PuntajeCorrectoBasico", 0);

        puntajeCorrectoBasico++;

        PlayerPrefs.SetInt("PuntajeCorrectoBasico", puntajeCorrectoBasico);
    }

    void IncrementarPuntajeBasicoIncorrecto()
    {
        int puntajeIncorrectoBasico = PlayerPrefs.GetInt("PuntajeIncorrectoBasico", 0);

        puntajeIncorrectoBasico++;

        PlayerPrefs.SetInt("PuntajeIncorrectoBasico", puntajeIncorrectoBasico);
    }
}
