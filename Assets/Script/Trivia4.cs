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
        AudioManager.instancia.ReproducirSonido(sonidoBoton);

        if (indiceSeleccionado == indiceRespuestaCorrecta)
        {
            retroalimentacionTexto.text = "¡Correcto!";
            IncrementarPuntajeIntermedioCorrecto();
        }
        else
        {
            retroalimentacionTexto.text = "Incorrecto.";
            IncrementarPuntajeIntermedioIncorrecto();
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

    void IncrementarPuntajeIntermedioCorrecto()
    {
        int puntajeCorrectoIntermedio = PlayerPrefs.GetInt("PuntajeCorrectoIntermedio", 0);
        PlayerPrefs.SetInt("PuntajeCorrectoIntermedio", ++puntajeCorrectoIntermedio);
    }

    void IncrementarPuntajeIntermedioIncorrecto()
    {
        int puntajeIncorrectoIntermedio = PlayerPrefs.GetInt("PuntajeIncorrectoIntermedio", 0);
        PlayerPrefs.SetInt("PuntajeIncorrectoIntermedio", ++puntajeIncorrectoIntermedio);
    }
}
