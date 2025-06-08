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
        panelDialogo.SetActive(false);

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
        if (dialogoIniciado) return;

        dialogoIniciado = true;

        panelDialogo.SetActive(true);

        indice = 0;

        GameObject jugador = GameObject.FindGameObjectWithTag("Player");
        if (jugador != null)
        {
            rbJugadorOriginal = jugador.GetComponent<Rigidbody2D>();
            if (rbJugadorOriginal != null)
            {
                rbJugadorOriginal.velocity = Vector2.zero;
                rbJugadorOriginal.bodyType = RigidbodyType2D.Static;
            }

            playerController = jugador.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.puedeMoverse = false;
            }
        }

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

        MostrarLinea();
    }

        /// <summary>
        /// Muestra la siguiente línea de diálogo con animación de escritura y gestiona el fin del diálogo.
        /// </summary>
    public void MostrarLinea()
    {
        if (sonidoPanel != null && AudioManager.instancia != null)
        {
            AudioManager.instancia.ReproducirSonido(sonidoPanel);
        }

        string[] dialogoActual = esReaparicion ? lineasReaparicion : lineasIniciales;

        if (indice < dialogoActual.Length)
        {
            if (escrituraActual != null)
            {
                StopCoroutine(escrituraActual);
            }

            escrituraActual = StartCoroutine(EscribirLinea(dialogoActual[indice]));

            indice++;
        }
        else
        {
            panelDialogo.SetActive(false);

            if (!esReaparicion)
            {
                if (rbJugadorOriginal != null)
                    rbJugadorOriginal.bodyType = RigidbodyType2D.Dynamic;

                if (playerController != null)
                    playerController.puedeMoverse = true;

                if (joystickUI != null)
                {
                    joystickUI.SetActive(true);
                    FixedJoystick joystick = joystickUI.GetComponent<FixedJoystick>();
                    if (joystick != null)
                        joystick.enabled = true;
                }
            }

            if (!esReaparicion && npcPrefab != null && puntoReaparicion != null)
            {
                GameObject nuevoNPC = Instantiate(npcPrefab, puntoReaparicion.position, Quaternion.identity);
                SistemaDialogo nuevoDialogo = nuevoNPC.GetComponent<SistemaDialogo>();
                if (nuevoDialogo != null)
                {
                    nuevoDialogo.esReaparicion = true;

                    nuevoDialogo.joystickUI = joystickUI;
                }
            }

            if (esReaparicion)
            {
                Paneles1 trivias = FindObjectOfType<Paneles1>();
                if (trivias != null)
                {
                    trivias.SiguienteTrivia();
                }
            }

            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            ActivarDialogo();
        }
    }

    private IEnumerator EscribirLinea(string linea)
    {
        textoDialogo.text = "";

        foreach (char letra in linea.ToCharArray())
        {
            textoDialogo.text += letra;

            yield return new WaitForSeconds(velocidadEscritura);
        }
    }
}
