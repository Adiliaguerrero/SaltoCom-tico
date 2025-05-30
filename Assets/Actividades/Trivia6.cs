using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class PreguntaOpcionMultiple2 : MonoBehaviour
{
    public Paneles1 Paneles1;
    public TextMeshProUGUI textoPregunta;
    public Button[] botonesOpciones;
    public TextMeshProUGUI[] textosOpciones;
    public Button botonConfirmar;
    public TextMeshProUGUI textoResultado;

    public AudioClip sonidoBoton;  

    private string pregunta = "¿Cuál de las siguientes oraciones usa correctamente la coma para separar elementos de una serie?";
    private string[] opciones = { "A.Me gusta comer pizza hamburguesa y helado.", "B.En el parque hay árboles, flores y bancos.", " C.Compré una camiseta, unos pantalones, y unos zapatos.", "D.Mis colores favoritos son rojo, azul y verde" };
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

        ActualizarColores();
    }

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
            textoResultado.text = "¡Respuestas correctas";
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

    private void ActualizarColores()
    {
        for (int i = 0; i < botonesOpciones.Length; i++)
        {
            bool seleccionado = seleccionadas.Contains(i);

            textosOpciones[i].color = seleccionado ? Color.yellow : Color.white;

            ColorBlock colores = botonesOpciones[i].colors;
            colores.normalColor = Color.white;
            botonesOpciones[i].colors = colores;
        }
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
