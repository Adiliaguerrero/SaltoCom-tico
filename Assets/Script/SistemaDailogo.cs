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
    // Panel que contiene la UI del diálogo (texto y botón)
    public GameObject panelDialogo;

    // Líneas del diálogo inicial que se mostrarán la primera vez que se interactúa con el NPC
    public string[] lineasIniciales;

    // Líneas del diálogo que se muestran cuando el NPC reaparece tras la primera interacción
    public string[] lineasReaparicion;

    // Velocidad a la que se escribe cada letra en el texto (en segundos)
    public float velocidadEscritura = 0.05f;

    // Referencia a la corrutina actual que escribe el texto para poder detenerla si es necesario
    private Coroutine escrituraActual;

    // Componente TextMeshPro donde se mostrará el texto del diálogo
    public TextMeshProUGUI textoDialogo;

    // Índice que lleva el control de qué línea de diálogo se está mostrando
    private int indice = 0;

    // Prefab del NPC que se instancia para que reaparezca tras el primer diálogo
    public GameObject npcPrefab;

    // Punto en el mundo donde aparecerá el NPC reaparecido
    public Transform puntoReaparicion;

    // Variable para evitar que el diálogo se active múltiples veces simultáneamente
    private bool dialogoIniciado = false;

    // Variable que indica si este diálogo corresponde a la reaparición del NPC
    public bool esReaparicion = false;

    // Rigidbody2D del jugador para bloquear y desbloquear su movimiento durante el diálogo
    private Rigidbody2D rbJugadorOriginal;

    // Referencia al script del jugador para permitir o impedir que se mueva
    private PlayerController playerController;

    // Objeto UI del joystick para controlar el movimiento táctil, que se desactivará durante el diálogo
    public GameObject joystickUI;

    // Clip de audio que se reproduce cada vez que se avanza una línea del diálogo
    public AudioClip sonidoPanel;

    // Método que se llama al iniciar el juego o la escena
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

    // Método público que activa el diálogo cuando el jugador interactúa con el NPC
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

    // Método que muestra la siguiente línea de diálogo
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
