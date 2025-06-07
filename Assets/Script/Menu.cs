using UnityEngine;                       
using UnityEngine.SceneManagement;

    /// <summary>
    /// Esta clase controla las acciones del menú principal, como cambiar de escena y salir del juego.
    /// </summary>
public class MenuController : MonoBehaviour
{
        /// <summary>
        /// Sonido que se reproduce al pulsar un botón del menú.
        /// </summary>
    public AudioClip sonidoBoton;

    
        /// <summary>
        /// Método que se llama para ir a la escena de introducción o selección de niveles.
        /// </summary>
    public void IrAInicio()
    {
        AudioManager.instancia.ReproducirSonido(sonidoBoton);

        StartCoroutine(CargarEscenaConDelay("Dialogo", sonidoBoton.length));
    }

        /// <summary>
        /// Método que se llama para ir a la escena de logros o estadísticas del juego.
        /// </summary>
    public void IrALogros()
    {
        AudioManager.instancia.ReproducirSonido(sonidoBoton);

        StartCoroutine(CargarEscenaConDelay("Logros", sonidoBoton.length));
    }

    
        /// <summary>
        /// Método que se llama para salir del juego.
        /// </summary>
    public void SalirDelJuego()
    {
        AudioManager.instancia.ReproducirSonido(sonidoBoton);

        StartCoroutine(SalirConDelay(sonidoBoton.length));
    }



    /// <summary>
    /// Corrutina que espera un tiempo específico antes de cargar una escena.
    /// </summary>
    /// <param name="nombreEscena">Nombre de la escena a cargar.</param>
    /// <param name="delay">Duración de la espera antes de cargar la escena.</param>
    /// <returns>IEnumerator para control de la corrutina.</returns>
    private System.Collections.IEnumerator CargarEscenaConDelay(string nombreEscena, float delay)
    {
        yield return new WaitForSeconds(delay);

        SceneManager.LoadScene(nombreEscena);
    }


    /// <summary>
    /// Corrutina que espera un tiempo antes de cerrar la aplicación.
    /// </summary>
    /// <param name="delay">Duración de la espera antes de cerrar la aplicación.</param>
    /// <returns>
    /// IEnumerator para control de la corrutina.
    /// </returns>
    private System.Collections.IEnumerator SalirConDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        Debug.Log("Saliendo del juego...");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.ExitPlaymode();
#else
            // Cerramos la aplicación (funciona en Android y PC)
            Application.Quit();
#endif
    }
}
