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

    public static AudioManager instancia;  // Singleton: referencia estática a la única instancia de AudioManager.

    private AudioSource audioSource;  // Componente que reproduce los clips de audio.

    // Awake se ejecuta cuando se instancia el objeto en la escena, antes de Start.
    private void Awake()
    {
        // Si no existe ninguna instancia del AudioManager, esta se convierte en la principal.
        if (instancia == null)
        {
            instancia = this;

            // Evita que este objeto se destruya al cambiar de escena.
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Si ya existe una instancia, destruye esta para mantener una sola.
            Destroy(gameObject);
            return; // Sale del método para evitar ejecutar el resto del código.
        }

        // Agrega automáticamente un componente AudioSource al objeto, si no lo tiene.
        audioSource = gameObject.AddComponent<AudioSource>();

        // Carga el estado del sonido guardado en las preferencias del jugador.
        CargarEstadoSonido();
    }

    // Este método verifica si existe una preferencia guardada y la carga. Si no, activa sonidos por defecto.
    private void CargarEstadoSonido()
    {
        // Verifica si hay una clave guardada llamada "SonidosActivos"
        if (PlayerPrefs.HasKey("SonidosActivos"))
        {
            // Si existe, asigna el valor (1 = true, 0 = false)
            sonidosActivos = PlayerPrefs.GetInt("SonidosActivos") == 1;
        }
        else
        {
            // Si no existe la clave, se activan los sonidos por defecto.
            sonidosActivos = true;
        }
    }

    // Método público para activar o desactivar el sonido y guardar la preferencia del jugador.
    public void CambiarEstadoSonido(bool estado)
    {
        sonidosActivos = estado;

        // Guarda el estado como entero: 1 si está activo, 0 si está inactivo.
        PlayerPrefs.SetInt("SonidosActivos", sonidosActivos ? 1 : 0);
        PlayerPrefs.Save(); // Asegura que la preferencia se escriba en disco.
    }

    // Reproduce un clip de sonido si los sonidos están activos y el clip no es nulo.
    public void ReproducirSonido(AudioClip clip)
    {
        if (sonidosActivos && clip != null)
        {
            // PlayOneShot reproduce el clip una vez sin interrumpir otros sonidos.
            audioSource.PlayOneShot(clip);
        }
    }

    // Devuelve el estado actual de los sonidos (true = activos, false = inactivos).
    public bool ObtenerEstadoSonido()
    {
        return sonidosActivos;
    }
}
