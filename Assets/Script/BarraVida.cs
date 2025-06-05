using UnityEngine;       // Librería principal de Unity para componentes y funciones base.
using UnityEngine.UI;    // Librería necesaria para trabajar con UI, en este caso la barra de vida (Image).

// Esta clase para controlar el sistema de vida del jugador y activar el obejto llamado game over
public class BarraVida : MonoBehaviour
{
    public Image rellenoBarraVida;  // Referencia al componente Image que representa el relleno de la barra de vida.

    private float VidaMaxima = 5f;  // Vida  que puede tener el jugador. Valor fijo.

    public GameObject panelGameOver;      // Panel UI que se muestra cuando el jugador pierde (Game Over).
    public GameObject imagenAEliminar;    // Imagen u objeto que debe eliminarse al perder.

    private bool gameOverActivado = false;  // Bandera boleana  para evitar activar Game Over varias veces.

    // Método público para la barra de vida  que actualizara visualmente con el daño.
    public void ActualizarBarra(float vidaActual)
    {
        // Limita el valor de vidaActual para que no sea menor que 0 ni mayor que VidaMaxima.
        vidaActual = Mathf.Clamp(vidaActual, 0f, VidaMaxima);

        // Actualiza el fillAmount es un componente  imagen que sirve para el relleno de vida del Image esto reflejara la vida  como porcentaje (0 a 1).
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
