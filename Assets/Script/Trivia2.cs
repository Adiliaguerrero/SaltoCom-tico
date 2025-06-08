using UnityEngine;              
using TMPro;                  
using UnityEngine.UI;            


    /// <summary>
    /// Controla una pregunta de opción única con botones interactivos y retroalimentación.
    /// </summary>
public class OpcionUnica : MonoBehaviour
{
    // Referencia al controlador de paneles para avanzar entre preguntas
    public Paneles1 Paneles1;

    // Componente TextMeshPro donde se mostrará la pregunta
    public TMP_Text preguntaTexto;

    // Array con los botones que representan las opciones para responder
    public Button[] botonesOpciones;

    // Texto donde se mostrará la retroalimentación (correcto o incorrecto)
    public TextMeshProUGUI retroalimentacionTexto;

    // Clip de audio que se reproducirá al pulsar cualquier botón, asignado desde el Inspector
    public AudioClip sonidoBoton;

    // Pregunta que se mostrará al usuario (privada porque no es necesaria en Inspector)
    private string pregunta = "En la oración \"Compré lápices, cuadernos, marcadores y borradores\", ¿cuántas comas hay?";

    // Array con las opciones que aparecerán en los botones
    private string[] opciones = { "A.Uno.", "B.Dos.", "C.Tres.", "D.Cuatro." };

    // Índice que indica cuál opción es la correcta (comienza en 0)
    private int indiceRespuestaCorrecta = 1; // La opción correcta es "B.Dos."

    // Método que se ejecuta al iniciar el script
    void Start()
    {
        // Asigna el texto de la pregunta al componente correspondiente
        preguntaTexto.text = pregunta;

        // Limpia el texto de retroalimentación para que no muestre nada al iniciar
        retroalimentacionTexto.text = "";

        // Recorre el array de botones para asignarles el texto correspondiente
        for (int i = 0; i < botonesOpciones.Length; i++)
        {
            // Obtiene el componente TextMeshPro hijo dentro del botón
            TextMeshProUGUI textoBoton = botonesOpciones[i].GetComponentInChildren<TextMeshProUGUI>();

            // Si el componente existe, asigna el texto de la opción correspondiente
            if (textoBoton != null)
            {
                textoBoton.text = opciones[i];
            }
        }
    }

    // Métodos públicos que se llaman desde los botones para seleccionar cada opción
    public void SeleccionarOpcion0() => VerificarRespuesta(0);
    public void SeleccionarOpcion1() => VerificarRespuesta(1);
    public void SeleccionarOpcion2() => VerificarRespuesta(2);
    public void SeleccionarOpcion3() => VerificarRespuesta(3);

    // Método privado que verifica si la opción seleccionada es correcta o no
    void VerificarRespuesta(int indiceSeleccionado)
    {
        // Reproduce el sonido del botón usando el AudioManager global
        AudioManager.instancia.ReproducirSonido(sonidoBoton);

        // Compara el índice seleccionado con el índice de la respuesta correcta
        if (indiceSeleccionado == indiceRespuestaCorrecta)
        {
            // Si es correcta, muestra el mensaje "¡Correcto!"
            retroalimentacionTexto.text = "¡Correcto!";

            // Incrementa el puntaje de respuestas correctas
            IncrementarPuntajeBasicoCorrecto();
        }
        else
        {
            // Si es incorrecta, muestra el mensaje "Incorrecto."
            retroalimentacionTexto.text = "Incorrecto.";

            // Incrementa el puntaje de respuestas incorrectas
            IncrementarPuntajeBasicoIncorrecto();
        }

        // Deshabilita todos los botones para evitar cambiar la respuesta
        foreach (Button b in botonesOpciones)
        {
            b.interactable = false;
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

    // Método que incrementa el puntaje guardado en PlayerPrefs para respuestas correctas
    void IncrementarPuntajeBasicoCorrecto()
    {
        // Obtiene el puntaje actual de respuestas correctas, si no existe devuelve 0
        int puntajeCorrectoBasico = PlayerPrefs.GetInt("PuntajeCorrectoBasico", 0);

        // Incrementa en 1 el puntaje
        puntajeCorrectoBasico++;

        // Guarda el nuevo puntaje actualizado en PlayerPrefs
        PlayerPrefs.SetInt("PuntajeCorrectoBasico", puntajeCorrectoBasico);
    }

    // Método que incrementa el puntaje guardado en PlayerPrefs para respuestas incorrectas
    void IncrementarPuntajeBasicoIncorrecto()
    {
        // Obtiene el puntaje actual de respuestas incorrectas, si no existe devuelve 0
        int puntajeIncorrectoBasico = PlayerPrefs.GetInt("PuntajeIncorrectoBasico", 0);

        // Incrementa en 1 el puntaje
        puntajeIncorrectoBasico++;

        // Guarda el nuevo puntaje actualizado en PlayerPrefs
        PlayerPrefs.SetInt("PuntajeIncorrectoBasico", puntajeIncorrectoBasico);
    }
}
