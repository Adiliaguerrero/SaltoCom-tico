using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

// Esta clase controla una secuencia de diálogos que se muestran en pantalla letra por letra y puntos de carga animados.
    /// <summary>
    /// Controla la presentación secuencial de diálogos en pantalla, letra por letra, con una animación de puntos al final y cambio automático de escena.
    /// </summary>
    /// <remarks>
    /// Ideal para escenas introductorias o transiciones narrativas.
    /// Utiliza corutinas para mostrar texto gradualmente y gestionar tiempos.
    /// </remarks>
public class PantallaDialogo : MonoBehaviour
{
    /// <summary>
    /// Componente de texto donde se muestra el diálogo actual.
    /// </summary>
    public TMP_Text textoPantalla;

    /// <summary>
    /// Arreglo de líneas de diálogo a mostrar.
    /// </summary>
    [TextArea(2, 5)]
    public string[] dialogos;


        /// <summary>
        /// Tiempo de espera entre cada letra del texto (efecto de máquina de escribir).
        /// </summary>
    public float tiempoEntreLetras = 0.05f;


        /// <summary>
        /// Tiempo de espera entre cada línea de diálogo.
        /// </summary>
    public float tiempoEntreDialogos = 3f;

        /// <summary>
        /// Texto que muestra una animación de puntos suspensivos como indicador de carga.
        /// </summary>
    public TMP_Text puntosCargaTexto;

    
        /// <summary>
        /// Tiempo que se espera después de los puntos antes de cambiar de escena.
        /// </summary>
    public float tiempoAntesCambioEscena = 2f;

        /// <summary>
        /// Nombre de la escena que se cargará una vez finalizada toda la secuencia de diálogo.
        /// </summary>
    public string nombreEscenaSiguiente;

    // Índice para llevar el control de qué línea de diálogo se está mostrando
    private int indiceDialogo = 0;

    // Método que se ejecuta al iniciar el dialogo
    void Start()
    {
        // Si hay al menos un diálogo, comienza la secuencia de diálogo
        if (dialogos.Length > 0)
            StartCoroutine(MostrarDialogo());
    }

    // Corrutina que muestra los diálogos letra por letra y espera entre líneas
    IEnumerator MostrarDialogo()
    {
        // Mientras haya más diálogos por mostrar
        while (indiceDialogo < dialogos.Length)
        {
            textoPantalla.text = ""; // Limpia el texto actual
            string linea = dialogos[indiceDialogo]; // Obtiene la línea actual

            // Añade cada letra poco a poco con espera entre cada una
            foreach (char letra in linea)
            {
                textoPantalla.text += letra;
                yield return new WaitForSeconds(tiempoEntreLetras);
            }

            // Espera antes de mostrar la siguiente línea
            yield return new WaitForSeconds(tiempoEntreDialogos);
            indiceDialogo++;
        }

        // Activa el texto de puntos de carga y comienza su animación
        puntosCargaTexto.gameObject.SetActive(true);
        StartCoroutine(AnimarPuntos());

        // Espera un tiempo antes de cambiar de escena
        yield return new WaitForSeconds(tiempoAntesCambioEscena);

        // Cambia a la escena especificada
        SceneManager.LoadScene(nombreEscenaSiguiente);
    }

    // Corrutina que anima los puntos de carga (., .., ...)
    IEnumerator AnimarPuntos()
    {
        string[] puntos = { ".", "..", "..." };
        int index = 0;

        // Bucle infinito para animar los puntos mientras la escena no cambia
        while (true)
        {
            puntosCargaTexto.text = puntos[index];
            index = (index + 1) % puntos.Length;
            yield return new WaitForSeconds(0.4f);
        }
    }
}
