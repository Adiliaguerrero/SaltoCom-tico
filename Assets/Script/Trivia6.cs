using UnityEngine;
using TMPro;                  

/// <summary>
/// Controla la transcripción de una oración en la que el usuario debe colocar correctamente las comas.
/// </summary>
public class TranscribirOracion2 : MonoBehaviour
{
    /// <summary>
    /// Referencia al controlador de paneles para avanzar entre preguntas o escenas.
    /// </summary>
    public Paneles1 Paneles1;

    /// <summary>
    /// Componente TMP_Text donde se muestra la oración original sin comas.
    /// </summary>
        public TMP_Text textoOracion;

    /// <summary>
    /// Campo de entrada donde el usuario debe escribir la oración corregida con comas.
    /// </summary>
    public TMP_InputField campoRespuesta;

    /// <summary>
    /// Texto que muestra la retroalimentación tras verificar la respuesta (correcta o incorrecta).
    /// </summary>
    public TMP_Text textoRetroalimentacion;

   
    /// <summary>
    /// Sonido que se reproduce al verificar la respuesta, asignado desde el Inspector.
    /// </summary>
    public AudioClip sonidoBoton;

    private string oracionSinComas = "Reescriba la siguiente oración colocando la coma en los lugares correctos \n\"En cuanto me levanté me bañé comí y cepillé mis dientes.\"";

    private string oracionCorrecta = "En cuanto me levanté me bañé, comí y cepillé mis dientes.";

    void Start()
    {
        textoOracion.text = oracionSinComas;

        textoRetroalimentacion.text = "";

        campoRespuesta.text = "";
    }

    /// <summary>
    /// Verifica si la respuesta ingresada por el usuario coincide con la oración correcta.
    /// </summary>
    public void VerificarRespuesta()
    {
        AudioManager.instancia.ReproducirSonido(sonidoBoton);

        string respuestaUsuario = campoRespuesta.text.Trim();

        if (respuestaUsuario == oracionCorrecta)
        {
            textoRetroalimentacion.text = "¡Correcto! ";

            IncrementarPuntajeIntermedioCorrecto();
        }
        else
        {
            textoRetroalimentacion.text = "Incorrecto. ";

            IncrementarPuntajeIntermedioIncorrecto();
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

        puntajeCorrectoIntermedio++;

        PlayerPrefs.SetInt("PuntajeCorrectoIntermedio", puntajeCorrectoIntermedio);
    }

    void IncrementarPuntajeIntermedioIncorrecto()
    {
        int puntajeIncorrectoIntermedio = PlayerPrefs.GetInt("PuntajeIncorrectoIntermedio", 0);

        puntajeIncorrectoIntermedio++;

        PlayerPrefs.SetInt("PuntajeIncorrectoIntermedio", puntajeIncorrectoIntermedio);
    }
}
