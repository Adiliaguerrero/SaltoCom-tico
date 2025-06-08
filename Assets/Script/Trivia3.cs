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
        textoPregunta.text = pregunta;

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
        AudioManager.instancia.ReproducirSonido(sonidoBoton);

        if (seleccionadas.Contains(indice))
        {
            seleccionadas.Remove(indice);
        }
        else if (seleccionadas.Count < 2)
        {
            seleccionadas.Add(indice);
        }


    }
    
    /// <summary>
    /// Verifica si las opciones seleccionadas son correctas y proporciona retroalimentación.
    /// </summary>
    public void VerificarRespuestas()
    {
        AudioManager.instancia.ReproducirSonido(sonidoBoton);

        List<int> correctas = new List<int>(indicesCorrectos);

        seleccionadas.Sort();
        correctas.Sort();

        int correctasSeleccionadas = 0;

        for (int i = 0; i < seleccionadas.Count; i++)
        {
            if (correctas.Contains(seleccionadas[i]))
            {
                correctasSeleccionadas++;
            }
        }

        for (int i = 0; i < botonesOpciones.Length; i++)
        {
            if (seleccionadas.Contains(i))
            {
                if (correctas.Contains(i))
                {
                    textosOpciones[i].color = Color.green;
                }
                else
                {
                    textosOpciones[i].color = Color.red;
                }
            }
            else
            {
                textosOpciones[i].color = Color.white;
            }
        }

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

        Invoke("PasarASiguienteTrivia", 2f);
    }

    void PasarASiguienteTrivia()
    {
        Paneles1.SiguienteTrivia();
    }



    void IncrementarPuntajeBasicoCorrecto()
    {
        int puntaje = PlayerPrefs.GetInt("PuntajeCorrectoBasico", 0);
        PlayerPrefs.SetInt("PuntajeCorrectoBasico", ++puntaje);
    }

    void IncrementarPuntajeBasicoIncorrecto()
    {
        int puntaje = PlayerPrefs.GetInt("PuntajeIncorrectoBasico", 0);
        PlayerPrefs.SetInt("PuntajeIncorrectoBasico", ++puntaje);
    }
}
