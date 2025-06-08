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

    public AudioClip sonidoBoton;


    public string pregunta = "¿El sol es una estrella?";

    
    public bool respuestaCorrecta = true;

    // Método que se ejecuta al iniciar el script
    void Start()
    {
        // Llama al método para mostrar la pregunta en pantalla y preparar la UI
        MostrarPregunta();
    }

    // Método que muestra la pregunta y limpia cualquier retroalimentación 
    void MostrarPregunta()
    {
        // Asigna el texto de la pregunta al componente correspondiente
        preguntaTexto.text = pregunta;

        // Limpia el texto de retroalimentación para que no muestre nada al iniciar
        retroalimentacionTexto.text = "";

        // Habilita ambos botones para que el usuario pueda seleccionar una respuesta
        botonVerdadero.interactable = true;
        botonFalso.interactable = true;
    }

    // Método público que se llama cuando el usuario presiona el botón "Verdadero"
    public void PresionarVerdadero()
    {
        // Llama a la función para evaluar la respuesta, pasando true porque eligió "Verdadero"
        SeleccionarRespuesta(true);
    }

    // Método público que se llama cuando el usuario presiona el botón "Falso"
    public void PresionarFalso()
    {
        // Llama a la función para evaluar la respuesta, pasando false porque eligió "Falso"
        SeleccionarRespuesta(false);
    }

    // Método privado que maneja la lógica para evaluar la respuesta del usuario
    void SeleccionarRespuesta(bool respuestaUsuario)
    {
        // Reproduce el sonido del botón usando el AudioManager global
        AudioManager.instancia.ReproducirSonido(sonidoBoton);

        // Compara la respuesta del usuario con la respuesta correcta
        if (respuestaUsuario == respuestaCorrecta)
        {
            // Si es correcta, muestra el mensaje "¡Correcto!"
            retroalimentacionTexto.text = "¡Correcto!";

            // Aumenta el puntaje de respuestas correctas
            IncrementarPuntajeBasicoCorrecto();
        }
        else
        {
            // Si es incorrecta, muestra el mensaje "Incorrecto."
            retroalimentacionTexto.text = "Incorrecto.";

            // Aumenta el puntaje de respuestas incorrectas
            IncrementarPuntajeBasicoIncorrecto();
        }

        // Deshabilita los botones para que el usuario no pueda cambiar la respuesta
        botonVerdadero.interactable = false;
        botonFalso.interactable = false;

        // Llama al método para pasar a la siguiente trivia después de 2 segundos
        Invoke("PasarASiguienteTrivia", 2f);
    }

    // Método que avanza a la siguiente pregunta de trivia usando el controlador de paneles
    void PasarASiguienteTrivia()
    {
        // Llama al método SiguienteTrivia del controlador Paneles1
        Paneles1.SiguienteTrivia();
    }

    // Método que incrementa el puntaje guardado en PlayerPrefs para respuestas correctas
    void IncrementarPuntajeBasicoCorrecto()
    {
        // Obtiene el puntaje actual de respuestas correctas, si no existe devuelve 0
        int puntaje = PlayerPrefs.GetInt("PuntajeCorrectoBasico", 0);

        // Incrementa en 1 el puntaje
        puntaje++;

        // Guarda el nuevo puntaje actualizado en PlayerPrefs
        PlayerPrefs.SetInt("PuntajeCorrectoBasico", puntaje);
    }

    // Método que incrementa el puntaje guardado en PlayerPrefs para respuestas incorrectas
    void IncrementarPuntajeBasicoIncorrecto()
    {
        // Obtiene el puntaje actual de respuestas incorrectas, si no existe devuelve 0
        int puntaje = PlayerPrefs.GetInt("PuntajeIncorrectoBasico", 0);

        // Incrementa en 1 el puntaje
        puntaje++;

        // Guarda el nuevo puntaje actualizado en PlayerPrefs
        PlayerPrefs.SetInt("PuntajeIncorrectoBasico", puntaje);
    }
}
