 using UnityEngine;
using TMPro;

public class TranscribirOracion2 : MonoBehaviour
{
    public Paneles1 Paneles1;

    public TMP_Text textoOracion;
    public TMP_InputField campoRespuesta;
    public TMP_Text textoRetroalimentacion;

    public AudioClip sonidoBoton; 

    private string oracionSinComas = ": En mi mochila llevo libros _____ cuadernos _____ lápices _____ marcadores y borradores";
    private string oracionCorrecta = "En mi mochila llevo, libros, cuadernos, lapices, marcadores y borradores.";

    void Start()
    {
        textoOracion.text = oracionSinComas;
        textoRetroalimentacion.text = "";
        campoRespuesta.text = "";
    }

    public void VerificarRespuesta()
    {
       
        AudioManager.instancia.ReproducirSonido(sonidoBoton);

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
