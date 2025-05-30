using UnityEngine;
using System.Collections;


public class Ninja : MonoBehaviour
{
    public float detectionRadius = 5.0f;
    public float attackRadius = 1.0f;
    public float speed = 2.0f;
    public float attackCooldown = 1.0f;
    public float tiempoAnimacionMuerte = 0.8f; // Duración de la animación de muerte

    private Rigidbody2D rb;
    private Vector2 movement;
    private Animator animator;
    private bool atacando = false;
    private bool enCooldown = false;
    private Transform player;
    private bool estaMuerto = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        animator.SetBool("IsWalking", false);
        animator.SetBool("IsAttacking", false);
    }

    void Update()
    {
        if (estaMuerto) return; // No hacer nada si ya está muerto

        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
            else
            {
                return;
            }
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer < detectionRadius)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            movement = new Vector2(direction.x, 0);

            if (distanceToPlayer < attackRadius)
            {
                if (!atacando && !enCooldown)
                {
                    atacando = true;
                    animator.SetBool("IsWalking", false);
                    animator.SetBool("IsAttacking", true);
                    Invoke(nameof(AtacarJugador), 0.5f);
                }
            }
            else
            {
                CancelarAtaque();
                animator.SetBool("IsWalking", true);
            }

            transform.localScale = new Vector3(direction.x > 0 ? 1 : -1, 1, 1);
        }
        else
        {
            movement = Vector2.zero;
            CancelarAtaque();
            animator.SetBool("IsWalking", false);
        }

        rb.MovePosition(rb.position + movement * speed * Time.deltaTime);
    }

    void AtacarJugador()
    {
        atacando = false;

        if (player == null)
        {
            animator.SetBool("IsAttacking", false);
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer < attackRadius)
        {
            PlayerController playerController = player.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.RecibeDanio(transform.position, 1);
            }

            StartCoroutine(IniciarCooldown());
        }
        else
        {
            CancelarAtaque();
        }

        animator.SetBool("IsAttacking", false);
    }

    void CancelarAtaque()
    {
        if (atacando)
        {
            atacando = false;
            CancelInvoke(nameof(AtacarJugador));
        }

        animator.SetBool("IsAttacking", false);
    }

    System.Collections.IEnumerator IniciarCooldown()
    {
        enCooldown = true;
        yield return new WaitForSeconds(attackCooldown);
        enCooldown = false;
    }
private void OnCollisionEnter2D(Collision2D collision)
{
    // Si ya está muerto, no hacer nada
    if (estaMuerto) return;

    // Verifica si el jugador lo tocó desde arriba
    if (collision.gameObject.CompareTag("Player") && collision.contacts[0].normal.y < -0.5f)
    {
        Debug.Log("¡Enemigo murió!");

        estaMuerto = true;
        CancelarAtaque(); // Asegúrate de tener este método definido o elimínalo si no aplica
        StopAllCoroutines();

        // Activar animación de muerte (Trigger)
        animator.SetTrigger("Morir");

        // Desactivar colisiones para que no vuelva a activarse
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        // Detener cualquier movimiento
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Static;
        }

        // Iniciar desvanecimiento
        StartCoroutine(DesvanecerYDestruir());
    }
}

private IEnumerator DesvanecerYDestruir()
{
    // Esperar duración de la animación de muerte (ajústalo si es necesario)
    yield return new WaitForSeconds(1.0f);

    SpriteRenderer sr = GetComponent<SpriteRenderer>();
    if (sr != null)
    {
        float duration = 1.5f; // segundos para desaparecer
        float elapsed = 0f;

        Color originalColor = sr.color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / duration);
            sr.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }
    }

    // Destruir el objeto al final
    Destroy(gameObject);
}


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}

