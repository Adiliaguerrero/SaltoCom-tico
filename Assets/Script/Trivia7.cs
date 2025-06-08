using UnityEngine;              
using TMPro;


/// <summary>
/// Controla la transcripción de una oración avanzada en la que el usuario debe colocar correctamente las comas.
/// </summary>
public class TranscribirOracion3 : MonoBehaviour
{
    /// <summary>
    /// Referencia al controlador de paneles encargado de avanzar entre las trivias del juego.
    /// </summary>
    public Paneles1 Paneles1;

    /// <summary>
    /// Componente TMP_Text donde se muestra la oración original sin comas para que el usuario la corrija.
    /// </summary>
    public TMP_Text textoOracion;

    /// <summary>
    /// Campo de entrada donde el usuario escribe la oración corregida con puntuación adecuada.
    /// </summary>
    public TMP_InputField campoRespuesta;

    /// <summary>
    /// Componente de texto que muestra la retroalimentación (correcta o incorrecta) después de verificar la respuesta.
    /// </summary>
    public TMP_Text textoRetroalimentacion;

    /// <summary>
    /// Clip de audio que se reproduce cuando el usuario presiona el botón de verificación.
    /// </summary>
    public AudioClip sonidoBoton;

    private string oracionSinComas = "Reescriba la siguiente oración colocando la coma en los lugares correctos. \n\"En mi mochila llevo libros cuadernos lápices marcadores y borradores.\"";

    private string oracionCorrecta = "En mi mochila llevo libros, cuadernos, lápices, marcadores y borradores.";

    void Start()
    {
        textoOracion.text = oracionSinComas;

        textoRetroalimentacion.text = "";

        campoRespuesta.text = "";
    }


    /// <summary>
    /// Verifica si la respuesta ingresada por el usuario coincide exactamente con la oración correcta.
    /// </summary>
    public void VerificarRespuesta()
    {
        AudioManager.instancia.ReproducirSonido(sonidoBoton);

        string respuestaUsuario = campoRespuesta.text.Trim();

        if (respuestaUsuario == oracionCorrecta)
        {
            textoRetroalimentacion.text = "¡Correcto! ";

            IncrementarPuntajeAvanzadoCorrecto();
        }
        else
        {
            textoRetroalimentacion.text = "Incorrecto. ";

            IncrementarPuntajeAvanzadoIncorrecto();
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

        puntajeCorrectoAvanzado++;

        PlayerPrefs.SetInt("PuntajeCorrectoAvanzado", puntajeCorrectoAvanzado);
    }

    void IncrementarPuntajeAvanzadoIncorrecto()
    {
        int puntajeIncorrectoAvanzado = PlayerPrefs.GetInt("PuntajeIncorrectoAvanzado", 0);

        puntajeIncorrectoAvanzado++;

        PlayerPrefs.SetInt("PuntajeIncorrectoAvanzado", puntajeIncorrectoAvanzado);
    }
}
