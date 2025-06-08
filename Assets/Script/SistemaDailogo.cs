using UnityEngine;
using TMPro;                 
using UnityEngine.UI;       
using System.Collections;


    /// <summary>
    /// Controla el sistema de diálogo de un NPC, incluyendo la escritura progresiva del texto,
    /// el bloqueo del movimiento del jugador durante el diálogo y la reaparición del NPC.
    /// </summary>
public class SistemaDialogo : MonoBehaviour
{
        /// <summary>
        /// Panel de diálogo que contiene el texto y el botón de avance.
        /// </summary>
    public GameObject panelDialogo;

        /// <summary>
        /// Líneas del diálogo inicial mostradas la primera vez que se interactúa con el NPC.
        /// </summary>
    public string[] lineasIniciales;

        /// <summary>
        /// Líneas del diálogo mostradas cuando el NPC reaparece.
        /// </summary>
    public string[] lineasReaparicion;

        /// <summary>
        /// Velocidad con la que se escribe cada letra del texto (en segundos).
        /// </summary>
    public float velocidadEscritura = 0.05f;

    private Coroutine escrituraActual;

        /// <summary>
        /// Objeto de texto (TextMeshProUGUI) donde se mostrará el diálogo.
        /// </summary>
    public TextMeshProUGUI textoDialogo;

    private int indice = 0;

        /// <summary>
        /// Prefab del NPC que se instanciará cuando reaparezca con otro diálogo.
        /// </summary>
    public GameObject npcPrefab;

        /// <summary>
        /// Punto donde aparecerá el NPC reaparecido.
        /// </summary>
    public Transform puntoReaparicion;

    private bool dialogoIniciado = false;

    
        /// <summary>
        /// Indica si el diálogo actual es parte de la reaparición del NPC.
        /// </summary>
    public bool esReaparicion = false;

    private Rigidbody2D rbJugadorOriginal;

    private PlayerController playerController;

        /// <summary>
        /// UI del joystick que se desactiva durante el diálogo y se reactiva al finalizar.
        /// </summary>
    public GameObject joystickUI;

        /// <summary>
        /// Sonido que se reproduce al avanzar en el diálogo.
        /// </summary>
    public AudioClip sonidoPanel;

    void Start()
    {
        // Oculta el panel de diálogo para que no se vea al inicio
        panelDialogo.SetActive(false);

        // Obtiene el botón del panel y le añade un listener para que al hacer clic avance la línea de diálogo
        Button boton = panelDialogo.GetComponent<Button>();
        if (boton != null)
        {
            boton.onClick.AddListener(MostrarLinea);
        }
    }

        /// <summary>
        /// Activa el sistema de diálogo, bloqueando al jugador y mostrando las líneas de texto.
        /// </summary>
    public void ActivarDialogo()
    {
        // Si el diálogo ya se inició, no se vuelve a activar
        if (dialogoIniciado) return;

        // Marca que el diálogo está en curso
        dialogoIniciado = true;

        // Muestra el panel de diálogo en pantalla
        panelDialogo.SetActive(true);

        // Reinicia el índice para mostrar desde la primera línea
        indice = 0;

        // Busca el jugador en la escena para detener su movimiento durante el diálogo
        GameObject jugador = GameObject.FindGameObjectWithTag("Player");
        if (jugador != null)
        {
            // Obtiene el Rigidbody2D del jugador y detiene su física
            rbJugadorOriginal = jugador.GetComponent<Rigidbody2D>();
            if (rbJugadorOriginal != null)
            {
                rbJugadorOriginal.velocity = Vector2.zero;
                rbJugadorOriginal.bodyType = RigidbodyType2D.Static;
            }

            // Obtiene el script que controla el movimiento del jugador y lo desactiva
            playerController = jugador.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.puedeMoverse = false;
            }
        }

        // Desactiva el joystick para evitar que el jugador se mueva con controles táctiles mientras se muestra el diálogo
        if (joystickUI != null)
        {
            FixedJoystick joystick = joystickUI.GetComponent<FixedJoystick>();
            if (joystick != null)
            {
                joystick.OnPointerUp(null);
                joystick.enabled = false;
            }
            joystickUI.SetActive(false);
        }

        // Comienza a mostrar la primera línea del diálogo
        MostrarLinea();
    }

        /// <summary>
        /// Muestra la siguiente línea de diálogo con animación de escritura y gestiona el fin del diálogo.
        /// </summary>
    public void MostrarLinea()
    {
        // Reproduce el sonido asociado al avance de diálogo si está configurado y el AudioManager existe
        if (sonidoPanel != null && AudioManager.instancia != null)
        {
            AudioManager.instancia.ReproducirSonido(sonidoPanel);
        }

        // Elige qué diálogo mostrar según si es la reaparición o el diálogo inicial
        string[] dialogoActual = esReaparicion ? lineasReaparicion : lineasIniciales;

        // Si todavía quedan líneas por mostrar
        if (indice < dialogoActual.Length)
        {
            // Si hay una corrutina escribiendo el texto, la detiene para mostrar la siguiente línea
            if (escrituraActual != null)
            {
                StopCoroutine(escrituraActual);
            }

            // Inicia la corrutina para escribir la línea letra a letra
            escrituraActual = StartCoroutine(EscribirLinea(dialogoActual[indice]));

            // Incrementa el índice para la siguiente vez
            indice++;
        }
        else
        {
            // Si ya no hay más líneas, cierra el panel de diálogo
            panelDialogo.SetActive(false);

            // Si es el diálogo inicial (no reaparición)
            if (!esReaparicion)
            {
                // Reactiva la física y movimiento del jugador
                if (rbJugadorOriginal != null)
                    rbJugadorOriginal.bodyType = RigidbodyType2D.Dynamic;

                if (playerController != null)
                    playerController.puedeMoverse = true;

                // Reactiva el joystick
                if (joystickUI != null)
                {
                    joystickUI.SetActive(true);
                    FixedJoystick joystick = joystickUI.GetComponent<FixedJoystick>();
                    if (joystick != null)
                        joystick.enabled = true;
                }
            }

            // Si no es diálogo de reaparición, instancia el NPC reaparecido con diálogo diferente
            if (!esReaparicion && npcPrefab != null && puntoReaparicion != null)
            {
                GameObject nuevoNPC = Instantiate(npcPrefab, puntoReaparicion.position, Quaternion.identity);
                SistemaDialogo nuevoDialogo = nuevoNPC.GetComponent<SistemaDialogo>();
                if (nuevoDialogo != null)
                {
                    // Marca que es diálogo de reaparición para que muestre las líneas correspondientes
                    nuevoDialogo.esReaparicion = true;

                    // Le pasa la referencia del joystick para manejarlo correctamente
                    nuevoDialogo.joystickUI = joystickUI;
                }
            }

            // Si es diálogo de reaparición, avanza la trivia o lógica siguiente
            if (esReaparicion)
            {
                Paneles1 trivias = FindObjectOfType<Paneles1>();
                if (trivias != null)
                {
                    trivias.SiguienteTrivia();
                }
            }

            // Destruye este objeto NPC para que no quede duplicado en la escena
            Destroy(gameObject);
        }
    }

    // Método que detecta la colisión con el jugador para activar el diálogo
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Si el objeto que colisiona es el jugador
        if (collision.gameObject.CompareTag("Player"))
        {
            // Activa el diálogo
            ActivarDialogo();
        }
    }

    // Corrutina que escribe el texto letra por letra para simular escritura progresiva
    private IEnumerator EscribirLinea(string linea)
    {
        // Limpia el texto previo
        textoDialogo.text = "";

        // Itera cada carácter del string recibido
        foreach (char letra in linea.ToCharArray())
        {
            // Agrega la letra al texto visible
            textoDialogo.text += letra;

            // Espera el tiempo definido para la velocidad de escritura antes de agregar la siguiente letra
            yield return new WaitForSeconds(velocidadEscritura);
        }
    }
}
