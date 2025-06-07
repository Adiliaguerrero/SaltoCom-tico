using UnityEngine;                 
using UnityEngine.UI;              
using TMPro;                       
using System.Collections;          
using UnityEngine.SceneManagement;

    /// <summary>
    /// Controla la simulación de una barra de carga con un <see cref="Slider"/> y texto porcentual,
    /// que culmina cargando una nueva escena.
    /// </summary>
public class CargaInicial : MonoBehaviour
{
    // Variable pública para asignar desde el Inspector la barra deslizante (slider)
    // Representa visualmente el progreso de carga, va de 0 a 1
        /// <summary>
        /// Referencia al componente Slider que representa visualmente el progreso de carga.
        /// </summary>
    public Slider slider;

    
        /// <summary>
        /// Tiempo total (en segundos) que durará la simulación de carga.
        /// </summary>
    public float tiempoCarga = 5f;

   
        /// <summary>
        /// Texto que mostrará el porcentaje de carga en pantalla.
        /// </summary>
    public TextMeshProUGUI textoCarga;


        /// <summary>
        /// Nombre de la escena que se cargará al completar la carga.
        /// </summary>
    public string nombreEscena;

        /// <summary>
        /// Se ejecuta al iniciar la escena. Verifica referencias y comienza la carga simulada.
        /// </summary>
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

        /// <summary>
        /// Corrutina que simula una barra de carga progresiva y muestra el porcentaje en texto.
        /// </summary>
        /// <returns>
        /// Enumerator para ejecución por frames.
        /// </returns>
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


        /// <summary>
        /// Cambia a la escena especificada una vez completada la simulación de carga.
        /// </summary>
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
