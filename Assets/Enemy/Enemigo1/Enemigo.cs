using UnityEngine;

public class Enemigo : MonoBehaviour
{
    public float detectionRadius = 5.0f;
    public float attackRadius = 1.0f;
    public float speed = 2.0f;

    private Rigidbody2D rb;
    private Vector2 movement;
    private Animator animator;
    private bool atacando = false;
    private Transform player; // Referencia persistente al Player

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // Inicializar animaciones en estado Idle
        animator.SetBool("IsWalking", false);
        animator.SetBool("IsAttacking", false);
    }

    void Update()
    {
        // Buscar al Player si aún no está referenciado
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
            else
            {
                return; // Si no hay Player, salir del Update
            }
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer < detectionRadius)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            movement = new Vector2(direction.x, 0);

            if (distanceToPlayer < attackRadius)
            {
                if (!atacando)
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
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer < attackRadius)
        {
            PlayerController playerController = player.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.RecibeDanio(transform.position, 1);
            }
        }
        else
        {
            CancelarAtaque();
        }
    }

    void CancelarAtaque()
    {
        atacando = false;
        animator.SetBool("IsAttacking", false);
        animator.SetBool("IsWalking", false);
        CancelInvoke(nameof(AtacarJugador));
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && collision.contacts[0].normal.y < 0)
        {
            Destroy(gameObject);
        }
    }
}

