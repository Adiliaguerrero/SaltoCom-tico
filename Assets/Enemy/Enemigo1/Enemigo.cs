using UnityEngine;

public class Enemigo : MonoBehaviour
{
    public float detectionRadius = 5.0f;
    public float attackRadius = 1.0f;
    public float speed = 2.0f;
    public float attackCooldown = 1.0f; // Tiempo entre ataques

    private Rigidbody2D rb;
    private Vector2 movement;
    private Animator animator;
    private bool atacando = false;
    private bool enCooldown = false;
    private Transform player;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        animator.SetBool("IsWalking", false);
        animator.SetBool("IsAttacking", false);
    }

    void Update()
    {
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

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }

   private bool estaMuerto = false; // Para evitar que muera varias veces

private void OnCollisionEnter2D(Collision2D collision)
{
    if (estaMuerto) return;

    if (collision.gameObject.CompareTag("Player") && collision.contacts[0].normal.y < 0)
    {
        estaMuerto = true;
        CancelarAtaque(); // Para cancelar cualquier ataque en curso
        StopAllCoroutines(); // Por si estaba atacando
        StartCoroutine(SquashAndDisappear());
    }
}

private System.Collections.IEnumerator SquashAndDisappear()
{
    // Cambiar a color rojo
    SpriteRenderer sr = GetComponent<SpriteRenderer>();
    if (sr != null)
    {
        sr.color = Color.red;
    }

    float duration = 0.2f;
    float timer = 0f;

    Vector3 originalScale = transform.localScale;
    Vector3 targetScale = new Vector3(originalScale.x * 1.5f, 0.1f, originalScale.z);

    while (timer < duration)
    {
        transform.localScale = Vector3.Lerp(originalScale, targetScale, timer / duration);
        timer += Time.deltaTime;
        yield return null;
    }

    transform.localScale = targetScale;
    yield return new WaitForSeconds(0.2f); // Espera antes de desaparecer

    Destroy(gameObject);
}
}

