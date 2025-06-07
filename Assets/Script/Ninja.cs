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
        // Obtenemos referencias a los componentes necesarios del objeto
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // Inicializamos los estados de animación
        animator.SetBool("IsWalking", false);
        animator.SetBool("IsAttacking", false);
    }

    // Este método se ejecuta una vez por frame
        /// <summary>
        /// Lógica de detección, movimiento y ataque que se ejecuta cada frame.
        /// </summary>
    void Update()
    {
        // Si el ninja está muerto, no hace nada más
        if (estaMuerto) return;

        // Si aún no se ha encontrado al jugador, lo busca por su etiqueta
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
            else
            {
                // Si no encuentra al jugador, detiene la ejecución del frame
                return;
            }
        }

        // Calculamos la distancia actual entre el ninja y el jugador
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Si el jugador está dentro del rango de detección
        if (distanceToPlayer < detectionRadius)
        {
            // Calculamos la dirección en que debe moverse el ninja (solo horizontal)
            Vector2 direction = (player.position - transform.position).normalized;
            movement = new Vector2(direction.x, 0);

            // Si el jugador está dentro del rango de ataque
            if (distanceToPlayer < attackRadius)
            {
                // Si no está atacando ni en cooldown, lanza un ataque
                if (!atacando && !enCooldown)
                {
                    atacando = true;
                    animator.SetBool("IsWalking", false);
                    animator.SetBool("IsAttacking", true);

                    // Iniciamos el ataque con un pequeño retraso para sincronizar con la animación
                    Invoke(nameof(AtacarJugador), 0.5f);
                }
            }
            else
            {
                // Si el jugador está fuera del rango de ataque, se cancela el ataque
                CancelarAtaque();
                animator.SetBool("IsWalking", true);
            }

            // Se ajusta la escala del ninja para que mire hacia el jugador
            transform.localScale = new Vector3(direction.x > 0 ? 1 : -1, 1, 1);
        }
        else
        {
            // Si el jugador está fuera del rango de detección, se detiene el movimiento y las animaciones
            movement = Vector2.zero;
            CancelarAtaque();
            animator.SetBool("IsWalking", false);
        }

        // Movimiento del ninja en el mundo usando física
        rb.MovePosition(rb.position + movement * speed * Time.deltaTime);
    }

    // Método que se llama para ejecutar el ataque al jugador
        /// <summary>
        /// Ejecuta el ataque al jugador si está dentro del rango.
        /// </summary>
    void AtacarJugador()
    {
        atacando = false;  // Se marca que ya no se está atacando

        // Si no hay referencia al jugador, se detiene la animación de ataque y se sale
        if (player == null)
        {
            animator.SetBool("IsAttacking", false);
            return;
        }

        // Calcula la distancia al jugador
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Si el jugador sigue dentro del rango de ataque
        if (distanceToPlayer < attackRadius)
        {
            // Busca el componente PlayerController en el jugador para aplicar daño
            PlayerController playerController = player.GetComponent<PlayerController>();
            if (playerController != null)
            {
                // Llama al método para que el jugador reciba daño
                playerController.RecibeDanio(transform.position, 1);
            }

            // Inicia el tiempo de espera antes de permitir otro ataque
            StartCoroutine(IniciarCooldown());
        }
        else
        {
            // Si el jugador ya se alejó, se cancela el ataque
            CancelarAtaque();
        }

        // Se actualiza el estado de la animación para reflejar que no está atacando
        animator.SetBool("IsAttacking", false);
    }

    // Método para cancelar un ataque en progreso
    void CancelarAtaque()
    {
        if (atacando)
        {
            atacando = false;
            CancelInvoke(nameof(AtacarJugador)); // Cancela el ataque programado
        }

        animator.SetBool("IsAttacking", false); // Detiene la animación de ataque
    }

    // Corrutina que inicia un cooldown entre ataques
    System.Collections.IEnumerator IniciarCooldown()
    {
        enCooldown = true; // Activa el cooldown
        yield return new WaitForSeconds(attackCooldown); // Espera el tiempo definido
        enCooldown = false; // Se puede volver a atacar
    }

    // Método que se ejecuta cuando ocurre una colisión con este objeto
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Si el ninja ya está muerto, no hace nada
        if (estaMuerto) return;

        // Verifica si la colisión fue con el jugador y si el contacto fue desde arriba
        if (collision.gameObject.CompareTag("Player") && collision.contacts[0].normal.y < -0.5f)
        {
            Debug.Log("¡Enemigo murió!");

            estaMuerto = true;      // Marca al ninja como muerto
            CancelarAtaque();       // Detiene cualquier ataque pendiente
            StopAllCoroutines();    // Detiene todas las corrutinas activas

            // Inicia la animación de muerte
            animator.SetTrigger("Morir");

            // Desactiva el collider del enemigo para que ya no interactúe físicamente
            Collider2D col = GetComponent<Collider2D>();
            if (col != null) col.enabled = false;

            // Detiene el movimiento y convierte al Rigidbody en estático
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = Vector2.zero;
                rb.bodyType = RigidbodyType2D.Static;
            }

            // Llama a una corrutina para hacer desaparecer y destruir el enemigo
            StartCoroutine(DesvanecerYDestruir());
        }
    }

    // Corrutina que hace desaparecer al enemigo suavemente antes de destruirlo
    private IEnumerator DesvanecerYDestruir()
    {
        // Espera 1 segundo antes de comenzar la desaparición
        yield return new WaitForSeconds(1.0f);

        // Obtiene el componente SpriteRenderer para modificar la opacidad
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            float duration = 1.5f;  // Tiempo que tardará en desvanecerse
            float elapsed = 0f;     // Tiempo transcurrido

            Color originalColor = sr.color; // Guarda el color original

            // Bucle que va disminuyendo el valor alfa (opacidad) del sprite
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float alpha = Mathf.Lerp(1f, 0f, elapsed / duration); // Interpola de opaco a transparente
                sr.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha); // Aplica nuevo color
                yield return null; // Espera al siguiente frame
            }
        }

        // Destruye el objeto una vez que se ha desvanecido completamente
        Destroy(gameObject);
    }

    // Método que se ejecuta en el editor para mostrar visualmente los radios de detección y ataque
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius); // Radio de detección (rojo)

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRadius);    // Radio de ataque (azul)
    }
}