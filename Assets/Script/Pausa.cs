using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    /// <summary>
    /// Controla la funcionalidad de pausa en el juego, permitiendo pausar y reanudar la partida.
    /// </summary> 
    /// <remarks>
    /// Requiere que se asigne un GameObject UI como "MenuPausa" en el inspector.
    /// </remarks>

public class Pausa : MonoBehaviour
{
        /// <summary>
        /// Referencia al GameObject que contiene el menú de pausa (UI).
        /// </summary>
    public GameObject MenuPausa;
        /// <summary>
        /// Indica si el juego está actualmente en estado de pausa.
        /// </summary>
    public bool JuegoPausado = false;

    /// <summary>
    /// Verifica cada frame si se presiona la tecla 'E' para pausar/reanudar.
    /// </summary>
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            if (JuegoPausado)
            {
                Reanudar();
            }
            else
            {
                Pausar();
            }
    }
        /// <summary>
        /// Reanuda el juego, ocultando el menú de pausa y restableciendo la escala de tiempo.
        /// </summary>
       /// <remarks>
        /// Establece <see cref="Time.timeScale"/> a 1 para reanudar la física y animaciones.
        /// </remarks>
    public void Reanudar()
    {
        MenuPausa.SetActive(false);
        Time.timeScale = 1;
        JuegoPausado = false;
    }

    public void Pausar()
    {
        MenuPausa.SetActive(true);
        Time.timeScale = 0;
        JuegoPausado = true;
    }


}
