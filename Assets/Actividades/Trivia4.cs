 using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class OpcionUnica2 : MonoBehaviour
{
    public Paneles1 Paneles1;

    public TMP_Text preguntaTexto;
    public Button[] botonesOpciones;
    public TextMeshProUGUI retroalimentacionTexto;

    public AudioClip sonidoBoton;  

    private string pregunta = " ¿Cuales de las siguientes oraciones usa correctamenteb la comab para separar elementos de una serie?";
    private string[] opciones = { "A.Despues de compre.", "B.Despuesb de manzanas y peras.", "C.Despues de uvas .", "D.Despues de peras y uvas." };
    private int indiceRespuestaCorrecta = 1;

    void Start()
    {
        preguntaTexto.text = pregunta;
        retroalimentacionTexto.text = "";

        for (int i = 0; i < botonesOpciones.Length; i++)
        {
            TextMeshProUGUI textoBoton = botonesOpciones[i].GetComponentInChildren<TextMeshProUGUI>();
            if (textoBoton != null)
            {
                textoBoton.text = opciones[i];
            }
        }
    }

    public void SeleccionarOpcion0() => VerificarRespuesta(0);
    public void SeleccionarOpcion1() => VerificarRespuesta(1);
    public void SeleccionarOpcion2() => VerificarRespuesta(2);
    public void SeleccionarOpcion3() => VerificarRespuesta(3);

    void VerificarRespuesta(int indiceSeleccionado)
    {
        
        AudioManager.instancia.ReproducirSonido(sonidoBoton);

        if (indiceSeleccionado == indiceRespuestaCorrecta)
        {
            retroalimentacionTexto.text = "¡Correcto!";
            IncrementarPuntajeBasicoCorrecto();
        }
        else
        {
            retroalimentacionTexto.text = "Incorrecto.";
            IncrementarPuntajeBasicoIncorrecto();
        }

        foreach (Button b in botonesOpciones)
        {
            b.interactable = false;
        }

        Invoke("PasarASiguienteTrivia", 2f);
    }

    void PasarASiguienteTrivia()
    {
        Paneles1.SiguienteTrivia();
    }

    void IncrementarPuntajeBasicoCorrecto()
    {
        int puntajeCorrectoBasico = PlayerPrefs.GetInt("PuntajeCorrectoBasico", 0);
        PlayerPrefs.SetInt("PuntajeCorrectoBasico", ++puntajeCorrectoBasico);
    }

    void IncrementarPuntajeBasicoIncorrecto()
    {
        int puntajeIncorrectoBasico = PlayerPrefs.GetInt("PuntajeIncorrectoBasico", 0);
        PlayerPrefs.SetInt("PuntajeIncorrectoBasico", ++puntajeIncorrectoBasico);
    }
}

