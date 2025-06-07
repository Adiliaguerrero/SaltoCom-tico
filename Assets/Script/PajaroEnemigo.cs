using UnityEngine;
using System.Collections;


    /// <summary>
    /// Gestiona el comportamiento de un pájaro enemigo que se mueve horizontalmente y daña al jugador al contacto.
    /// </summary>
    /// <remarks>
    /// El pájaro vuela de un lado a otro en un rango definido y cambia de dirección tras un tiempo de espera.
    /// Si colisiona con el jugador, le inflige daño usando el método <c>RecibeDanio</c>.
    /// </remarks>
public class PajaroEnemigo : MonoBehaviour
{
    // Sección en el Inspector para ajustar el movimiento del pájaro
    [Header("Movimiento")]

    // Velocidad de movimiento del pájaro
    public float velocidad = 2f;

    // Distancia total que recorre en cada dirección antes de voltearse
    public float distanciaVuelo = 5f;

    // Tiempo que espera antes de cambiar de dirección
    public float tiempoEspera = 2f;

    // Sección en el Inspector para ajustar el daño que causa
    [Header("Daño")]

    // Cantidad de daño que inflige al jugador al tocarlo
    public int danio = 1;

    // Guarda la posición inicial para calcular el vuelo
    private Vector3 posicionInicial;

    // Indica si el pájaro se está moviendo hacia la derecha
    private bool moviendoDerecha = true;

    // Método Start se ejecuta al iniciar la escena
    void Start()
    {
        // Guarda la posición inicial del pájaro
        posicionInicial = transform.position;

        // Inicia la rutina de vuelo del pájaro
        StartCoroutine(RutinaVuelo());
    }

    // Corrutina que controla el movimiento de ida y vuelta del pájaro
    IEnumerator RutinaVuelo()
    {
        while (true)
        {
            // Calcula la posición X objetivo según la dirección actual
            float objetivoX = moviendoDerecha ? posicionInicial.x + distanciaVuelo : posicionInicial.x - distanciaVuelo;

            // Mientras no haya llegado al objetivo, se sigue moviendo en esa dirección
            while (Mathf.Abs(transform.position.x - objetivoX) > 0.1f)
            {
                // Define la dirección de movimiento (derecha o izquierda)
                Vector3 direccion = moviendoDerecha ? Vector3.right : Vector3.left;

                // Mueve al pájaro en la dirección correspondiente
                transform.Translate(direccion * velocidad * Time.deltaTime);

                // Espera un frame antes de continuar (para que la animación sea fluida)
                yield return null;
            }

            // Espera el tiempo definido antes de cambiar de dirección
            yield return new WaitForSeconds(tiempoEspera);

            // Cambia la dirección de movimiento
            moviendoDerecha = !moviendoDerecha;

            // Invierte la escala del objeto para que se voltee visualmente
            Voltear();
        }
    }

    // Método que voltea la escala horizontal del pájaro
    void Voltear()
    {
        // Toma la escala actual
        Vector3 escala = transform.localScale;

        // Invierte el eje X
        escala.x *= -1;

        // Aplica la nueva escala para voltear el sprite
        transform.localScale = escala;
    }

    // Método que se ejecuta al detectar una colisión con otro collider (modo Trigger)
    private void OnTriggerEnter2D(Collider2D otro)
    {
        // Intenta obtener el componente PlayerController del objeto que colisionó
        PlayerController jugador = otro.GetComponent<PlayerController>();

        // Si se encontró al jugador, le aplica daño
        if (jugador != null)
        {
            // Obtiene la posición del enemigo para determinar la dirección del daño
            Vector2 direccionDanio = transform.position;

            // Llama al método del jugador para recibir daño
            jugador.RecibeDanio(direccionDanio, danio);
        }
    }
}
