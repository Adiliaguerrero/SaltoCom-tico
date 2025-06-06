using UnityEngine; 
using UnityEngine.SceneManagement; 
using System.Collections;

/// <summary>
/// Controla el comportamiento del abismo de caída. 
/// Si el jugador entra en contacto con esta zona, el personaje muere y se muestra el panel de Game Over.
/// </summary>
/// <remarks>
/// Esta clase debe ser añadida a un GameObject con un Collider2D marcado como "Trigger".
/// Se utiliza <see cref="OnTriggerEnter2D(Collider2D)"/> para detectar la colisión del jugador.
/// </remarks>
public class AbismoCaida : MonoBehaviour
{
    /// <summary>
    /// Referencia al panel de Game Over que se activa cuando el jugador cae en el abismo.
    /// </summary>
    public GameObject gameOverPanel;

    ///<summary>
    /// Se ejecuta automáticamente cuando otro objeto con un Collider2D entra en el área del trigger.
    /// Detecta si el jugador ha caído en el abismo.
    /// </summary>

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verificamos si el objeto que entra al abismo tiene la etiqueta "Player"
        if (other.CompareTag("Player"))
        {
            // Muestra un mensaje en la consola indicando que el jugador cayó
            Debug.Log("Jugador ha tocado la zona de muerte");

            // Destruye el objeto del jugador (lo elimina de la escena)
            Destroy(other.gameObject);

            // Inicia una corrutina que mostrará el panel de Game Over luego de 2 segundos
            StartCoroutine(ShowGameOver());
        }
    }

    // Corrutina que espera 2 segundos antes de mostrar el panel de Game Over
    IEnumerator ShowGameOver()
    {
        // Espera 2 segundos
        yield return new WaitForSeconds(2f);

        // Activa el panel de Game Over para que sea visible en pantalla
        gameOverPanel.SetActive(true);

        // Detiene el tiempo del juego (pausa todos los objetos y animaciones excepto la UI)
        Time.timeScale = 0f;
    }

    // Método público que se puede llamar desde un botón para reiniciar el juego
    public void RestartGame()
    {
        // Reactiva el tiempo del juego (lo normaliza)
        Time.timeScale = 1f;

        // Recarga la escena actual (reinicia el nivel)
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
