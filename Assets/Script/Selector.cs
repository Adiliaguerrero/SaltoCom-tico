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
    // Array con los personajes disponibles (GameObjects con SpriteRenderer)
    public GameObject[] personajes;

    // Nombres correspondientes a cada personaje para mostrar en pantalla
    public string[] nombresPersonajes;

    // Imagen UI donde se mostrará el sprite del personaje seleccionado
    public Image imagenPersonaje;

    // Índice del personaje actualmente seleccionado
    private int indicePersonaje = 0;

    // Botones para navegar entre personajes y seleccionar
    public Button siguienteButton;
    public Button anteriorButton;
    public Button seleccionarButton;

    // Texto UI para mostrar el nombre del personaje seleccionado
    public TMP_Text nombrePersonajeTexto;

    [Header("Audio")]
    // Sonido que se reproduce al presionar los botones
    public AudioClip sonidoBoton;

    // Método para avanzar al siguiente personaje en la lista
    public void SiguientePersonaje()
    {
        ReproducirSonidoBoton();

        // Incrementa el índice y lo mantiene dentro del rango del array
        indicePersonaje = (indicePersonaje + 1) % personajes.Length;

        CambiarPersonaje();
    }

    // Método para retroceder al personaje anterior en la lista
    public void AnteriorPersonaje()
    {
        ReproducirSonidoBoton();

        // Decrementa el índice, asegurando que sea positivo y dentro del rango
        indicePersonaje = (indicePersonaje - 1 + personajes.Length) % personajes.Length;

        CambiarPersonaje();
    }

    // Actualiza la imagen y el nombre del personaje mostrado en pantalla
    private void CambiarPersonaje()
    {
        GameObject personajeSeleccionado = personajes[indicePersonaje];

        // Asignamos el sprite del personaje actual a la imagen UI
        imagenPersonaje.sprite = personajeSeleccionado.GetComponent<SpriteRenderer>().sprite;

        // Actualizamos el texto con el nombre del personaje, o "Desconocido" si no hay nombre asignado
        if (indicePersonaje < nombresPersonajes.Length)
        {
            nombrePersonajeTexto.text = nombresPersonajes[indicePersonaje];
        }
        else
        {
            nombrePersonajeTexto.text = "Desconocido";
        }
    }

    // Método para confirmar la selección del personaje y cargar la escena "Aventura"
    public void SeleccionarPersonaje()
    {
        ReproducirSonidoBoton();

        // Guardamos el índice del personaje seleccionado en PlayerPrefs para usarlo en la siguiente escena
        PlayerPrefs.SetInt("PersonajeSeleccionado", indicePersonaje);
        PlayerPrefs.Save();

        // Cargamos la escena de aventura
        SceneManager.LoadScene("Aventura");
    }

    // Inicialización: asignamos los métodos a los botones y actualizamos la UI para mostrar el personaje inicial
    void Start()
    {
        siguienteButton.onClick.AddListener(SiguientePersonaje);
        anteriorButton.onClick.AddListener(AnteriorPersonaje);
        seleccionarButton.onClick.AddListener(SeleccionarPersonaje);

        CambiarPersonaje();
    }

    // Reproduce un sonido de botón, si está asignado y hay un AudioManager disponible
    private void ReproducirSonidoBoton()
    {
        if (sonidoBoton != null && AudioManager.instancia != null)
        {
            AudioManager.instancia.ReproducirSonido(sonidoBoton);
        }
    }
}
