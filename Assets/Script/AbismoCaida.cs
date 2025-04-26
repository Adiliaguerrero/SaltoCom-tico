using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class DeathZone : MonoBehaviour
{
    public GameObject gameOverPanel; // Panel de Game Over

    private void OnTriggerEnter2D(Collider2D other)
{
    if (other.CompareTag("Player"))
    {
        Debug.Log("Jugador ha tocado la zona de muerte");
        Destroy(other.gameObject); // Destruye al jugador
        StartCoroutine(ShowGameOver()); // Inicia la cuenta regresiva
    }
}


    IEnumerator ShowGameOver()
    {
        yield return new WaitForSeconds(2f); // Espera 3 segundos
        gameOverPanel.SetActive(true); // Muestra el Panel de Game Over
        Time.timeScale = 0f; // Pausa el juego
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // Reactiva el tiempo
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reinicia la escena
    }
}
