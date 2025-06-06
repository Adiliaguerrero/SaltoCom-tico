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
    /// 
    /// <param name="other">El Collider2D del objeto que entró en el área del trigger.
    /// </param>
    /// <remarks>
    /// Si el objeto tiene la etiqueta "Player", se elimina de la escena y se inicia la rutina de Game Over.
    /// </remarks>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Jugador ha tocado la zona de muerte");

            Destroy(other.gameObject);

            StartCoroutine(ShowGameOver());
        }
    }

    /// <summary>
    /// Corrutina que espera 2 segundos antes de mostrar el panel de Game Over.
    /// </summary>
    IEnumerator ShowGameOver()
    {
        yield return new WaitForSeconds(2f);

        gameOverPanel.SetActive(true);

        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
