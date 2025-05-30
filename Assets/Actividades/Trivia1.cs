using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Trivia1 : MonoBehaviour
{
    public Paneles1 Paneles1;

    public TextMeshProUGUI preguntaTexto;
    public Button botonVerdadero;
    public Button botonFalso;
    public TextMeshProUGUI retroalimentacionTexto;

    public AudioClip sonidoBoton; // Asignalo desde el Inspector

    public string pregunta = "¿El sol es una estrella?";
    public bool respuestaCorrecta = true;

    void Start()
    {
        MostrarPregunta();
    }

    void MostrarPregunta()
    {
        preguntaTexto.text = pregunta;
        retroalimentacionTexto.text = "";

        botonVerdadero.interactable = true;
        botonFalso.interactable = true;
    }

    public void PresionarVerdadero()
    {
        SeleccionarRespuesta(true);
    }

    public void PresionarFalso()
    {
        SeleccionarRespuesta(false);
    }

    void SeleccionarRespuesta(bool respuestaUsuario)
    {
        AudioManager.instancia.ReproducirSonido(sonidoBoton);

        if (respuestaUsuario == respuestaCorrecta)
        {
            retroalimentacionTexto.text = "¡Correcto!";
            IncrementarPuntajeBasicoCorrecto();
        }
        else
        {
            retroalimentacionTexto.text = "Incorrecto.";
            IncrementarPuntajeBasicoIncorrecto();
        }

        botonVerdadero.interactable = false;
        botonFalso.interactable = false;

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
