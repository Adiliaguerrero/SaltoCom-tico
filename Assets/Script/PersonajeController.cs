using UnityEngine;
using UnityEngine.EventSystems;


    /// <summary>
    /// Gestiona el comportamiento integral del personaje.
    /// Incluye movimiento, salto, animaciones, sistema de vida, daño, entrada táctil y compatibilidad con joystick.
    /// </summary>
public class PlayerController : MonoBehaviour
{

        /// <summary>
        /// Velocidad horizontal del personaje.
        /// </summary>
    public float velocidad = 5f;

        /// <summary>
        /// Fuerza aplicada para el salto del personaje.
        /// </summary>
    public float fuerzaSalto = 12f;

        /// <summary>
        /// Referencia al Animator para controlar animaciones.
        /// </summary>
    public Animator animator;

        /// <summary>
        /// Referencia al Rigidbody2D para aplicar físicas.
        /// </summary>
    public Rigidbody2D rb;

        /// <summary>
        /// Determina si el personaje puede moverse.
        /// </summary>
    public bool puedeMoverse = true;

    private bool enSuelo = true;

    private Transform detectorSuelo;

    [SerializeField] private float vidaMaxima = 5f;


        /// <summary>
        /// Vida actual del personaje.
        /// </summary>
    public float vida = 5f;


         /// <summary>
         /// Referencia al joystick para controles móviles.
         /// </summary>
    public Joystick joystick;

    private bool recibiendoDanio = false;

    /// <summary>
    /// Fuerza del rebote al recibir daño.
    /// </summary>
    public float FuerzaRebote = 5f;

    /// <summary>
    /// Referencia al componente que controla la barra de vida.
    /// </summary>
    public BarraVida barraVida;

    private CameraShake cameraShake;

        /// <summary>
        /// Inicializa variables y referencias necesarias al comenzar la escena.
        /// </summary>
    void Start()
    {
        detectorSuelo = transform.Find("DetectorSuelo");
        if (detectorSuelo == null)
        {
            Debug.LogError("No se encontró DetectorSuelo en el personaje instanciado.");
        }

        if (barraVida == null)
        {
            barraVida = FindObjectOfType<BarraVida>();
        }

        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        vida = vidaMaxima;

        PlayerPrefs.SetFloat("VidaJugador", vida);
        PlayerPrefs.Save();

        if (barraVida != null)
        {
            barraVida.ActualizarBarra(vida);
        }

        cameraShake = Camera.main.GetComponent<CameraShake>();
    }

   void Update()
{
    if (recibiendoDanio) return;

    if (!puedeMoverse)
    {
        rb.velocity = new Vector2(0, rb.velocity.y);
        animator.SetBool("Movement", false);
        return;
    }

    if (detectorSuelo != null)
    {
        Collider2D[] colisiones = Physics2D.OverlapCircleAll(detectorSuelo.position, 0.1f);
        enSuelo = false;

        foreach (var col in colisiones)
        {
            if (col.CompareTag("Suelo"))
            {
                enSuelo = true;
                break;
            }
        }
    }

    float velocidadX = Input.GetAxisRaw("Horizontal");

    if (joystick != null && joystick.Horizontal != 0)
    {
        velocidadX = joystick.Horizontal;
    }

    rb.velocity = new Vector2(velocidadX * velocidad, rb.velocity.y);

    if (animator != null)
    {
        animator.SetBool("Movement", velocidadX != 0);
        animator.SetBool("Jump", !enSuelo);
    }

    if (velocidadX > 0)
        transform.localScale = new Vector3(1f, 1f, 1f);
    else if (velocidadX < 0)
        transform.localScale = new Vector3(-1f, 1f, 1f);

    bool saltoSolicitado = Input.GetButtonDown("Jump");

    for (int i = 0; i < Input.touchCount; i++)
    {
        Touch toque = Input.GetTouch(i);

        if (EventSystem.current.IsPointerOverGameObject(toque.fingerId))
            continue;

        if (toque.phase == TouchPhase.Began && toque.position.x > Screen.width / 2)
        {
            saltoSolicitado = true;
            break;
        }
    }

    if (Input.GetMouseButtonDown(0))
    {
        if (!EventSystem.current.IsPointerOverGameObject() && Input.mousePosition.x > Screen.width / 2)
        {
            saltoSolicitado = true;
        }
    }

    if (saltoSolicitado && enSuelo)
    {
        rb.velocity = new Vector2(rb.velocity.x, fuerzaSalto);
        animator.SetTrigger("JumpTrigger");
    }
}

        /// <summary>
        /// Gestiona la recepción de daño por parte del personaje.
        /// </summary>
        /// <param name="direccion">Dirección del daño recibido.</param>
        /// <param name="cantDanio">Cantidad de daño recibido.</param>
    public void RecibeDanio(Vector2 direccion, int cantDanio)
    {
        if (recibiendoDanio) return;

        recibiendoDanio = true;

        vida -= cantDanio;
        vida = Mathf.Clamp(vida, 0f, vidaMaxima);

        PlayerPrefs.SetFloat("VidaJugador", vida);
        PlayerPrefs.Save();

        if (barraVida != null)
        {
            barraVida.ActualizarBarra(vida);
            if (vida <= 0)
            {
                barraVida.ActivarGameOver();
            }
        }
        else
        {
            // Muestra error si no hay barra de vida asignada
            Debug.LogError("No se encontró la barra de vida en PlayerController.");
        }

        Vector2 rebote = new Vector2(transform.position.x - direccion.x, 1).normalized;
        rb.AddForce(rebote * FuerzaRebote * cantDanio, ForceMode2D.Impulse);

        if (cameraShake != null)
        {
            cameraShake.Sacudir();
        }

        Invoke(nameof(DesactivaDanio), 0.5f);
    }

    private void DesactivaDanio()
    {
        // Permite que el personaje vuelva a recibir daño
        recibiendoDanio = false;
    }

    private void OnValidate()
    {
        if (barraVida != null)
        {
            vida = Mathf.Clamp(vida, 0f, vidaMaxima);
            barraVida.ActualizarBarra(vida);
        }
    }

    /// <summary>
    /// Restaura la vida del personaje a su valor máximo.
    /// </summary>
    public void RestaurarVida()
    {
        vida = vidaMaxima;

        PlayerPrefs.SetFloat("VidaJugador", vida);
        PlayerPrefs.Save();

        if (barraVida != null)
        {
            barraVida.ActualizarBarra(vida);
        }
    }

    /// <summary>Restaura una cantidad parcial de vida al personaje.</summary>
    /// <param name="cantidad">Cantidad de vida a restaurar.</param>
    public void RestaurarVidaParcial(float cantidad)
    {
        vida += cantidad;

        vida = Mathf.Clamp(vida, 0f, vidaMaxima);

        PlayerPrefs.SetFloat("VidaJugador", vida);
        PlayerPrefs.Save();

        if (barraVida != null)
        {
            barraVida.ActualizarBarra(vida);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (detectorSuelo != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(detectorSuelo.position, 0.1f);
        }
    }
#endif
}
