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
