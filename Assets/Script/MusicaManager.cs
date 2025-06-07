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

    // Método Awake que se ejecuta al cargar el objeto (antes de Start)
    /// <summary>
    /// Se ejecuta al cargar el objeto. Inicializa la instancia Singleton y aplica el estado de la música.
    /// </summary>
    private void Awake()
    {
        // Si no hay una instancia activa, esta se convierte en la principal
        if (instancia == null)
        {
            instancia = this;

            // Evita que este objeto se destruya al cambiar de escena
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Si ya existe una instancia, destruye esta para evitar duplicados
            Destroy(gameObject);
            return;
        }

        // Carga el estado guardado de la música (1 = activa, 0 = desactivada)
        musicaActiva = PlayerPrefs.GetInt("MusicaActiva", 1) == 1;

        // Aplica el estado guardado a la música de fondo
        AplicarEstadoMusica();
    }

    // Método público que alterna el estado de la música (encendida/apagada)
    public void AlternarMusica()
    {
        // Cambia el estado booleano a su valor opuesto
        musicaActiva = !musicaActiva;

        // Guarda el nuevo estado en PlayerPrefs para que se mantenga entre sesiones
        PlayerPrefs.SetInt("MusicaActiva", musicaActiva ? 1 : 0);
        PlayerPrefs.Save();

        // Aplica el nuevo estado a la música de fondo
        AplicarEstadoMusica();
    }

    // Método privado que aplica el estado de la música al componente AudioSource
    private void AplicarEstadoMusica()
    {
        // Verifica que el componente de música esté asignado
        if (musicaFondo != null)
        {
            // Si la música está activa
            if (musicaActiva)
            {
                // Si no está sonando, la reproduce
                if (!musicaFondo.isPlaying) musicaFondo.Play();

                // Asegura que el volumen esté al máximo
                musicaFondo.volume = 1f;
            }
            else
            {
                // Si la música está desactivada, baja el volumen a cero
                musicaFondo.volume = 0f;
            }
        }
    }
}
