using UnityEngine;
using TMPro;

public class TranscribirOracion : MonoBehaviour
{
    public Paneles1 Paneles1;

    public TMP_Text textoOracion;
    public TMP_InputField campoRespuesta;
    public TMP_Text textoRetroalimentacion;

    private string oracionSinComas = "Compré manzanas peras uvas y plátanos";
    private string oracionCorrecta = "Compré manzanas, peras, uvas y plátanos.";

    void Start()
    {
        textoOracion.text = oracionSinComas;
        textoRetroalimentacion.text = "";
        campoRespuesta.text = "";
    }

    public void VerificarRespuesta()
    {
        string respuestaUsuario = campoRespuesta.text.Trim();

        if (respuestaUsuario == oracionCorrecta)
        {
            textoRetroalimentacion.text = "¡Correcto! ";
            IncrementarPuntajeBasicoCorrecto();
        }
        else
        {
            textoRetroalimentacion.text = "Incorrecto. ";
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
        int puntajeCorrecto = PlayerPrefs.GetInt("PuntajeCorrectoBasico", 0);
        PlayerPrefs.SetInt("PuntajeCorrectoBasico", ++puntajeCorrecto);
    }

    void IncrementarPuntajeBasicoIncorrecto()
    {
        int puntajeIncorrecto = PlayerPrefs.GetInt("PuntajeIncorrectoBasico", 0);
        PlayerPrefs.SetInt("PuntajeIncorrectoBasico", ++puntajeIncorrecto);
    }
}
