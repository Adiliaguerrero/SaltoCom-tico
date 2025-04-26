using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float velocidad = 5f;
    public float fuerzaSalto = 12f;
    public Animator animator;
    public Rigidbody2D rb;

    private bool enSuelo = true;
    private Transform detectorSuelo;
    private LayerMask capaSuelo;

    [SerializeField] private float vidaMaxima = 5f;
    public float vida = 5f;
    public Joystick joystick;

    private bool recibiendoDanio = false;
    public float FuerzaRebote = 5f;
    public BarraVida barraVida;

    void Start()
    {
        // Buscar automáticamente detector de suelo si no está asignado
        detectorSuelo = transform.Find("DetectorSuelo");
        if (detectorSuelo == null)
        {
            Debug.LogError("No se encontró DetectorSuelo en el personaje instanciado.");
        }

        // Asignar capa de suelo dinámicamente
        capaSuelo = LayerMask.GetMask("Suelo");

        // Buscar la barra de vida si no está asignada
        if (barraVida == null)
        {
            barraVida = FindObjectOfType<BarraVida>();
        }

        // Asignar el Animator automáticamente si no se asignó en el Inspector
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
    }

    void Update()
    {
        if (recibiendoDanio) return;

        // Comprobar si está en el suelo
        if (detectorSuelo != null)
        {
            enSuelo = Physics2D.OverlapCircle(detectorSuelo.position, 0.1f, capaSuelo);
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
            animator.SetBool("Jump", !enSuelo); // Mantiene la animación de salto activa solo cuando no está en suelo
        }

        if (velocidadX > 0)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (velocidadX < 0)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }

        // Detectar salto
        if (Input.GetButtonDown("Jump") && enSuelo)
        {
            rb.velocity = new Vector2(rb.velocity.x, fuerzaSalto);
            animator.SetTrigger("JumpTrigger"); // Se activa la animación de salto
        }
    }

   public void RecibeDanio(Vector2 direccion, int cantDanio)
{
    if (recibiendoDanio) return;

    recibiendoDanio = true;
    vida -= cantDanio;
    vida = Mathf.Clamp(vida, 0f, vidaMaxima); // Asegura que la vida no sea menor a 0

    PlayerPrefs.SetFloat("VidaJugador", vida);
    PlayerPrefs.Save();

    if (barraVida != null)
    {
        barraVida.ActualizarBarra(vida);
        
        // ⚠️ Si la vida llegó a 0, activar Game Over
        if (vida <= 0)
        {
            barraVida.ActivarGameOver();
        }
    }
    else
    {
        Debug.LogError("No se encontró la barra de vida en PlayerController.");
    }

    // Rebote del personaje al recibir daño
    Vector2 rebote = new Vector2(transform.position.x - direccion.x, 1).normalized;
    rb.AddForce(rebote * FuerzaRebote * cantDanio, ForceMode2D.Impulse);

    Invoke(nameof(DesactivaDanio), 0.5f);
}


    private void DesactivaDanio()
    {
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
}
