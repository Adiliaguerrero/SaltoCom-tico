using UnityEngine;
using UnityEngine.UI;

    /// <summary>
    /// Esta clase controla la interacción del botón que activa o desactiva la música del juego.
    /// </summary>
public class BotonMusica : MonoBehaviour
{
    /// <summary>
    /// Prefab que contiene el controlador de música a instanciar si no existe.
    /// </summary>
    public GameObject musicaManagerPrefab;

    /// <summary>
    /// Referencia al script que gestiona la música de fondo en el juego.
    /// </summary>
    private MusicaManager musicaManager;

    /// <summary>
    /// Referencia al componente Button que ejecuta la acción de alternar la música.
    /// </summary>
    public Button boton;

    
    /// <summary>
    /// Se ejecuta al instanciar el objeto, antes de Start.
    /// </summary>
    private void Awake()
    {
        // Verifica si no existe una instancia de MusicaManager y si se asignó un prefab
        if (MusicaManager.instancia == null && musicaManagerPrefab != null)
        {
            // Instancia el prefab del manager de música para asegurar que esté en la escena
            Instantiate(musicaManagerPrefab);
        }

        // Obtiene la instancia actual del MusicaManager
        musicaManager = MusicaManager.instancia;
    }

    /// <summary>
    /// Se ejecuta al comenzar la escena, después de Awake.
    /// </summary>
    private void Start()
    {
        // Si no se asignó el botón desde el Inspector, lo busca automáticamente
        if (boton == null)
        {
            boton = GetComponent<Button>();
        }

        // Asigna el método OnClick al evento del botón
        boton.onClick.AddListener(OnClick);
    }

    /// <summary>
    /// Método que se llama al hacer clic en el botón para alternar el estado de la música.
    /// </summary>
    public void OnClick()
    {
        // Si hay una instancia válida del manager de música
        if (musicaManager != null)
        {
            // Llama al método que alterna (activa o desactiva) la música
            musicaManager.AlternarMusica();
        }
    }
}
