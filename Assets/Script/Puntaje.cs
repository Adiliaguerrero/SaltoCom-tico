using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

    /// <summary>
    /// Gestiona la puntuación de trivias, actualización de UI, reinicio de puntajes y navegación al menú.
    /// Implementa un patrón Singleton para persistencia entre escenas.
    /// </summary>
public class PuntosManager : MonoBehaviour
{
    /// <summary>
    /// Instancia estática única del gestor de puntos.
    /// </summary>
public static PuntosManager instancia;

        /// <summary>
        /// Texto que muestra el número de respuestas correctas en nivel básico.
        /// </summary>
    public TextMeshProUGUI BasicoCorrectoText;

        /// <summary>
        /// Texto que muestra el número de respuestas incorrectas en nivel básico.
        /// </summary>
    public TextMeshProUGUI BasicoIncorrectoText;

        /// <summary>
        /// Texto que muestra el número de respuestas correctas en nivel intermedio.
        /// </summary>
    public TextMeshProUGUI IntermedioCorrectoText;

        /// <summary>
        /// Texto que muestra el número de respuestas incorrectas en nivel intermedio.
        /// </summary>
    public TextMeshProUGUI IntermedioIncorrectoText;

        /// <summary>
        /// Texto que muestra el número de respuestas correctas en nivel avanzado.
        /// </summary>
    public TextMeshProUGUI AvanzadoCorrectoText;

        /// <summary>
        /// Texto que muestra el número de respuestas incorrectas en nivel avanzado.
        /// </summary>
    public TextMeshProUGUI AvanzadoIncorrectoText;

        /// <summary>
        /// Botón para limpiar los puntajes acumulados.
        /// </summary>
    public Button limpiarPuntajeButton;
        /// <summary>
        /// Botón para cambiar a otra escena.
        /// </summary>
    public Button cambiarEscenaButton;

        /// <summary>
        /// Clip de audio que se reproducirá al presionar un botón.
        /// </summary>
    public AudioClip botonSonido;

    
        /// <summary>
        /// Nombre de la escena a cargar cuando se presione el botón de cambiar escena.
        /// </summary>
    public string nombreEscena;

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

    void Start()
    {
        // Actualizamos las referencias a los objetos UI en la escena
        ActualizarReferencias();

        // Actualizamos los puntajes mostrados en los textos
        ActualizarPuntajes();
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene escena, LoadSceneMode modo)
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

        // Si el botón para limpiar puntaje existe, asignamos su evento con sonido y acción
        if (limpiarPuntajeButton != null)
        {
            limpiarPuntajeButton.onClick.RemoveAllListeners();
            limpiarPuntajeButton.onClick.AddListener(() => ReproducirSonido(LimpiarPuntaje));
        }

        // Si el botón para cambiar escena existe, asignamos su evento con sonido y acción
        if (cambiarEscenaButton != null)
        {
            cambiarEscenaButton.onClick.RemoveAllListeners();
            cambiarEscenaButton.onClick.AddListener(() => ReproducirSonido(CambiarEscena));
        }
    }

    // Metodo que actualiza o cambia los textos de puntajes con los valores guardados en PlayerPrefs
    /// <summary>
    /// Actualiza los textos en pantalla con los valores almacenados en PlayerPrefs.
    /// </summary>
    public void ActualizarPuntajes()
    {
        if (BasicoCorrectoText != null)
            BasicoCorrectoText.text = PlayerPrefs.GetInt("PuntajeCorrectoBasico", 0).ToString();

        if (BasicoIncorrectoText != null)
            BasicoIncorrectoText.text = PlayerPrefs.GetInt("PuntajeIncorrectoBasico", 0).ToString();

        if (IntermedioCorrectoText != null)
            IntermedioCorrectoText.text = PlayerPrefs.GetInt("PuntajeCorrectoIntermedio", 0).ToString();

        if (IntermedioIncorrectoText != null)
            IntermedioIncorrectoText.text = PlayerPrefs.GetInt("PuntajeIncorrectoIntermedio", 0).ToString();

        if (AvanzadoCorrectoText != null)
            AvanzadoCorrectoText.text = PlayerPrefs.GetInt("PuntajeCorrectoAvanzado", 0).ToString();

        if (AvanzadoIncorrectoText != null)
            AvanzadoIncorrectoText.text = PlayerPrefs.GetInt("PuntajeIncorrectoAvanzado", 0).ToString();
    }

        /// <summary>
        /// Reinicia todos los puntajes guardados en PlayerPrefs y actualiza la UI.
        /// </summary>
    public void LimpiarPuntaje()
    {
        PlayerPrefs.SetInt("PuntajeCorrectoBasico", 0);
        PlayerPrefs.SetInt("PuntajeIncorrectoBasico", 0);
        PlayerPrefs.SetInt("PuntajeCorrectoIntermedio", 0);
        PlayerPrefs.SetInt("PuntajeIncorrectoIntermedio", 0);
        PlayerPrefs.SetInt("PuntajeCorrectoAvanzado", 0);
        PlayerPrefs.SetInt("PuntajeIncorrectoAvanzado", 0);
        PlayerPrefs.Save();

        ActualizarPuntajes();
    }


    /// <summary>
    /// Carga la escena especificada en <see cref="nombreEscena"/>.
    /// </summary>
    public void CambiarEscena()
    {
        if (!string.IsNullOrEmpty(nombreEscena))
        {
            SceneManager.LoadScene(nombreEscena);
        }
    }

    void ReproducirSonido(System.Action accion)
    {
        if (botonSonido != null && AudioManager.instancia != null)
        {
            AudioManager.instancia.ReproducirSonido(botonSonido);
        }

        accion?.Invoke();
    }
}
