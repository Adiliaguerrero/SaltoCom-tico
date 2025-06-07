
using UnityEngine;          
using UnityEngine.UI;

// Esta clase controla la gestion de configuración y créditos, así como los botones para abrirlos y cerrarlos
    /// <summary>
    /// Controla la visualización de los paneles de configuración y créditos,
    /// así como la asignación de eventos a los botones respectivos.
    /// </summary>
    /// <remarks>
    /// Este script debe estar asignado a un GameObject en la escena.
    /// Asegúrate de asignar correctamente los objetos del Inspector antes de ejecutar.
    /// </remarks>
    /// <seealso cref="AudioManager"/>
public class ConfiguracionUI : MonoBehaviour
{

    /// <summary>
    /// Panel que contiene las opciones de configuración.
    /// </summary>
    /// <value>Debe estar desactivado al inicio del juego.
    /// </value>
    public GameObject panelConfiguracion;


    /// <summary>
    /// Botón que abre el panel de configuración.
    /// </summary>
    public Button botonAbrir;

    
    /// <summary>
    /// Botón que cierra el panel de configuración.
    /// </summary>
    public Button botonCerrar;


    /// <summary>
    /// Panel que muestra los créditos del juego.
    /// </summary>
    /// <value>
    /// Se recomienda desactivarlo en el inicio del juego.
    /// </value>
    public GameObject panelCreditos;


    /// <summary>
    /// Botón que abre el panel de créditos.
    /// </summary>
    public Button botonCreditosAbrir;

   
    /// <summary>
    /// Botón que cierra el panel de créditos.
    /// </summary>
    public Button botonCreditosCerrar;


    /// <summary>
    /// Sonido que se reproduce al presionar cualquier botón.
    /// </summary>
    /// <seealso cref="AudioManager.ReproducirSonido(AudioClip)"/>
    public AudioClip sonidoBoton;


        /// <summary>
        /// Se ejecuta al iniciar el objeto y configura la visibilidad de los paneles
        /// y la asignación de eventos a los botones.
        /// </summary>
        /// <remarks>
        /// Este método desactiva inicialmente los paneles y vincula los métodos a los botones.
        /// </remarks>
    void Start()
    {
        panelConfiguracion.SetActive(false);

        panelCreditos.SetActive(false);

        botonAbrir.onClick.AddListener(AbrirConfiguracion);

        botonCerrar.onClick.AddListener(CerrarConfiguracion);

        botonCreditosAbrir.onClick.AddListener(AbrirCreditos);

        botonCreditosCerrar.onClick.AddListener(CerrarCreditos);
    }

    
    
        /// <summary>
        /// Muestra el panel de configuración, reproduce un sonido y pausa el juego.
        /// </summary>
        /// <example>
        /// Este método se llama automáticamente al hacer clic en el botón <c>botonAbrir</c>.
        /// </example>
        /// <exception cref="System.NullReferenceException">
        /// Se lanza si el <c>AudioManager</c> o el <c>panelConfiguracion</c> no están asignados.
        /// </exception>
    void AbrirConfiguracion()
    {
        AudioManager.instancia.ReproducirSonido(sonidoBoton);

        panelConfiguracion.SetActive(true);

        Time.timeScale = 0f;
    }

    
        /// <summary>
        /// Oculta el panel de configuración, reproduce un sonido y reanuda el juego.
        /// </summary>
        /// <example>
        /// Este método se llama al presionar el botón <c>botonCerrar</c>.
        /// </example>
    void CerrarConfiguracion()
    {
        AudioManager.instancia.ReproducirSonido(sonidoBoton);

        panelConfiguracion.SetActive(false);

        Time.timeScale = 1f;
    }


        /// <summary>
        /// Muestra el panel de créditos y reproduce un sonido.
        /// </summary>
        /// <remarks>
        /// El tiempo del juego no se pausa al mostrar créditos.
        /// </remarks>
    void AbrirCreditos()
    {
        AudioManager.instancia.ReproducirSonido(sonidoBoton);

        panelCreditos.SetActive(true);
    }

    
        /// <summary>
        /// Oculta el panel de créditos y reproduce un sonido.
        /// </summary>
        /// <remarks>
        /// Se recomienda ocultar el panel con animación si deseas una mejor experiencia de usuario.
        /// </remarks>
    void CerrarCreditos()
    {
        AudioManager.instancia.ReproducirSonido(sonidoBoton);

        panelCreditos.SetActive(false);
    }
}
