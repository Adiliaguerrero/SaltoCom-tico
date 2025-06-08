using UnityEngine;
using TMPro;


    /// <summary>
    /// Controla la verificación de comas colocadas correctamente en un texto usando espacios válidos.
    /// </summary>
public class TextoConComasTMP : MonoBehaviour
{
    /// <summary>
    /// Componente TextMeshProUGUI que contiene el texto con espacios para colocar comas.
    /// </summary>
    public TextMeshProUGUI textoTMP;

    /// <summary>
    /// Texto que muestra la retroalimentación sobre el resultado de la verificación.
    /// </summary>
    public TextMeshProUGUI textoRetroalimentacion;

    /// <summary>
    /// Arreglo de RectTransforms que representan las posiciones válidas donde deben ir las comas.
    /// </summary>
    public RectTransform[] espaciosValidos;

    /// <summary>
    /// Referencia al controlador de paneles encargado de avanzar entre preguntas o escenas.
    /// </summary> 
    public Paneles1 Paneles1;

    /// <summary>
    /// Clip de sonido que se reproduce al verificar las comas.
    /// </summary>
    public AudioClip sonidoBoton;

    private string textoOriginal = "Compré plátanos [   ] manzanas [   ] uvas [   ] peras y pan.";

    void Start()
    {
        textoTMP.text = textoOriginal;

        if (textoRetroalimentacion != null)
            textoRetroalimentacion.gameObject.SetActive(false);
    }

    /// <summary>
    /// Verifica si las comas han sido colocadas correctamente en los espacios válidos.
    /// </summary>
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

            IncrementarPuntajeIntermedioCorrecto();
        }
        else
        {
            textoRetroalimentacion.text = "Incorrecto. No has colocado todas las comas correctamente.";

            IncrementarPuntajeIntermedioIncorrecto();
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

    void IncrementarPuntajeIntermedioCorrecto()
    {
        int puntajeCorrectoIntermedio = PlayerPrefs.GetInt("PuntajeCorrectoIntermedio", 0);
        PlayerPrefs.SetInt("PuntajeCorrectoIntermedio", ++puntajeCorrectoIntermedio);
    }

    void IncrementarPuntajeIntermedioIncorrecto()
    {
        int puntajeIncorrectoIntermedio = PlayerPrefs.GetInt("PuntajeIncorrectoIntermedio", 0);
        PlayerPrefs.SetInt("PuntajeIncorrectoIntermedio", ++puntajeIncorrectoIntermedio);
    }
}
