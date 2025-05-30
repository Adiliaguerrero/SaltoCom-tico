using UnityEngine;
using UnityEngine.UI;

public class BarraVida : MonoBehaviour
{
    public Image rellenoBarraVida;
    private float VidaMaxima = 5f;

    public GameObject panelGameOver;     
    public GameObject imagenAEliminar;     

    public void ActualizarBarra(float vidaActual)
    {
        vidaActual = Mathf.Clamp(vidaActual, 0f, VidaMaxima);
        rellenoBarraVida.fillAmount = vidaActual / VidaMaxima;

        Debug.Log("Barra de vida actualizada: " + rellenoBarraVida.fillAmount);

        if (vidaActual <= 0f)
        {
            ActivarGameOver();
        }
    }

    public void ActivarGameOver()
    {
        if (panelGameOver != null)
        {
            panelGameOver.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            Debug.LogError("Panel de Game Over no asignado.");
        }

        if (imagenAEliminar != null)
        {
            Destroy(imagenAEliminar);  
        }
        else
        {
            Debug.LogWarning("Imagen a eliminar no asignada.");
        }
    }
}
