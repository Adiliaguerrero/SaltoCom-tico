using UnityEngine;                // Importa la biblioteca principal de Unity para funciones básicas del motor.
using UnityEngine.UI;            // Importa la biblioteca de UI, necesaria para trabajar con botones y otros elementos gráficos.

// Esta clase controla un botón que permite activar/desactivar el sonido y reproducir un efecto de sonido cuando se pulsa.
public class BotonClipControl : MonoBehaviour
{
    public Button botonSonido;           // Referencia al botón en la escena que controlará el sonido.
    public AudioClip sonidoBoton;        // Referencia de un Clip de audio que se reproducirá cuando se presione el botón.

    //  Método Awake se ejecuta al inicio, antes de Start o que comienze el juego , ideal para preparar una funcion en la escena.
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

    // Se llama al inicio de la escena, justo después de Awake. Aquí se configura el botón.
    private void Start()
    {
        if (botonSonido != null)
            // Si el botón está asignado, le agrega un listener (evento) que llama al método ToggleSonido cuando se presiona.
            botonSonido.onClick.AddListener(ToggleSonido);
        else
            // Si no se asignó ningún botón, se muestra una advertencia en la consola.
            Debug.LogWarning("Botón de sonido no asignado.");
    }

    // Este método se llama cuando el botón es presionado. Alterna el estado del sonido y reproduce un clip.
    private void ToggleSonido()
    {
        // Obtiene el estado actual del sonido (activo o no) desde el AudioManager.
        bool sonidoActivo = AudioManager.instancia.ObtenerEstadoSonido();

        // Cambia el estado del sonido al valor opuesto (si estaba activo, lo desactiva y viceversa).
        AudioManager.instancia.CambiarEstadoSonido(!sonidoActivo);

        // Si hay un clip asignado, se reproduce usando el AudioManager.
        if (sonidoBoton != null)
        {
            AudioManager.instancia.ReproducirSonido(sonidoBoton);
        }
    }
}
