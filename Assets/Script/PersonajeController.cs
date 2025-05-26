using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float velocidad = 5f;
    public float fuerzaSalto = 12f;
    public Animator animator;
    public Rigidbody2D rb;
    public bool puedeMoverse = true;


    private bool enSuelo = true;
    private Transform detectorSuelo;

    [SerializeField] private float vidaMaxima = 5f;
    public float vida = 5f;
    public Joystick joystick;

    private bool recibiendoDanio = false;
    public float FuerzaRebote = 5f;
    public BarraVida barraVida;

    private CameraShake cameraShake;

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
    {
        transform.localScale = new Vector3(1f, 1f, 1f);
    }
    else if (velocidadX < 0)
    {
        transform.localScale = new Vector3(-1f, 1f, 1f);
    }

     
    bool saltoSolicitado = Input.GetButtonDown("Jump");

     
    if (Input.touchCount > 0)
    {
        Touch toque = Input.GetTouch(0);
        if (toque.phase == TouchPhase.Began && toque.position.x > Screen.width / 2)
        {
            saltoSolicitado = true;
        }
    }

     
    if (Input.GetMouseButtonDown(0))
    {
        if (Input.mousePosition.x > Screen.width / 2)
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


    public void RestaurarVidaParcial(float cantidad)
{
    vida += cantidad;
    vida = Mathf.Clamp(vida, 0f, vidaMaxima); // Evita que supere el máximo

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
