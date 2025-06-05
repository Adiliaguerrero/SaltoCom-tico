using UnityEngine;
using UnityEngine.EventSystems;

   // Esta Clase responsable de gestionar el comportamiento integral del personaje, abarcando desplazamiento horizontal,
// mecánicas de salto, control de animaciones, sistema de salud y la perdida de  vida por daño.
// Incluye compatibilidad con entradas de teclado, joystick y pantallas táctiles.


public class PlayerController : MonoBehaviour
{
    // Velocidad horizontal del personaje
    public float velocidad = 5f;

    // Fuerza aplicada para el salto del personaje
    public float fuerzaSalto = 12f;

    // Referencia al Animator para controlar las animaciones
    public Animator animator;

    // Referencia al Rigidbody2D para la física del personaje
    public Rigidbody2D rb;

    // Controla si el personaje puede moverse o no
    public bool puedeMoverse = true;

    // Variable interna que indica si el personaje está tocando el suelo
    private bool enSuelo = true;

    // Transform que sirve para detectar si el personaje está en el suelo
    private Transform detectorSuelo;

    // Vida máxima que puede tener el personaje
    [SerializeField] private float vidaMaxima = 5f;

    // Vida actual del personaje
    public float vida = 5f;

    // Referencia al joystick para controles táctiles
    public Joystick joystick;

    // Controla si el personaje está recibiendo daño para evitar múltiples impactos simultáneos
    private bool recibiendoDanio = false;

    // Fuerza del rebote que se aplica al personaje cuando recibe daño
    public float FuerzaRebote = 5f;

    // Referencia a la barra de vida para actualizar la interfaz
    public BarraVida barraVida;

    // Referencia al script de sacudida de cámara para efectos visuales al recibir daño
    private CameraShake cameraShake;

    // Método Start: se ejecuta al iniciar la escena o activar el objeto
    void Start()
    {
        // Busca el objeto hijo llamado "DetectorSuelo" para detectar colisiones con el suelo
        detectorSuelo = transform.Find("DetectorSuelo");
        if (detectorSuelo == null)
        {
            Debug.LogError("No se encontró DetectorSuelo en el personaje instanciado.");
        }

        // Si no se asignó la barra de vida desde el Inspector, intenta buscarla en la escena
        if (barraVida == null)
        {
            barraVida = FindObjectOfType<BarraVida>();
        }

        // Si no se asignó el Animator, lo obtiene automáticamente del GameObject
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        // Inicializa la vida actual con el valor máximo
        vida = vidaMaxima;

        // Guarda la vida actual del jugador en PlayerPrefs para mantenerla persistente
        PlayerPrefs.SetFloat("VidaJugador", vida);
        PlayerPrefs.Save();

        // Actualiza la barra de vida en la interfaz
        if (barraVida != null)
        {
            barraVida.ActualizarBarra(vida);
        }

        // Obtiene la referencia al script de sacudida de cámara desde la cámara principal
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

    // variable para Salto solicitado  
    bool saltoSolicitado = Input.GetButtonDown("Jump");

    // ✅ Detecta todos los toques en pantalla (móvil)
    for (int i = 0; i < Input.touchCount; i++)
    {
        Touch toque = Input.GetTouch(i);

        // Ignora si está tocando  objetos de la UI
        if (EventSystem.current.IsPointerOverGameObject(toque.fingerId))
            continue;

        //  Solo si el toque está en el lado derecho de la pantalla salta
        if (toque.phase == TouchPhase.Began && toque.position.x > Screen.width / 2)
        {
            saltoSolicitado = true;
            break;
        }
    }

    // Permite salto con clic de mouse (útil para pruebas en PC)
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


    public void RecibeDanio(Vector2 direccion, int cantDanio)
    {
        // Si ya está recibiendo daño, no hacer nada
        if (recibiendoDanio) return;

        // Se marca como que está recibiendo daño
        recibiendoDanio = true;

        // Se resta vida y se limita entre 0 y el máximo
        vida -= cantDanio;
        vida = Mathf.Clamp(vida, 0f, vidaMaxima);

        // Guarda la vida actual en PlayerPrefs
        PlayerPrefs.SetFloat("VidaJugador", vida);
        PlayerPrefs.Save();

        // Si hay barra de vida, se actualiza y verifica si hay game over
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

        // Aplica rebote al personaje en dirección opuesta al daño recibido
        Vector2 rebote = new Vector2(transform.position.x - direccion.x, 1).normalized;
        rb.AddForce(rebote * FuerzaRebote * cantDanio, ForceMode2D.Impulse);

        // Si hay efecto de cámara, se activa sacudida
        if (cameraShake != null)
        {
            cameraShake.Sacudir();
        }

        // Desactiva el estado de daño después de un breve tiempo
        Invoke(nameof(DesactivaDanio), 0.5f);
    }

    private void DesactivaDanio()
    {
        // Permite que el personaje vuelva a recibir daño
        recibiendoDanio = false;
    }

    private void OnValidate()
    {
        // Asegura que la vida no exceda los límites y actualiza la barra en el editor
        if (barraVida != null)
        {
            vida = Mathf.Clamp(vida, 0f, vidaMaxima);
            barraVida.ActualizarBarra(vida);
        }
    }

    public void RestaurarVida()
    {
        // Restaura la vida al valor máximo
        vida = vidaMaxima;

        // Guarda la nueva vida en PlayerPrefs
        PlayerPrefs.SetFloat("VidaJugador", vida);
        PlayerPrefs.Save();

        // Actualiza la barra de vida si está presente
        if (barraVida != null)
        {
            barraVida.ActualizarBarra(vida);
        }
    }


    public void RestaurarVidaParcial(float cantidad)
    {
        // Aumenta la vida con la cantidad indicada
        vida += cantidad;

        // Asegura que la vida no exceda el máximo ni sea menor que 0
        vida = Mathf.Clamp(vida, 0f, vidaMaxima);

        // Guarda la vida actual en PlayerPrefs
        PlayerPrefs.SetFloat("VidaJugador", vida);
        PlayerPrefs.Save();

        // Si hay una barra de vida, se actualiza
        if (barraVida != null)
        {
            barraVida.ActualizarBarra(vida);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        // Dibuja un gizmo rojo en la posición del detector de suelo para visualizarlo en el editor
        if (detectorSuelo != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(detectorSuelo.position, 0.1f);
        }
    }
#endif
}
