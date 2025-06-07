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

    // Método Start se ejecuta automáticamente cuando comienza la escena o juego
    void Start()
    {
        // Verificamos si el slider fue asignado correctamente en el Inspector
        if (slider != null)
        {
            // Si está asignado, iniciamos la corutina que simula la carga
            StartCoroutine(Cargar());
        }
        else
        {
            // Si no está asignado, mostramos un error en la consola para avisar al programador
            Debug.LogError("El Slider no está asignado en el Inspector.");
        }
    }

    // Corutina que simula la barra de carga y actualiza el texto durante el proceso
    IEnumerator Cargar()
    {
        // Variable para llevar el tiempo que ha pasado desde que empezó la carga
        float tiempoTranscurrido = 0f;

        // Mientras el tiempo transcurrido sea menor al tiempo de carga definido,
        // seguimos ejecutando el bucle para actualizar la barra y texto
        while (tiempoTranscurrido < tiempoCarga)
        {
            // Sumamos el tiempo que pasó desde el último frame
            tiempoTranscurrido += Time.deltaTime;

            // Calculamos el progreso como un valor entre 0 y 1, dividiendo tiempo transcurrido / tiempo total
            float progreso = tiempoTranscurrido / tiempoCarga;

            // Actualizamos el valor del slider para que refleje el progreso actual
            slider.value = progreso;

            // Si el texto para mostrar porcentaje está asignado, actualizamos su contenido
            if (textoCarga != null)
            {
                // Convertimos el progreso decimal a porcentaje entero redondeado
                int porcentaje = Mathf.RoundToInt(progreso * 100);

                // Actualizamos el texto para mostrar el porcentaje de carga al usuario
                textoCarga.text = $"Cargando... {porcentaje}%";
            }

            // Esperamos al siguiente frame para continuar con el bucle (deja que Unity actualice la pantalla)
            yield return null;
        }

        // Cuando termina el tiempo de carga, nos aseguramos de mostrar el 100% en el texto
        if (textoCarga != null)
        {
            textoCarga.text = "Cargando... 100%";
        }

        // Esperamos 2 segundos para que el usuario pueda ver que la carga llegó al 100%
        yield return new WaitForSeconds(2f);

        // Actualizamos el texto para indicar que la carga ya se completó
        if (textoCarga != null)
        {
            textoCarga.text = "Carga completada!";
        }

        // Esperamos otros 2 segundos para que el usuario pueda ver el mensaje de carga completa
        yield return new WaitForSeconds(2f);

        // Llamamos al método que maneja la acción después de terminar la carga
        OnCargaCompleta();
    }

    // Método que se ejecuta cuando la carga ha finalizado y cambia de escena
    void OnCargaCompleta()
    {
        // Imprime en la consola que la carga terminó, útil para depuración
        Debug.Log("Carga completa");

        // Verificamos que el nombre de la escena para cargar no esté vacío o nulo
        if (!string.IsNullOrEmpty(nombreEscena))
        {
            // Cargamos la escena con el nombre asignado en el Inspector
            SceneManager.LoadScene(nombreEscena);
        }
        else
        {
            // Si no se asignó el nombre, mostramos un error en consola para avisar al programador
            Debug.LogError("El nombre de la escena no está asignado en el Inspector.");
        }
    }
}
