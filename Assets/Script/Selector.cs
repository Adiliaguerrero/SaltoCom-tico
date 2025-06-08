using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

    /// <summary>
    /// Permite al jugador seleccionar un personaje, mostrando su sprite e información,
    /// y luego carga la escena de aventura con el personaje elegido.
    /// </summary>
public class SelectorPersonaje : MonoBehaviour
{
        /// <summary>
        /// Lista de objetos de personajes disponibles para selección.
        /// </summary>
    public GameObject[] personajes;

        /// <summary>
        /// Nombres correspondientes a cada personaje para mostrar en la interfaz.
        /// </summary>
    public string[] nombresPersonajes;
    
    /// <summary>
    /// Imagen UI donde se muestra el sprite del personaje seleccionado.
    /// </summary>
    public Image imagenPersonaje;

    private int indicePersonaje = 0;
    
        /// <summary>
        /// Botón para ir al siguiente personaje.
        /// </summary>
    public Button siguienteButton;

        /// <summary>
        /// Botón para ir al personaje anterior.
        /// </summary>
    public Button anteriorButton;

        /// <summary>
        /// Botón para confirmar la selección del personaje actual.
        /// </summary>
    public Button seleccionarButton;

        /// <summary>
        /// Texto UI para mostrar el nombre del personaje seleccionado.
        /// </summary>
    public TMP_Text nombrePersonajeTexto;


    /// <summary>
    /// Sonido que se reproduce al interactuar con los botones.
    /// </summary>
    [Header("Audio")]
    public AudioClip sonidoBoton;

        /// <summary>
        /// Avanza al siguiente personaje disponible en el array de personajes.
        /// </summary>
    public void SiguientePersonaje()
    {
        ReproducirSonidoBoton();

        indicePersonaje = (indicePersonaje + 1) % personajes.Length;

        CambiarPersonaje();
    }

        /// <summary>
        /// Retrocede al personaje anterior en la lista de selección.
        /// </summary>
    public void AnteriorPersonaje()
    {
        ReproducirSonidoBoton();

        indicePersonaje = (indicePersonaje - 1 + personajes.Length) % personajes.Length;

        CambiarPersonaje();
    }

    private void CambiarPersonaje()
    {
        GameObject personajeSeleccionado = personajes[indicePersonaje];

        imagenPersonaje.sprite = personajeSeleccionado.GetComponent<SpriteRenderer>().sprite;

        if (indicePersonaje < nombresPersonajes.Length)
        {
            nombrePersonajeTexto.text = nombresPersonajes[indicePersonaje];
        }
        else
        {
            nombrePersonajeTexto.text = "Desconocido";
        }
    }


        /// <summary>
        /// Guarda el personaje seleccionado en PlayerPrefs y carga la escena de aventura.
        /// </summary>
    public void SeleccionarPersonaje()
    {
        ReproducirSonidoBoton();

        // Guardamos el índice del personaje seleccionado en PlayerPrefs para usarlo en la siguiente escena
        PlayerPrefs.SetInt("PersonajeSeleccionado", indicePersonaje);
        PlayerPrefs.Save();

        // Cargamos la escena de aventura
        SceneManager.LoadScene("Aventura");
    }

    void Start()
    {
        siguienteButton.onClick.AddListener(SiguientePersonaje);
        anteriorButton.onClick.AddListener(AnteriorPersonaje);
        seleccionarButton.onClick.AddListener(SeleccionarPersonaje);

        CambiarPersonaje();
    }

    private void ReproducirSonidoBoton()
    {
        if (sonidoBoton != null && AudioManager.instancia != null)
        {
            AudioManager.instancia.ReproducirSonido(sonidoBoton);
        }
    }
}
