using UnityEngine;
using TMPro;

public class TextoConComasTMP : MonoBehaviour
{
    public TextMeshProUGUI textoTMP;
    public TextMeshProUGUI textoRetroalimentacion;
    public RectTransform[] espaciosValidos;
    public Paneles1 Paneles1;

    public AudioClip sonidoBoton;  

    private string textoOriginal = "Compré plátano [   ] manzana [   ] uvas [   ] peras y pan.";

    void Start()
    {
        textoTMP.text = textoOriginal;

        if (textoRetroalimentacion != null)
            textoRetroalimentacion.gameObject.SetActive(false);
    }

    public void VerificarComas()
    {
        AudioManager.instancia.ReproducirSonido(sonidoBoton);  

        int espaciosOcupados = 0;
        ComaDrag[] todasLasComas = FindObjectsOfType<ComaDrag>();

        foreach (var espacio in espaciosValidos)
        {
            foreach (var coma in todasLasComas)
            {
                if (Vector3.Distance(coma.transform.position, espacio.position) < 0.1f)
                {
                    espaciosOcupados++;
                    break;
                }
            }
        }

        if (textoRetroalimentacion != null)
            textoRetroalimentacion.gameObject.SetActive(true);

        if (espaciosOcupados == espaciosValidos.Length)
        {
            Debug.Log("¡Comas correctamente colocadas!");
            textoTMP.text = "Compré plátano, manzana, uvas, peras y pan.";

            if (textoRetroalimentacion != null)
                textoRetroalimentacion.text = "¡Muy bien! Has colocado correctamente las comas.";

            IncrementarPuntajeBasicoCorrecto();
        }
        else
        {
            textoRetroalimentacion.text = "Incorrecto. No has colocado todas las comas correctamente.";
            IncrementarPuntajeBasicoIncorrecto();
        }

        foreach (var coma in todasLasComas)
        {
            Destroy(coma.gameObject);
        }

        Invoke("PasarASiguienteTrivia", 2f);
    }

    void PasarASiguienteTrivia()
    {
        if (Paneles1 != null)
        {
            Paneles1.SiguienteTrivia();
        }
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

