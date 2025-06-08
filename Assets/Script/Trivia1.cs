using UnityEngine;               
using UnityEngine.UI;          
using TMPro;

    /// <summary>
    /// Controla una pregunta de tipo Verdadero/Falso en una trivia.
    /// </summary>
public class Trivia1 : MonoBehaviour
{
    // Referencia al controlador de paneles para avanzar entre preguntas
    public Paneles1 Paneles1;

    // Componente TextMeshPro donde se mostrará la pregunta
    public TextMeshProUGUI preguntaTexto;

    // Botón para seleccionar "Verdadero"
    public Button botonVerdadero;

    // Botón para seleccionar "Falso"
    public Button botonFalso;

    // Texto donde se mostrará la retroalimentación (correcto o incorrecto)
    public TextMeshProUGUI retroalimentacionTexto;

    // Clip de audio que se reproduce al pulsar cualquier botón, asignado desde el Inspector
    public AudioClip sonidoBoton;

    // Texto que contiene la pregunta de la trivia
    public string pregunta = "¿El sol es una estrella?";

    // Valor booleano que indica cuál es la respuesta correcta (true = Verdadero, false = Falso)
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
