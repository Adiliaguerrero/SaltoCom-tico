using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class DeathZone : MonoBehaviour
{
    public GameObject gameOverPanel;  

    private void OnTriggerEnter2D(Collider2D other)
{
    if (other.CompareTag("Player"))
    {
        Debug.Log("Jugador ha tocado la zona de muerte");
        Destroy(other.gameObject);  
        StartCoroutine(ShowGameOver());  
    }
}


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
