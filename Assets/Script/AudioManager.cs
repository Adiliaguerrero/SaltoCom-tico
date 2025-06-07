using UnityEngine;  

/// <summary>
/// Gestiona globalmente el sistema de sonido del juego.
/// Permite activar o desactivar sonidos, reproducir clips de audio y guardar preferencias del jugador.
/// </summary>
public class AudioManager : MonoBehaviour
{
        /// <summary>
        /// Determina si los sonidos están habilitados o deshabilitados.
        /// </summary>.
    private bool sonidosActivos = true; 

        /// <summary>
        /// Instancia única del AudioManager en la escena (patrón Singleton).
        /// </summary>
    public static AudioManager instancia; 

        /// <summary>
        /// Componente que permite reproducir sonidos en la escena.
        /// </summary>
    private AudioSource audioSource;


        /// <summary>
        /// Awake se ejecuta cuando se instancia el objeto en la escena, antes de Start.
        /// Se ejecuta al iniciar el objeto. Establece el Singleton, configura el AudioSource y carga preferencias.
        /// </summary>
    private void Awake()
    {
        if (instancia == null)
        {
            instancia = this;

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return; // Sale del método para evitar ejecutar el resto del código.
        }

        audioSource = gameObject.AddComponent<AudioSource>();

        CargarEstadoSonido();
    }

        /// <summary>
        /// Carga la preferencia de sonido del jugador desde PlayerPrefs.
        /// Si no hay preferencia guardada, los sonidos se activan por defecto.
        /// </summary>
    private void CargarEstadoSonido()
    {
        // Verifica si hay una clave guardada llamada "SonidosActivos"
        if (PlayerPrefs.HasKey("SonidosActivos"))
        {
            sonidosActivos = PlayerPrefs.GetInt("SonidosActivos") == 1;
        }
        else
        {
            sonidosActivos = true;
        }
    }

        /// <summary>
        /// Cambia el estado del sonido (activado o desactivado) y guarda la preferencia en PlayerPrefs.
        /// </summary>
        /// <param name="estado">Nuevo estado del sonido: <c>true</c> para activar, <c>false</c> para desactivar.
        /// </param>
    public void CambiarEstadoSonido(bool estado)
    {
        sonidosActivos = estado;

        PlayerPrefs.SetInt("SonidosActivos", sonidosActivos ? 1 : 0);
        PlayerPrefs.Save(); // Asegura que la preferencia se escriba en disco.
    }


        /// <summary>
        /// Reproduce un clip de sonido si los sonidos están activos y el clip no es nulo.
        /// </summary>
        /// <param name="clip">Clip de audio a reproducir.</param>
    public void ReproducirSonido(AudioClip clip)
    {
        if (sonidosActivos && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    /// <summary>
    /// Obtiene el estado actual de los sonidos.
    /// </summary>
    /// <returns>
    /// <c>true</c> si los sonidos están activos; de lo contrario, <c>false</c>.
    /// </returns>
    // Devuelve el estado actual de los sonidos (true = activos, false = inactivos).
    public bool ObtenerEstadoSonido()
    {
        return sonidosActivos;
    }
}
