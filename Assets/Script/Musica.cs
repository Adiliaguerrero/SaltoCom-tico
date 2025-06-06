// Importamos los espacios de nombres necesarios de Unity
using UnityEngine;
using UnityEngine.UI;

// Esta clase controla la interaccion del botón que activa o desactiva la música del juego
public class BotonMusica : MonoBehaviour
{
    // Referencia al prefab que contiene el controlador de música
    public GameObject musicaManagerPrefab;  

    // Referencia al script MusicaManager que maneja la música de fondo
    private MusicaManager musicaManager;  

    // Referencia al botón que ejecuta la acción de alternar la música
    public Button boton; 

    // Método que se ejecuta cuando se instancia el objeto (antes de Start)
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

    // Método que se ejecuta al iniciar la escena (después de Awake)
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

    // Método que se ejecuta al hacer clic en el botón
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
