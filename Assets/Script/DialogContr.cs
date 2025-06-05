
// Importamos librerías necesarias para el manejo de listas, corutinas y Unity
using System.Collections;                // Para usar corutinas (funciones que se ejecutan en varios frames)
using System.Collections.Generic;        // Para usar Listas genéricas, en este caso para las líneas de diálogo
using UnityEngine;                       // Funciones básicas y objetos de Unity
using TMPro;                            // Para usar TextMeshPro, que mejora la visualización de texto

// Esta clase controla la visualización de un sistema de dialogo de texto despues de activarse un portal 
public class DialogoController : MonoBehaviour
{
    // Panel que contiene todo el diálogo, que se mostrará y ocultará, asignado desde el Inspector
    public GameObject panelDialogo;

    // Componente TextMeshProUGUI donde se mostrará cada línea del diálogo
    public TextMeshProUGUI textoDialogo;

    // Lista pública que contiene todas las líneas de diálogo a mostrar
    // El atributo [TextArea(2, 4)] permite editar mejor en el Inspector, mostrando un área de texto con entre 2 y 4 líneas visibles
    [TextArea(2, 4)]
    public List<string> lineasDialogo;

    // Tiempo en segundos que permanecerá visible cada línea antes de pasar a la siguiente
    public float tiempoEntreLineas = 2f;

    // Método público que inicia la muestra del diálogo
    // Recibe un parámetro de tipo System.Action llamado 'alTerminar', que es una función que se ejecutará cuando termine el diálogo
    public void MostrarDialogo(System.Action alTerminar)
    {
        // Activamos el panel para que el diálogo se muestre en pantalla
        panelDialogo.SetActive(true);

        // Iniciamos la corutina MostrarLineas para mostrar las líneas de diálogo una a una
        StartCoroutine(MostrarLineas(alTerminar));
    }

    // Corutina privada que muestra las líneas del diálogo una a una, esperando un tiempo entre ellas
    // Recibe un parámetro 'callback', que es la función que se ejecutará al terminar todas las líneas
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
