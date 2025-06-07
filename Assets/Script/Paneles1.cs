using UnityEngine;

    /// <summary>
    /// Controla la activación secuencial de paneles tipo trivia y gestiona el estado del jugador durante su visualización.
    /// </summary>
    /// <remarks>
    /// Los paneles de trivia se activan uno por uno al presionar un botón.
    /// Durante la visualización, se desactivan elementos de la interfaz y se congela al jugador.
    /// </remarks>
public class Paneles1 : MonoBehaviour
{
    // Paneles que se activan uno por uno
        /// <summary>
        /// Primer panel de trivia.
        /// </summary>
    public GameObject panel1;

        /// <summary>
        /// Segundo panel de trivia.
        /// </summary>
    public GameObject panel2;

        /// <summary>
        /// Tercer panel de trivia.
        /// </summary>
    public GameObject panel3;

    // Elementos de la interfaz que se desactivan mientras los paneles están activos
        /// <summary>
        /// Joystick virtual que se desactiva durante la trivia.
        /// </summary>
    public GameObject joystick;

    
        /// <summary>
        /// Primer elemento visual auxiliar de la interfaz.
        /// </summary>
    public GameObject imagen1;


        /// <summary>
        /// Segundo elemento visual auxiliar de la interfaz.
        /// </summary>
    public GameObject imagen2;

    // Índice que lleva el seguimiento de la trivia actual
        /// <summary>
        /// Índice de la trivia actualmente mostrada.
        /// </summary>
    private int triviaActual = 0;

    private Rigidbody2D rbJugador;

    private PlayerController playerController;

        /// <summary>
        /// Inicializa referencias, desactiva paneles y activa elementos de interfaz al iniciar la escena.
        /// </summary>
    void Start()
    {
        // Desactiva todos los paneles al iniciar
        panel1.SetActive(false);
        panel2.SetActive(false);
        panel3.SetActive(false);

        // Activa joystick e imágenes al inicio
        joystick.SetActive(true);
        imagen1.SetActive(true);
        imagen2.SetActive(true);

        // Busca al jugador en la escena por su tag y obtiene sus componentes
        GameObject jugador = GameObject.FindGameObjectWithTag("Player");
        if (jugador != null)
        {
            rbJugador = jugador.GetComponent<Rigidbody2D>();
            playerController = jugador.GetComponent<PlayerController>();
        }
    }

        /// <summary>
        /// Método público que se llama desde un botón para avanzar a la siguiente trivia.
        /// </summary>
    public void SiguienteTrivia()
    {
        // Vuelve a obtener referencias del jugador por si no estaban asignadas aún
        GameObject jugador = GameObject.FindGameObjectWithTag("Player");
        if (jugador != null)
        {
            rbJugador = jugador.GetComponent<Rigidbody2D>();
            playerController = jugador.GetComponent<PlayerController>();
        }

        // Activa el panel correspondiente según el índice actual
        if (triviaActual == 0)
            ActivarPanel(0);
        else if (triviaActual == 1)
            ActivarPanel(1);
        else if (triviaActual == 2)
            ActivarPanel(2);
        else
        {
            // Si ya no hay más trivias, desactiva todos los paneles y reactiva el control del jugador
            Debug.Log("Todas las trivias han terminado.");

            panel1.SetActive(false);
            panel2.SetActive(false);
            panel3.SetActive(false);

            // Activa el joystick
            joystick.SetActive(true);

            // Vuelve a habilitar el componente de joystick si existe
            FixedJoystick joyComponent = joystick.GetComponent<FixedJoystick>();
            if (joyComponent != null)
            {
                joyComponent.enabled = true;
            }

            // Vuelve a activar las imágenes
            imagen1.SetActive(true);
            imagen2.SetActive(true);

            // Reactiva el movimiento del jugador
            if (rbJugador != null)
                rbJugador.bodyType = RigidbodyType2D.Dynamic;

            if (playerController != null)
                playerController.puedeMoverse = true;

            return;
        }

        triviaActual++;
    }

    private void ActivarPanel(int index)
    {
        // Activa solo el panel correspondiente al índice, desactivando los otros
        panel1.SetActive(index == 0);
        panel2.SetActive(index == 1);
        panel3.SetActive(index == 2);

        bool panelActivo = panel1.activeSelf || panel2.activeSelf || panel3.activeSelf;

        joystick.SetActive(!panelActivo);
        imagen1.SetActive(!panelActivo);
        imagen2.SetActive(!panelActivo);

        if (rbJugador != null)
            rbJugador.bodyType = panelActivo ? RigidbodyType2D.Static : RigidbodyType2D.Dynamic;

        if (playerController != null)
            playerController.puedeMoverse = !panelActivo;
    }
}
