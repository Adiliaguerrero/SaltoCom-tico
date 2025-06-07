using UnityEngine;
using System.Collections;

    /// <summary>
    /// Controla el comportamiento de un enemigo tipo Ninja, incluyendo detección del jugador, movimiento, ataque y muerte.
    /// </summary>
public class Ninja : MonoBehaviour
{
    
        /// <summary>
        /// Distancia máxima a la que el ninja puede detectar al jugador.
        /// </summary>
    public float detectionRadius = 5.0f;

    // Distancia mínima para iniciar el ataque al jugador
        /// <summary>
        /// Distancia mínima para que el ninja inicie un ataque.
        /// </summary>
    public float attackRadius = 1.0f;

        /// <summary>
        /// Velocidad de movimiento del ninja.
        /// </summary>
    public float speed = 2.0f;

        /// <summary>
        /// Tiempo de espera entre ataques consecutivos.
        /// </summary>
    public float attackCooldown = 1.0f;

        /// <summary>
        /// Tiempo que dura la animación de muerte antes de eliminar al ninja.
        /// </summary>
    public float tiempoAnimacionMuerte = 0.8f;

   
    private Rigidbody2D rb;              
    private Vector2 movement;           
    private Animator animator;          
    private bool atacando = false;      
    private bool enCooldown = false;    
    private Transform player;          
    private bool estaMuerto = false;    


        /// <summary>
        /// Inicializa componentes del ninja y animaciones al comenzar el juego.
        /// </summary>
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

     animator.SetBool("IsWalking", false);
        animator.SetBool("IsAttacking", false);
    }

    /// <summary>
    /// Lógica de detección, movimiento y ataque que se ejecuta cada frame.
    /// </summary>
    void Update()
    {
        if (estaMuerto) return;

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

        /// <summary>
        /// Ejecuta el ataque al jugador si está dentro del rango.
        /// </summary>
    void AtacarJugador()
    {
        atacando = false;  // Se marca que ya no se está atacando

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

    /// <summary>
    /// Cancela un ataque en curso y detiene su animación.
    /// </summary>
    void CancelarAtaque()
    {
        if (atacando)
        {
            atacando = false;
            CancelInvoke(nameof(AtacarJugador)); // Cancela el ataque programado
        }

        animator.SetBool("IsAttacking", false); // Detiene la animación de ataque
    }

    /// <summary>
    /// Inicia un período de espera entre ataques para evitar ataques consecutivos inmediatos.
    /// </summary>
    /// <returns>
    /// Una corrutina que espera el tiempo definido antes de permitir otro ataque.
    /// </returns>
    System.Collections.IEnumerator IniciarCooldown()
    {
        enCooldown = true; // Activa el cooldown
        yield return new WaitForSeconds(attackCooldown); // Espera el tiempo definido
        enCooldown = false; // Se puede volver a atacar
    }

    /// <summary>
    /// Maneja la lógica cuando el ninja colisiona con el jugador desde abajo, provocando su muerte.
    /// </summary>
    /// <param name="collision">Información sobre la colisión detectada.
    /// </param>
    /// 
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (estaMuerto) return;

        if (collision.gameObject.CompareTag("Player") && collision.contacts[0].normal.y < -0.5f)
        {
            Debug.Log("¡Enemigo murió!");

            estaMuerto = true;      
            CancelarAtaque();       
            StopAllCoroutines();    

            animator.SetTrigger("Morir");

            Collider2D col = GetComponent<Collider2D>();
            if (col != null) col.enabled = false;

            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = Vector2.zero;
                rb.bodyType = RigidbodyType2D.Static;
            }

            StartCoroutine(DesvanecerYDestruir());
        }
    }

    /// <summary>
    /// Corrutina que desvanecerá suavemente al ninja antes de destruirlo.
    /// </summary>
    /// <returns>Una corrutina que se ejecuta durante el proceso de desaparición.
    /// </returns>
    private IEnumerator DesvanecerYDestruir()
    {
        yield return new WaitForSeconds(1.0f);

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            float duration = 1.5f;  // Tiempo que tardará en desvanecerse
            float elapsed = 0f;     // Tiempo transcurrido

            Color originalColor = sr.color; // Guarda el color original

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float alpha = Mathf.Lerp(1f, 0f, elapsed / duration); // Interpola de opaco a transparente
                sr.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha); // Aplica nuevo color
                yield return null; // Espera al siguiente frame
            }
        }

        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius); // Radio de detección (rojo)

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRadius);    // Radio de ataque (azul)
    }
}