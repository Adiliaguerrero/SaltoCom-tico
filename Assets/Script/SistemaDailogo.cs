using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SistemaDialogo : MonoBehaviour
{
    public GameObject panelDialogo;
    public string[] lineasIniciales;
    public string[] lineasReaparicion;

    public TextMeshProUGUI textoDialogo;
    private int indice = 0;

    public GameObject npcPrefab;
    public Transform puntoReaparicion;

    private bool dialogoIniciado = false;
    public bool esReaparicion = false;

    private Rigidbody2D rbJugadorOriginal;
    private PlayerController playerController;

    [Header("Joystick")]
    public GameObject joystickUI;

    [Header("Audio")]
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

   public void MostrarLinea()
{
    
    if (sonidoPanel != null && AudioManager.instancia != null)
    {
        AudioManager.instancia.ReproducirSonido(sonidoPanel);
    }

    string[] dialogoActual = esReaparicion ? lineasReaparicion : lineasIniciales;

    if (indice < dialogoActual.Length)
    {
        textoDialogo.text = dialogoActual[indice];
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

    



    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            ActivarDialogo();

            Vector3 escala = transform.localScale;
            escala.x = Mathf.Abs(escala.x) * -1f;
            transform.localScale = escala;
        }
    }
}
