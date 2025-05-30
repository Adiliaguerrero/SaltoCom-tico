using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class PuntosManager : MonoBehaviour
{
    public static PuntosManager instancia;

    public TextMeshProUGUI BasicoCorrectoText;
    public TextMeshProUGUI BasicoIncorrectoText;
    public TextMeshProUGUI IntermedioCorrectoText;
    public TextMeshProUGUI IntermedioIncorrectoText;
    public TextMeshProUGUI AvanzadoCorrectoText;
    public TextMeshProUGUI AvanzadoIncorrectoText;

    public Button limpiarPuntajeButton;
    public Button cambiarEscenaButton;
    public string nombreEscena;
    public AudioClip botonSonido;  

    void Awake()
    {
        if (instancia != null && instancia != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instancia = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Start()
    {
        ActualizarReferencias();
        ActualizarPuntajes();

      
        if (limpiarPuntajeButton != null)
            limpiarPuntajeButton.onClick.AddListener(() => ReproducirSonido(LimpiarPuntaje));

        
        if (cambiarEscenaButton != null)
            cambiarEscenaButton.onClick.AddListener(() => ReproducirSonido(() => StartCoroutine(CambiarEscenaConSonido())));
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ActualizarReferencias();
        ActualizarPuntajes();
    }

    void ActualizarReferencias()
    {
       
        BasicoCorrectoText = GameObject.Find("BasicoCorrecto")?.GetComponent<TextMeshProUGUI>();
        BasicoIncorrectoText = GameObject.Find("BasicoIncorrecto")?.GetComponent<TextMeshProUGUI>();
        IntermedioCorrectoText = GameObject.Find("IntermedioCorrecto")?.GetComponent<TextMeshProUGUI>();
        IntermedioIncorrectoText = GameObject.Find("IntermedioIncorrecto")?.GetComponent<TextMeshProUGUI>();
        AvanzadoCorrectoText = GameObject.Find("AvanzadoCorrecto")?.GetComponent<TextMeshProUGUI>();
        AvanzadoIncorrectoText = GameObject.Find("AvanzadoIncorrecto")?.GetComponent<TextMeshProUGUI>();

        limpiarPuntajeButton = GameObject.Find("LimpiarPuntajeButton")?.GetComponent<Button>();
        cambiarEscenaButton = GameObject.Find("CambiarEscenaButton")?.GetComponent<Button>();

        if (limpiarPuntajeButton != null)
        {
            limpiarPuntajeButton.onClick.RemoveAllListeners();
            limpiarPuntajeButton.onClick.AddListener(() => ReproducirSonido(LimpiarPuntaje));
        }

        if (cambiarEscenaButton != null)
        {
            cambiarEscenaButton.onClick.RemoveAllListeners();
            cambiarEscenaButton.onClick.AddListener(() => ReproducirSonido(() => StartCoroutine(CambiarEscenaConSonido())));
        }
    }

    public void ActualizarPuntajes()
    {
        if (BasicoCorrectoText == null || BasicoIncorrectoText == null ||
            IntermedioCorrectoText == null || IntermedioIncorrectoText == null ||
            AvanzadoCorrectoText == null || AvanzadoIncorrectoText == null)
        {
            Debug.LogWarning("No se encontraron todos los textos necesarios en la escena.");
            return;
        }

        int puntajeCorrectoBasico = PlayerPrefs.GetInt("PuntajeCorrectoBasico", 0);
        int puntajeIncorrectoBasico = PlayerPrefs.GetInt("PuntajeIncorrectoBasico", 0);
        BasicoCorrectoText.text = puntajeCorrectoBasico.ToString();
        BasicoIncorrectoText.text = puntajeIncorrectoBasico.ToString();

        int puntajeCorrectoIntermedio = PlayerPrefs.GetInt("PuntajeCorrectoIntermedio", 0);
        int puntajeIncorrectoIntermedio = PlayerPrefs.GetInt("PuntajeIncorrectoIntermedio", 0);
        IntermedioCorrectoText.text = puntajeCorrectoIntermedio.ToString();
        IntermedioIncorrectoText.text = puntajeIncorrectoIntermedio.ToString();

        int puntajeCorrectoAvanzado = PlayerPrefs.GetInt("PuntajeCorrectoAvanzado", 0);
        int puntajeIncorrectoAvanzado = PlayerPrefs.GetInt("PuntajeIncorrectoAvanzado", 0);
        AvanzadoCorrectoText.text = puntajeCorrectoAvanzado.ToString();
        AvanzadoIncorrectoText.text = puntajeIncorrectoAvanzado.ToString();
    }

    public void LimpiarPuntaje()
    {
        PlayerPrefs.SetInt("PuntajeCorrectoBasico", 0);
        PlayerPrefs.SetInt("PuntajeIncorrectoBasico", 0);
        BasicoCorrectoText.text = "0";
        BasicoIncorrectoText.text = "0";

        PlayerPrefs.SetInt("PuntajeCorrectoIntermedio", 0);
        PlayerPrefs.SetInt("PuntajeIncorrectoIntermedio", 0);
        IntermedioCorrectoText.text = "0";
        IntermedioIncorrectoText.text = "0";

        PlayerPrefs.SetInt("PuntajeCorrectoAvanzado", 0);
        PlayerPrefs.SetInt("PuntajeIncorrectoAvanzado", 0);
        AvanzadoCorrectoText.text = "0";
        AvanzadoIncorrectoText.text = "0";
    }

    IEnumerator CambiarEscenaConSonido()
    {
        yield return new WaitForSeconds(botonSonido.length);
        if (!string.IsNullOrEmpty(nombreEscena))
            SceneManager.LoadScene(nombreEscena);
        else
            Debug.LogWarning("El nombre de la escena no est� especificado.");
    }

    void ReproducirSonido(System.Action accion)
    {
        if (botonSonido != null)
        {
            
        }
        accion?.Invoke();
    }
}
