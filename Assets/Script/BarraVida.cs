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
    // Método público para la barra de vida  que actualizara visualmente con el daño.
    public void ActualizarBarra(float vidaActual)
    {
        vidaActual = Mathf.Clamp(vidaActual, 0f, VidaMaxima);

        rellenoBarraVida.fillAmount = vidaActual / VidaMaxima;

        // Muestra en consola el valor actualizado de la barra para depuración.
        Debug.Log("Barra de vida actualizada: " + rellenoBarraVida.fillAmount);

        // Si la vida llega a 0 o menos, activa la pantalla de Game Over.
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
        // Si el Game Over ya fue activado, evita que se ejecute otra vez.
        if (gameOverActivado) return;
        gameOverActivado = true;

        // Activa el panel de Game Over si está asignado en el Inspector.
        if (panelGameOver != null)
        {
            panelGameOver.SetActive(true);
        }
        else
        {
            // Si no está asignado, muestra un error en consola para que el desarrollador lo corrija.
            Debug.LogError("Panel de Game Over no asignado.");
        }

        // Destruye la imagen u objeto asignado para eliminarlo de la escena.
        if (imagenAEliminar != null)
        {
            Destroy(imagenAEliminar);
        }
        else
        {
            // Muestra una advertencia si no se asignó la imagen para eliminar.
            Debug.LogWarning("Imagen a eliminar no asignada.");
        }

        // Busca en la escena el objeto con el tag "Player" para destruirlo (simulando la muerte).
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Destroy(player);
        }
        else
        {
            // Si no se encuentra el jugador, avisa en consola.
            Debug.LogWarning("No se encontró un objeto con el tag 'Player'.");
        }
    }
}
