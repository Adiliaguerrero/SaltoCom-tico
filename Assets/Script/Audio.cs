using UnityEngine;               
using UnityEngine.UI;            

    /// <summary>
    /// Esta clase controla un botón que permite activar/desactivar el sonido y reproducir un efecto de sonido cuando se pulsa.
    /// </summary>

public class BotonClipControl : MonoBehaviour

    
{
        /// <summary>
        /// Referencia al botón que se utiliza para controlar el sonido.
        /// </summary>
    public Button botonSonido;

        /// <summary>
        /// Clip de sonido que se reproduce cuando se presiona el botón.
        /// </summary>           
    public AudioClip sonidoBoton;  

    //  Método Awake se ejecuta al inicio, antes de Start o que comienze el juego.

        /// <summary>
        /// Inicializa el AudioManager si no existe en la escena.
        /// Se ejecuta antes de <see cref="Start"/> y es útil para configuración temprana.
        /// </summary>
    private void Awake()
    {
        // Verifica si ya existe una instancia u obejeto del AudioManager en la escena.
        if (AudioManager.instancia == null)
        {
            // Si no existe, crea un nuevo GameObject llamado "AudioManager".
            GameObject audioManagerObj = new GameObject("AudioManager");

            // Le agrega el componente AudioManager para controlar el sonido global.
            audioManagerObj.AddComponent<AudioManager>();

            // Se asegura de que el AudioManager no se destruya al cambiar de escena.
            DontDestroyOnLoad(audioManagerObj);
        }
    }

        /// <summary>
        /// Se llama al inicio de la escena, justo después de Awake. Aquí se configura el botón.
        /// </summary>
    
    private void Start()
    {
        if (botonSonido != null)
            botonSonido.onClick.AddListener(ToggleSonido);
        else
            Debug.LogWarning("Botón de sonido no asignado.");
    }

        /// <summary>
        /// Alterna el estado del sonido global y reproduce un clip de audio si está asignado.
        /// </summary>
    private void ToggleSonido()
    {
        bool sonidoActivo = AudioManager.instancia.ObtenerEstadoSonido();

        AudioManager.instancia.CambiarEstadoSonido(!sonidoActivo);

        if (sonidoBoton != null)
        {
            AudioManager.instancia.ReproducirSonido(sonidoBoton);
        }
    }
}
