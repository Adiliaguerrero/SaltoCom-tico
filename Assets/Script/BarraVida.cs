using UnityEngine;       
using UnityEngine.UI;


    /// <summary>
    /// Controla el sistema de vida del jugador y activa el panel de Game Over cuando la vida llega a cero.
    /// </summary>
public class BarraVida : MonoBehaviour
{
        /// <summary>
        /// Imagen que representa el relleno de la barra de vida en la interfaz gráfica.
        /// </summary>
    public Image rellenoBarraVida;  // Referencia al componente Image que representa el relleno de la barra de vida.

        /// <summary>
        /// Valor máximo de vida que puede tener el jugador.
        /// </summary>
    private float VidaMaxima = 5f;  

        /// <summary>
        /// Panel de Game Over que se muestra cuando el jugador pierde.
        /// </summary>
    public GameObject panelGameOver;      

        /// <summary>
        /// Imagen u objeto que debe ser eliminado al activarse el Game Over.
        /// </summary>
    public GameObject imagenAEliminar;    

        /// <summary>
        /// Bandera booleana para evitar que se active el Game Over más de una vez.
        /// </summary>
    private bool gameOverActivado = false; 

        /// <summary>
        /// Actualiza visualmente la barra de vida del jugador.
        /// Si la vida es igual o menor a cero, se activa el panel de Game Over.
        /// </summary>
        /// <param name="vidaActual">Valor actual de la vida del jugador.
        /// </param>
    public void ActualizarBarra(float vidaActual)
    {
        vidaActual = Mathf.Clamp(vidaActual, 0f, VidaMaxima);

        rellenoBarraVida.fillAmount = vidaActual / VidaMaxima;

        // Muestra en consola el valor actualizado de la barra para depuración.
        Debug.Log("Barra de vida actualizada: " + rellenoBarraVida.fillAmount);

        if (vidaActual <= 0f)
        {
            ActivarGameOver();
        }
    }

    // Método para activar la pantalla de Game Over y realizar acciones relacionadas.
    
        /// <summary>
        /// Activa la interfaz de Game Over, destruye objetos asociados y elimina al jugador de la escena.
        /// </summary>
    public void ActivarGameOver()
    {
        if (gameOverActivado) return;
        gameOverActivado = true;

        if (panelGameOver != null)
        {
            panelGameOver.SetActive(true);
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

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Destroy(player);
        }
        else
        {
            Debug.LogWarning("No se encontró un objeto con el tag 'Player'.");
        }
    }
}
