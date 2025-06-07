using UnityEngine;


    /// <summary>
    /// Esta clase gestiona la música de fondo global en el juego.
    /// Implementa el patrón Singleton para mantenerse persistente entre escenas.
    /// </summary>
public class MusicaManager : MonoBehaviour
{
    /// <summary>
    /// Instancia estática del <see cref="MusicaManager"/> para aplicar el patrón Singleton.
    /// </summary>
    public static MusicaManager instancia;

    /// <summary>
    /// Componente <see cref="AudioSource"/> que reproduce la música de fondo.
    /// </summary>
    public AudioSource musicaFondo;

    /// <summary>
    /// Variable booleana que indica si la música está activa o no.
    /// </summary>
    private bool musicaActiva;

    /// <summary>
    /// Se ejecuta al cargar el objeto. Inicializa la instancia Singleton y aplica el estado de la música.
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
            return;
        }

        musicaActiva = PlayerPrefs.GetInt("MusicaActiva", 1) == 1;

        AplicarEstadoMusica();
    }

        /// <summary>
        /// Alterna entre activar y desactivar la música de fondo,
        /// y guarda el estado en las preferencias del jugador.
        /// </summary>
    public void AlternarMusica()
    {
        musicaActiva = !musicaActiva;

        PlayerPrefs.SetInt("MusicaActiva", musicaActiva ? 1 : 0);
        PlayerPrefs.Save();

        AplicarEstadoMusica();
    }

        /// <summary>
        /// Aplica el estado actual de la música al componente <see cref="AudioSource"/>.
        /// </summary>
    private void AplicarEstadoMusica()
    {
        if (musicaFondo != null)
        {
            if (musicaActiva)
            {
                if (!musicaFondo.isPlaying) musicaFondo.Play();

                musicaFondo.volume = 1f;
            }
            else
            {
                musicaFondo.volume = 0f;
            }
        }
    }
}
