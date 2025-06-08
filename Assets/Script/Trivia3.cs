using UnityEngine;             
using UnityEngine.UI;           
using TMPro;                    
using System.Collections.Generic;

// Clase que controla una pregunta de opción múltiple con botones seleccionables

    /// <summary>
    /// Controla una pregunta de opción múltiple, permite seleccionar varias respuestas y muestra retroalimentación.
    /// </summary>
public class PreguntaOpcionMultiple : MonoBehaviour
{
    /// <summary>
    /// Referencia al controlador de paneles para avanzar entre preguntas.
    /// </summary>
    public Paneles1 Paneles1;

    /// <summary>
    /// Texto que muestra la pregunta actual.
    /// </summary>
    public TextMeshProUGUI textoPregunta;

    /// <summary>
    /// Botones que representan las opciones posibles.
    /// </summary>
    public Button[] botonesOpciones;
    
    /// <summary>
    /// Textos asociados a cada opción del array de botones.
    /// </summary>
    public TextMeshProUGUI[] textosOpciones;

    /// <summary>
    /// Botón utilizado para confirmar la selección del jugador.
    /// </summary>
    public Button botonConfirmar;

    /// <summary>
    /// Texto que muestra el resultado de la selección del jugador.
    /// </summary>
    public TextMeshProUGUI textoResultado;

    /// <summary>
    /// Clip de audio que se reproduce al seleccionar o confirmar una opción.
    /// </summary>
    public AudioClip sonidoBoton;

    private string pregunta = "¿Cuál de las siguientes oraciones usa correctamente la coma para separar elementos de una serie?";

    private string[] opciones = {
        "A.Me gusta comer pizza hamburguesa y helado.",
        "B.En el parque hay árboles, flores y bancos.",
        "C.Compré una camiseta, unos pantalones, y unos zapatos.",
        "D.Mis colores favoritos son rojo, azul y verde"
    };

    private int[] indicesCorrectos = { 0, 3 };

    private List<int> seleccionadas = new List<int>();

    void Start()
    {
        // Asigna el texto de la pregunta al componente correspondiente
        textoPregunta.text = pregunta;

        // Recorre el array de textos de opciones para asignarles el texto correspondiente
        for (int i = 0; i < textosOpciones.Length; i++)
        {
            textosOpciones[i].text = opciones[i];
        }
    }

    /// <summary>
    /// Método público llamado cuando el jugador selecciona una opción.
    /// </summary>
    /// <param name="indice">Índice de la opción seleccionada.</param>
    public void SeleccionarOpcion(int indice)
    {
        // Reproduce el sonido del botón usando el AudioManager global
        AudioManager.instancia.ReproducirSonido(sonidoBoton);

        // Si la opción ya estaba seleccionada, la elimina (toggle)
        if (seleccionadas.Contains(indice))
        {
            seleccionadas.Remove(indice);
        }
        // Si no está seleccionada y aún no hay 2 seleccionadas, la añade
        else if (seleccionadas.Count < 2)
        {
            seleccionadas.Add(indice);
        }

        // Actualiza los colores para reflejar las opciones seleccionadas

    }
    
    /// <summary>
    /// Verifica si las opciones seleccionadas son correctas y proporciona retroalimentación.
    /// </summary>
    public void VerificarRespuestas()
    {
        // Reproduce el sonido del botón al confirmar
        AudioManager.instancia.ReproducirSonido(sonidoBoton);

        // Creamos una copia de las respuestas correctas para trabajar con ellas
        List<int> correctas = new List<int>(indicesCorrectos);

        // Ordenamos ambas listas para facilitar la comparación
        seleccionadas.Sort();
        correctas.Sort();

        // Contador de respuestas correctas seleccionadas por el usuario
        int correctasSeleccionadas = 0;

        // Recorremos las opciones seleccionadas para contar cuántas son correctas
        for (int i = 0; i < seleccionadas.Count; i++)
        {
            if (correctas.Contains(seleccionadas[i]))
            {
                correctasSeleccionadas++;
            }
        }

        // Cambiamos el color del texto de cada opción según si fue correcta, incorrecta o no seleccionada
        for (int i = 0; i < botonesOpciones.Length; i++)
        {
            if (seleccionadas.Contains(i))
            {
                if (correctas.Contains(i))
                {
                    // Si está seleccionada y es correcta, pone el texto en verde
                    textosOpciones[i].color = Color.green;
                }
                else
                {
                    // Si está seleccionada pero es incorrecta, pone el texto en rojo
                    textosOpciones[i].color = Color.red;
                }
            }
            else
            {
                // Si no está seleccionada, el texto permanece blanco
                textosOpciones[i].color = Color.white;
            }
        }

        // Determina el mensaje a mostrar según cuántas respuestas correctas haya seleccionado el usuario
        if (correctasSeleccionadas == 2)
        {
            textoResultado.text = "¡Respuestas correctas!";
            IncrementarPuntajeBasicoCorrecto();
        }
        else if (correctasSeleccionadas == 1)
        {
            textoResultado.text = "Al menos una opción es correcta.";
        }
        else
        {
            textoResultado.text = "Respuestas incorrectas.";
            IncrementarPuntajeBasicoIncorrecto();
        }

        // Invoca el método para pasar a la siguiente trivia después de 2 segundos
        Invoke("PasarASiguienteTrivia", 2f);
    }

    // Método que avanza a la siguiente pregunta de trivia usando el controlador de paneles
    void PasarASiguienteTrivia()
    {
        Paneles1.SiguienteTrivia();
    }



    // Incrementa el puntaje de respuestas correctas en PlayerPrefs
    void IncrementarPuntajeBasicoCorrecto()
    {
        int puntaje = PlayerPrefs.GetInt("PuntajeCorrectoBasico", 0);
        PlayerPrefs.SetInt("PuntajeCorrectoBasico", ++puntaje);
    }

    // Incrementa el puntaje de respuestas incorrectas en PlayerPrefs
    void IncrementarPuntajeBasicoIncorrecto()
    {
        int puntaje = PlayerPrefs.GetInt("PuntajeIncorrectoBasico", 0);
        PlayerPrefs.SetInt("PuntajeIncorrectoBasico", ++puntaje);
    }
}
