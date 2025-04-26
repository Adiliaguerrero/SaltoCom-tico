using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro; // Importar el espacio de nombres para TextMeshPro

public class SelectorPersonaje : MonoBehaviour
{
    public GameObject[] personajes; // Array de prefabs de personajes
    public string[] nombresPersonajes; // Array de nombres personalizados
    public Image imagenPersonaje; // Imagen para mostrar el personaje
    private int indicePersonaje = 0; // Índice del personaje seleccionado

    public Button siguienteButton;
    public Button anteriorButton;
    public Button seleccionarButton;

    public TMP_Text nombrePersonajeTexto; // Campo de texto para el nombre

    // Método para cambiar al siguiente personaje
    public void SiguientePersonaje()
    {
        indicePersonaje = (indicePersonaje + 1) % personajes.Length;
        CambiarPersonaje();
    }

    // Método para cambiar al personaje anterior
    public void AnteriorPersonaje()
    {
        indicePersonaje = (indicePersonaje - 1 + personajes.Length) % personajes.Length;
        CambiarPersonaje();
    }

    // Método para cambiar el personaje mostrado en la UI
    private void CambiarPersonaje()
    {
        GameObject personajeSeleccionado = personajes[indicePersonaje];
        imagenPersonaje.sprite = personajeSeleccionado.GetComponent<SpriteRenderer>().sprite; // Para sprites 2D

        // Actualizar el nombre del personaje usando el array de nombres personalizados
        if (indicePersonaje < nombresPersonajes.Length)
        {
            nombrePersonajeTexto.text = nombresPersonajes[indicePersonaje];
        }
        else
        {
            nombrePersonajeTexto.text = "Desconocido"; // En caso de que falte un nombre
        }
    }

    // Método para seleccionar el personaje y cargar la siguiente escena
    public void SeleccionarPersonaje()
    {
        PlayerPrefs.SetInt("PersonajeSeleccionado", indicePersonaje);
        PlayerPrefs.Save();

        SceneManager.LoadScene("Escenario1");
    }

    // Start is called before the first frame update
    void Start()
    {
        siguienteButton.onClick.AddListener(SiguientePersonaje);
        anteriorButton.onClick.AddListener(AnteriorPersonaje);
        seleccionarButton.onClick.AddListener(SeleccionarPersonaje);

        CambiarPersonaje(); // Mostrar el primer personaje al inicio
    }
}

