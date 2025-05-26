using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalCambiarEscena : MonoBehaviour
{
    public string nombreEscenaDestino;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene(nombreEscenaDestino);
        }
    }
}
