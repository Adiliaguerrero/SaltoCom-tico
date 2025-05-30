using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

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

    [Header("Audio")]
    public AudioClip sonidoBoton; 

    public void SiguientePersonaje()
    {
        ReproducirSonidoBoton();
        indicePersonaje = (indicePersonaje + 1) % personajes.Length;
        CambiarPersonaje();
    }

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

    public void SeleccionarPersonaje()
    {
        ReproducirSonidoBoton();

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

    private void ReproducirSonidoBoton()
    {
        if (sonidoBoton != null && AudioManager.instancia != null)
        {
            AudioManager.instancia.ReproducirSonido(sonidoBoton);
        }
    }
}

