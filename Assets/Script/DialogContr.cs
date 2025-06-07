using System.Collections;               
using System.Collections.Generic;        
using UnityEngine;
using TMPro;

 
    /// <summary>
    /// Controla la visualización de un sistema de diálogo en pantalla después de activar un portal.
    /// </summary>
    /// <remarks>
    /// Este componente se encarga de mostrar líneas de diálogo una por una en un panel UI,
    /// utilizando un <see cref="TextMeshProUGUI"/> y un panel contenedor.
    /// </remarks>
    /// <example>
    /// Para mostrar el diálogo desde otro script:
    /// <code>
    /// var dialogo = FindObjectOfType&lt;DialogoController&gt;();
    /// dialogo.MostrarDialogo(() => Debug.Log("Diálogo finalizado"));
    /// </code>
    /// </example>
    /// <seealso cref="TextMeshProUGUI"/>
public class DialogoController : MonoBehaviour
{
        // Panel que contiene todo el diálogo, que se mostrará y ocultará, asignado desde el Inspector
        /// <summary>
        /// Panel de UI que contiene el diálogo completo, incluyendo fondo y texto.
        /// </summary>
        /// <remarks>
        /// Debe estar desactivado por defecto y se activa al iniciar el diálogo.
        /// </remarks>
    public GameObject panelDialogo;

    // Componente TextMeshProUGUI donde se mostrará cada línea del diálogo
        /// <summary>
        /// Componente de texto TMP que mostrará cada línea del diálogo.
        /// </summary>
    public TextMeshProUGUI textoDialogo;

        /// <summary>
        /// Lista de líneas que serán mostradas una a una en el diálogo.
        /// </summary>
        /// <remarks>
        /// El atributo <c>[TextArea]</c> permite una mejor edición en el Inspector.
        /// </remarks>
        [TextArea(2, 4)]
    public List<string> lineasDialogo;

    
        /// <summary>
        /// Tiempo (en segundos) que cada línea permanecerá en pantalla antes de pasar a la siguiente.
        /// </summary>
    public float tiempoEntreLineas = 2f;

        /// <summary>
        /// Inicia la visualización del diálogo.
        /// </summary>
        /// <param name="alTerminar">Acción a ejecutar cuando termine el diálogo.</param>
        /// <remarks>
        /// Esta función activa el panel y comienza a mostrar las líneas mediante una corrutina.
        /// </remarks>
        /// <exception cref="System.NullReferenceException">
        /// Se lanza si <see cref="textoDialogo"/> no está asignado en el Inspector.
        /// </exception>
    public void MostrarDialogo(System.Action alTerminar)
    {
        // Activamos el panel para que el diálogo se muestre en pantalla
        panelDialogo.SetActive(true);

        // Iniciamos la corutina MostrarLineas para mostrar las líneas de diálogo una a una
        StartCoroutine(MostrarLineas(alTerminar));
    }

        /// <summary>
        /// Corrutina que recorre cada línea del diálogo y la muestra en pantalla con una pausa entre cada una.
        /// </summary>
        /// <param name="callback">Función a invocar al finalizar el diálogo.</param>
        /// <returns>Un enumerador para manejar el retardo entre líneas.</returns>
        /// <remarks>
        /// Al terminar el diálogo, limpia el texto y desactiva el panel.
        /// </remarks>
        /// <seealso cref="IEnumerator"/>
    private IEnumerator MostrarLineas(System.Action callback)
    {
        // Recorremos cada línea en la lista lineasDialogo
        foreach (string linea in lineasDialogo)
        {
            // Actualizamos el texto mostrado con la línea actual
            textoDialogo.text = linea;

            // Esperamos el tiempo definido antes de mostrar la siguiente línea
            yield return new WaitForSeconds(tiempoEntreLineas);
        }

        // Al terminar todas las líneas, limpiamos el texto para que no quede nada visible
        textoDialogo.text = "";

        // Ocultamos el panel del diálogo para que desaparezca de la pantalla
        panelDialogo.SetActive(false);

        // Invocamos la función callback si fue asignada, para avisar que el diálogo terminó
        callback?.Invoke();
    }
}
