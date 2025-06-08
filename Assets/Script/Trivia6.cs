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
        // Asigna la oración sin comas al texto visible
        textoOracion.text = oracionSinComas;

        // Limpia el texto de retroalimentación para que no muestre nada al iniciar
        textoRetroalimentacion.text = "";

        // Limpia el campo de respuesta para que el usuario comience con un campo vacío
        campoRespuesta.text = "";
    }

    /// <summary>
    /// Verifica si la respuesta ingresada por el usuario coincide con la oración correcta.
    /// </summary>
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

            // Incrementa el puntaje de respuestas correctas en nivel intermedio
            IncrementarPuntajeIntermedioCorrecto();
        }
        else
        {
            // Si es incorrecta, muestra el mensaje "Incorrecto."
            textoRetroalimentacion.text = "Incorrecto. ";

            // Incrementa el puntaje de respuestas incorrectas en nivel intermedio
            IncrementarPuntajeIntermedioIncorrecto();
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

    // Método que incrementa el puntaje guardado en PlayerPrefs para respuestas correctas intermedias
    void IncrementarPuntajeIntermedioCorrecto()
    {
        // Obtiene el puntaje actual de respuestas correctas intermedias, si no existe devuelve 0
        int puntajeCorrectoIntermedio = PlayerPrefs.GetInt("PuntajeCorrectoIntermedio", 0);

        // Incrementa en 1 el puntaje
        puntajeCorrectoIntermedio++;

        // Guarda el nuevo puntaje actualizado en PlayerPrefs
        PlayerPrefs.SetInt("PuntajeCorrectoIntermedio", puntajeCorrectoIntermedio);
    }

    // Método que incrementa el puntaje guardado en PlayerPrefs para respuestas incorrectas intermedias
    void IncrementarPuntajeIntermedioIncorrecto()
    {
        // Obtiene el puntaje actual de respuestas incorrectas intermedias, si no existe devuelve 0
        int puntajeIncorrectoIntermedio = PlayerPrefs.GetInt("PuntajeIncorrectoIntermedio", 0);

        // Incrementa en 1 el puntaje
        puntajeIncorrectoIntermedio++;

        // Guarda el nuevo puntaje actualizado en PlayerPrefs
        PlayerPrefs.SetInt("PuntajeIncorrectoIntermedio", puntajeIncorrectoIntermedio);
    }
}
