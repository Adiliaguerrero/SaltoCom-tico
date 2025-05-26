using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro; // Importar el espacio de nombres para TextMeshPro

public class SelectorPersonaje : MonoBehaviour
{
    public GameObject[] personajes;  
    public string[] nombresPersonajes;  
    public Image imagenPersonaje; 
    private int indicePersonaje = 0;  

    public Button siguienteButton;
    public Button anteriorButton;
    public Button seleccionarButton;

    public TMP_Text nombrePersonajeTexto;  
    
    public void SiguientePersonaje()
    {
        indicePersonaje = (indicePersonaje + 1) % personajes.Length;
        CambiarPersonaje();
    }

    
    public void AnteriorPersonaje()
    {
        indicePersonaje = (indicePersonaje - 1 + personajes.Length) % personajes.Length;
        CambiarPersonaje();
    }

     
    private void CambiarPersonaje()
    {
        GameObject personajeSeleccionado = personajes[indicePersonaje];
        imagenPersonaje.sprite = personajeSeleccionado.GetComponent<SpriteRenderer>().sprite; // Para sprites 2D

 
        if (indicePersonaje < nombresPersonajes.Length)
        {
            nombrePersonajeTexto.text = nombresPersonajes[indicePersonaje];
        }
        else
        {
            nombrePersonajeTexto.text = "Desconocido";  
        }
    }

    //  seleccionar el personaje y cargar la siguiente escena
    public void SeleccionarPersonaje()
    {
        PlayerPrefs.SetInt("PersonajeSeleccionado", indicePersonaje);
        PlayerPrefs.Save();

        SceneManager.LoadScene("Aventura");
    }

    
    void Start()
    {
        siguienteButton.onClick.AddListener(SiguientePersonaje);
        anteriorButton.onClick.AddListener(AnteriorPersonaje);
        seleccionarButton.onClick.AddListener(SeleccionarPersonaje);

        CambiarPersonaje();  
    }
}

