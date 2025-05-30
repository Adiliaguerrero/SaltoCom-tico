using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public AudioClip sonidoBoton; 

    public void IrAInicio()
    {
        AudioManager.instancia.ReproducirSonido(sonidoBoton);
        StartCoroutine(CargarEscenaConDelay("Inicio", sonidoBoton.length));
    }

    public void IrALogros()
    {
        AudioManager.instancia.ReproducirSonido(sonidoBoton);
        StartCoroutine(CargarEscenaConDelay("Logros", sonidoBoton.length));
    }

    public void SalirDelJuego()
    {
        AudioManager.instancia.ReproducirSonido(sonidoBoton);
        StartCoroutine(SalirConDelay(sonidoBoton.length));
    }

    private System.Collections.IEnumerator CargarEscenaConDelay(string nombreEscena, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(nombreEscena);
    }

    private System.Collections.IEnumerator SalirConDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        Debug.Log("Saliendo del juego...");

        #if UNITY_EDITOR
            UnityEditor.EditorApplication.ExitPlaymode();
        #else
            Application.Quit();
        #endif
    }
}


