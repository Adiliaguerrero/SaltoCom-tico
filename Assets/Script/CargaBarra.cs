using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;  

public class CargaInicial : MonoBehaviour
{
    public Slider slider;  
    public float tiempoCarga = 5f;  
    public TextMeshProUGUI textoCarga;  
    public string nombreEscena;

    void Start()
    {
        if (slider != null)
        {
            StartCoroutine(Cargar());
        }
        else
        {
            Debug.LogError("El Slider no está asignado en el Inspector.");
        }
    }

    IEnumerator Cargar()
    {
        float tiempoTranscurrido = 0f;

        while (tiempoTranscurrido < tiempoCarga)
        {
            tiempoTranscurrido += Time.deltaTime;
            float progreso = tiempoTranscurrido / tiempoCarga;

            slider.value = progreso;

            if (textoCarga != null)
            {
                int porcentaje = Mathf.RoundToInt(progreso * 100);
                textoCarga.text = $"Cargando... {porcentaje}%";
            }

            yield return null;  
        }

        if (textoCarga != null)
        {
            textoCarga.text = "Cargando... 100%";
        }

        yield return new WaitForSeconds(2f);

        if (textoCarga != null)
        {
            textoCarga.text = "Carga completada!";
        }

        yield return new WaitForSeconds(2f);

        OnCargaCompleta();
    }

    void OnCargaCompleta()
    {
        Debug.Log("Carga completa");

        if (!string.IsNullOrEmpty(nombreEscena))
        {
            SceneManager.LoadScene(nombreEscena);
        }
        else
        {
            Debug.LogError("El nombre de la escena no está asignado en el Inspector.");
        }
    }
}