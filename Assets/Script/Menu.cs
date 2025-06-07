using UnityEngine;                       
using UnityEngine.SceneManagement;

// Esta clase controla las acciones del menú principal, como cambiar de escena y salir del juego
    /// <summary>
    /// Esta clase controla las acciones del menú principal, como cambiar de escena y salir del juego.
    /// </summary>
public class MenuController : MonoBehaviour
{
    /// <summary>
    /// Sonido que se reproduce al pulsar un botón del menú.
    /// </summary>
    public AudioClip sonidoBoton;

    // Método público que se llama para ir a la escena de selección de niveles o inicio
    public void IrAInicio()
    {
        // Reproduce el sonido del botón usando el AudioManager singleton
        AudioManager.instancia.ReproducirSonido(sonidoBoton);

        // Inicia una corutina para cargar la escena llamada "Dialogo" después de esperar el tiempo que dura el sonido
        StartCoroutine(CargarEscenaConDelay("Dialogo", sonidoBoton.length));
    }

    // Método público que se llama para ir a la escena de logros o estadísticas del juego
    public void IrALogros()
    {
        // Reproduce el sonido del botón
        AudioManager.instancia.ReproducirSonido(sonidoBoton);

        // Inicia la corutina para cargar la escena llamada "Logros" tras esperar el tiempo del sonido
        StartCoroutine(CargarEscenaConDelay("Logros", sonidoBoton.length));
    }

    // Método público para salir del juego cuando el jugador lo solicita
    public void SalirDelJuego()
    {
        // Reproduce el sonido del botón antes de salir
        AudioManager.instancia.ReproducirSonido(sonidoBoton);

        // Inicia la corutina que cerrará el juego después de esperar la duración del sonido
        StartCoroutine(SalirConDelay(sonidoBoton.length));
    }

    // Corutina privada para cargar una escena con un retraso específico
    // Recibe el nombre de la escena y el tiempo que debe esperar antes de cargarla
    private System.Collections.IEnumerator CargarEscenaConDelay(string nombreEscena, float delay)
    {
        // Espera el tiempo especificado (delay) en segundos antes de continuar
        yield return new WaitForSeconds(delay);

        // Carga la escena cuyo nombre se pasó como parámetro
        SceneManager.LoadScene(nombreEscena);
    }

    // Corutina privada para salir del juego con un retraso
    // Recibe el tiempo que debe esperar antes de cerrar la aplicación
    private System.Collections.IEnumerator SalirConDelay(float delay)
    {
        // Espera el tiempo especificado antes de salir
        yield return new WaitForSeconds(delay);

        // Muestra un mensaje en la consola para indicar que se está saliendo del juego (útil para pruebas)
        Debug.Log("Saliendo del juego...");

        // Condicional para detectar si estamos en el editor de Unity o en la aplicación compilada
#if UNITY_EDITOR
        // Si estamos en el editor, detenemos el modo de juego sin cerrar el editor
        UnityEditor.EditorApplication.ExitPlaymode();
#else
            // Cerramos la aplicación (funciona en Android y PC)
            Application.Quit();
#endif
    }
}
