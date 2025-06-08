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

    // Oración que se muestra sin comas para que el usuario la corrija
    private string oracionSinComas = "Reescriba la siguiente oración colocando la coma en los lugares correctos. \n\"En mi mochila llevo libros cuadernos lápices marcadores y borradores.\"";

    // Oración correcta con la puntuación adecuada para validar la respuesta del usuario
    private string oracionCorrecta = "En mi mochila llevo libros, cuadernos, lápices, marcadores y borradores.";

    // Método que se ejecuta al iniciar el script
    void Start()
    {
        // Asigna la oración sin comas al texto visible
        textoOracion.text = oracionSinComas;

        // Limpia el texto de retroalimentación para que no muestre nada al iniciar
        textoRetroalimentacion.text = "";

        // Limpia el campo de respuesta para que el usuario comience con un campo vacío
        campoRespuesta.text = "";
    }

    // Método público que verifica si la respuesta del usuario es correcta o no
    public void VerificarRespuesta()
    {
        // Reproduce el sonido del botón usando el AudioManager global
        AudioManager.instancia.ReproducirSonido(sonidoBoton);

        // Obtiene la respuesta del usuario y elimina espacios al inicio y final
        string respuestaUsuario = campoRespuesta.text.Trim();

        // Compara la respuesta del usuario con la oración correcta
        if (respuestaUsuario == oracionCorrecta)
        {
            // Si es correcta, muestra el mensaje "¡Correcto!"
            textoRetroalimentacion.text = "¡Correcto! ";

            // Incrementa el puntaje de respuestas correctas en nivel avanzado
            IncrementarPuntajeAvanzadoCorrecto();
        }
        else
        {
            // Si es incorrecta, muestra el mensaje "Incorrecto."
            textoRetroalimentacion.text = "Incorrecto. ";

            // Incrementa el puntaje de respuestas incorrectas en nivel avanzado
            IncrementarPuntajeAvanzadoIncorrecto();
        }

        // Llama al método para pasar a la siguiente trivia después de 2 segundos
        Invoke("PasarASiguienteTrivia", 2f);
    }

    // Método que avanza a la siguiente pregunta de trivia usando el controlador de paneles
    void PasarASiguienteTrivia()
    {
        // Llama al método SiguienteTrivia del controlador Paneles1
        Paneles1.SiguienteTrivia();
    }

    // Método que incrementa el puntaje guardado en PlayerPrefs para respuestas correctas avanzadas
    void IncrementarPuntajeAvanzadoCorrecto()
    {
        // Obtiene el puntaje actual de respuestas correctas avanzadas, si no existe devuelve 0
        int puntajeCorrectoAvanzado = PlayerPrefs.GetInt("PuntajeCorrectoAvanzado", 0);

        // Incrementa en 1 el puntaje
        puntajeCorrectoAvanzado++;

        // Guarda el nuevo puntaje actualizado en PlayerPrefs
        PlayerPrefs.SetInt("PuntajeCorrectoAvanzado", puntajeCorrectoAvanzado);
    }

    // Método que incrementa el puntaje guardado en PlayerPrefs para respuestas incorrectas avanzadas
    void IncrementarPuntajeAvanzadoIncorrecto()
    {
        // Obtiene el puntaje actual de respuestas incorrectas avanzadas, si no existe devuelve 0
        int puntajeIncorrectoAvanzado = PlayerPrefs.GetInt("PuntajeIncorrectoAvanzado", 0);

        // Incrementa en 1 el puntaje
        puntajeIncorrectoAvanzado++;

        // Guarda el nuevo puntaje actualizado en PlayerPrefs
        PlayerPrefs.SetInt("PuntajeIncorrectoAvanzado", puntajeIncorrectoAvanzado);
    }
}
