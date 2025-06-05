// Importamos las librerías necesarias para Unity, UI y TextMeshPro
using UnityEngine;               // Funciones básicas y objetos de Unity
using UnityEngine.UI;            // Para manejar componentes UI como botones
using TMPro;                    // Para textos con TextMeshPro
using System.Collections.Generic; // Para usar listas genéricas

// Clase que controla una pregunta de opción múltiple con botones seleccionables
public class PreguntaOpcionMultiple : MonoBehaviour
{
    // Referencia al controlador de paneles para avanzar entre preguntas
    public Paneles1 Paneles1;

    // Texto donde se mostrará la pregunta (TextMeshPro)
    public TextMeshProUGUI textoPregunta;

    // Array de botones que representan las opciones para responder
    public Button[] botonesOpciones;

    // Array con los textos asociados a cada opción (TextMeshPro)
    public TextMeshProUGUI[] textosOpciones;

    // Botón para confirmar las opciones seleccionadas
    public Button botonConfirmar;

    // Texto donde se mostrará el resultado o retroalimentación al usuario
    public TextMeshProUGUI textoResultado;

    // Clip de audio que se reproducirá al pulsar cualquier botón, asignado desde el Inspector
    public AudioClip sonidoBoton;

    // Pregunta que se mostrará al usuario (privada porque no es necesaria en Inspector)
    private string pregunta = "¿Cuál de las siguientes oraciones usa correctamente la coma para separar elementos de una serie?";

    // Array con las opciones que aparecerán en los botones
    private string[] opciones = {
        "A.Me gusta comer pizza hamburguesa y helado.",
        "B.En el parque hay árboles, flores y bancos.",
        "C.Compré una camiseta, unos pantalones, y unos zapatos.",
        "D.Mis colores favoritos son rojo, azul y verde"
    };

    // Índices de las opciones correctas (aquí son dos opciones correctas: la 0 y la 3)
    private int[] indicesCorrectos = { 0, 3 };

    // Lista para almacenar los índices de las opciones que el usuario ha seleccionado
    private List<int> seleccionadas = new List<int>();

    // Método que se ejecuta al iniciar el script
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

    // Método público llamado al seleccionar una opción, con el índice de la opción
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

    // Método que verifica las respuestas seleccionadas al confirmar
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
