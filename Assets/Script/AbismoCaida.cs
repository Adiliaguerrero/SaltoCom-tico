
using UnityEngine; // Importa las herramientas principales de Unity (Componentes, GameObject, Transform, etc.)
using UnityEngine.SceneManagement; // Permite gestionar escenas (cargar, reiniciar, etc.)
using System.Collections; // Necesario para usar corrutinas (funciones que permiten pausas temporales)


//Este clase permite que el personaje muera por un abismo de caida 
public class AbismoCaida : MonoBehaviour
{
    // Referencia al panel de Game Over (se activa cuando el jugador muere)
    public GameObject gameOverPanel;

    // Método OnTriggerEnter2D se llama automáticamente y se activa cuando otro objeto entra en el trigger de este objeto (abismo)
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
