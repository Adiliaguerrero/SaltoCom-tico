using UnityEngine;
using TMPro;                  

/// <summary>
/// Controla la transcripción de una oración en la que el usuario debe colocar correctamente las comas.
/// </summary>
public class TranscribirOracion2 : MonoBehaviour
{
    // Referencia al controlador de paneles para avanzar entre preguntas
    public Paneles1 Paneles1;

    // Componente TMP_Text donde se mostrará la oración sin comas
    public TMP_Text textoOracion;

    // Campo de entrada TMP_InputField donde el usuario escribirá la respuesta
    public TMP_InputField campoRespuesta;

    // Texto TMP donde se mostrará la retroalimentación (correcto o incorrecto)
    public TMP_Text textoRetroalimentacion;

    // Clip de audio que se reproducirá al pulsar el botón, asignado desde el Inspector
    public AudioClip sonidoBoton;

    // Oración que se muestra sin comas, para que el usuario la corrija
    private string oracionSinComas = "Reescriba la siguiente oración colocando la coma en los lugares correctos \n\"En cuanto me levanté me bañé comí y cepillé mis dientes.\"";

    // Oración correcta con la puntuación adecuada para validar la respuesta del usuario
    private string oracionCorrecta = "En cuanto me levanté me bañé, comí y cepillé mis dientes.";

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
