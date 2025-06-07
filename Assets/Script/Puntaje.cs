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

    // Referencias a botones para limpiar puntajes y cambiar de escena
    public Button limpiarPuntajeButton;
    public Button cambiarEscenaButton;

    // Clip de audio que se reproducirá al presionar un botón
    public AudioClip botonSonido;

    // Nombre de la escena a cargar al cambiar escena
    public string nombreEscena;

    // Método llamado automáticamente por Unity al instanciar este objeto
    void Awake()
    {
        // Verificamos si ya existe una instancia para mantener solo una
        if (instancia != null && instancia != this)
        {
            Destroy(gameObject); // Destruye duplicados
        }
        else
        {
            instancia = this;
            DontDestroyOnLoad(gameObject); // Persistencia entre escenas
        }
    }

    // Método llamado cuando la escena inicia o el objeto se activa
    void Start()
    {
        // Actualizamos las referencias a los objetos UI en la escena
        ActualizarReferencias();

        // Actualizamos los puntajes mostrados en los textos
        ActualizarPuntajes();
    }

    // Método para suscribirnos al evento de carga de escena
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Método para desuscribirnos del evento de carga de escena (cuando se desactiva el objeto)
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Método que se ejecuta cada vez que se carga una nueva escena
    void OnSceneLoaded(Scene escena, LoadSceneMode modo)
    {
        // Actualizamos las referencias a los objetos UI en la nueva escena
        ActualizarReferencias();

        // Actualizamos los puntajes en la UI con los datos guardados
        ActualizarPuntajes();
    }

    // Método para buscar los objetos en la escena y obtener sus componentes
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

    // Método para limpiar los puntajes guardados en PlayerPrefs y actualizar la UI
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

    // Método para cambiar la escena actual por la escena indicada en nombreEscena
    public void CambiarEscena()
    {
        if (!string.IsNullOrEmpty(nombreEscena))
        {
            SceneManager.LoadScene(nombreEscena);
        }
    }

    // Método que reproduce el sonido y luego ejecuta la acción indicada (limpiar o cambiar escena)
    void ReproducirSonido(System.Action accion)
    {
        if (botonSonido != null && AudioManager.instancia != null)
        {
            AudioManager.instancia.ReproducirSonido(botonSonido);
        }

        accion?.Invoke();
    }
}
