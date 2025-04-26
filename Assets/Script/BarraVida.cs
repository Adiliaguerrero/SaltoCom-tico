using UnityEngine;
using UnityEngine.UI;

public class BarraVida : MonoBehaviour
{
    public Image rellenoBarraVida;
    private float VidaMaxima = 5f;

    public GameObject panelGameOver; // Asume que tienes un Panel para Game Over

    // Ahora este método acepta un parámetro de tipo float para la vida actual
    public void ActualizarBarra(float vidaActual)
    {
        vidaActual = Mathf.Clamp(vidaActual, 0f, VidaMaxima);
        rellenoBarraVida.fillAmount = vidaActual / VidaMaxima; // Ahora 5/5 = barra llena

        Debug.Log("Barra de vida actualizada: " + rellenoBarraVida.fillAmount); // Depuración
    }

    // Método para activar el Game Over
    public void ActivarGameOver()
    {
        if (panelGameOver != null)
        {
            panelGameOver.SetActive(true);  // Activa el Panel de Game Over
            Time.timeScale = 0f;  // Pausa el juego
        }
        else
        {
            Debug.LogError("Panel de Game Over no asignado.");
        }
    }
}



