
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
        // Ocultamos el panel de configuración para que no se vea al comenzar el juego
        panelConfiguracion.SetActive(false);

        // Ocultamos también el panel de créditos para que no aparezca visible al iniciar
        panelCreditos.SetActive(false);

        // Agregamos a botonAbrir la función AbrirConfiguracion para que se llame al hacer clic en ese botón
        botonAbrir.onClick.AddListener(AbrirConfiguracion);

        // Agregamos a botonCerrar la función CerrarConfiguracion para cerrar el panel al hacer clic
        botonCerrar.onClick.AddListener(CerrarConfiguracion);

        // Agregamos a botonCreditosAbrir la función AbrirCreditos para mostrar créditos al pulsar el botón
        botonCreditosAbrir.onClick.AddListener(AbrirCreditos);

        // Agregamos a botonCreditosCerrar la función CerrarCreditos para ocultar los créditos al pulsar
        botonCreditosCerrar.onClick.AddListener(CerrarCreditos);
    }

    // Función que se llama para mostrar el panel de configuración
    void AbrirConfiguracion()
    {
        // Reproducimos el sonido de botón para indicar que se pulsó correctamente
        AudioManager.instancia.ReproducirSonido(sonidoBoton);

        // Activamos (mostramos) el panel de configuración
        panelConfiguracion.SetActive(true);

        // Pausamos el juego para que el jugador pueda modificar la configuración sin que avance el tiempo
        Time.timeScale = 0f;
    }

    // Función que se llama para ocultar el panel de configuración
    void CerrarConfiguracion()
    {
        // Reproducimos el sonido de botón para retroalimentar la acción de cerrar
        AudioManager.instancia.ReproducirSonido(sonidoBoton);

        // Desactivamos (ocultamos) el panel de configuración
        panelConfiguracion.SetActive(false);

        // Reanudamos el tiempo normal del juego para que todo continúe normalmente
        Time.timeScale = 1f;
    }

    // Función que se llama para mostrar el panel de créditos
    void AbrirCreditos()
    {
        // Reproducimos el sonido de botón
        AudioManager.instancia.ReproducirSonido(sonidoBoton);

        // Mostramos el panel de créditos
        panelCreditos.SetActive(true);
    }

    // Función que se llama para ocultar el panel de créditos
    void CerrarCreditos()
    {
        // Reproducimos el sonido de botón
        AudioManager.instancia.ReproducirSonido(sonidoBoton);

        // Ocultamos el panel de los créditos
        panelCreditos.SetActive(false);
    }
}
