using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void CambiarEscena(string nombreEscena)
    {
        SceneManager.LoadScene(nombreEscena);
    }

    public void SalirDelJuego()
    {
        Debug.Log("Saliendo del juego...");

        #if UNITY_EDITOR
            UnityEditor.EditorApplication.ExitPlaymode(); //  salir en el Editor de Unity
        #else
            Application.Quit(); // Salir juego app
        #endif
    }
}

