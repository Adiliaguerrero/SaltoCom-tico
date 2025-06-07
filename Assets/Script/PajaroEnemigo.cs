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
    
    /// <summary>
    /// Velocidad de movimiento del pájaro.
    /// </summary>
    [Header("Movimiento")]
    public float velocidad = 2f;

    /// <summary>
    /// Distancia máxima que el pájaro recorre en una dirección antes de cambiar.
    /// </summary>
    public float distanciaVuelo = 5f;

    // Tiempo que espera antes de cambiar de dirección
    /// <summary>
    /// Tiempo de espera antes de invertir la dirección del vuelo.
    /// </summary>
    public float tiempoEspera = 2f;

    /// <summary>
    /// Cantidad de daño que se aplica al jugador al entrar en contacto.
    /// </summary>
    [Header("Daño")]
    public int danio = 1;

    private Vector3 posicionInicial;

    private bool moviendoDerecha = true;

    void Start()
    {
        posicionInicial = transform.position;

        StartCoroutine(RutinaVuelo());
    }

    IEnumerator RutinaVuelo()
    {
        while (true)
        {
            float objetivoX = moviendoDerecha ? posicionInicial.x + distanciaVuelo : posicionInicial.x - distanciaVuelo;

            while (Mathf.Abs(transform.position.x - objetivoX) > 0.1f)
            {
                Vector3 direccion = moviendoDerecha ? Vector3.right : Vector3.left;

                transform.Translate(direccion * velocidad * Time.deltaTime);

                yield return null;
            }

            yield return new WaitForSeconds(tiempoEspera);

            moviendoDerecha = !moviendoDerecha;

            Voltear();
        }
    }

    void Voltear()
    {
        Vector3 escala = transform.localScale;

        escala.x *= -1;

        transform.localScale = escala;
    }

    private void OnTriggerEnter2D(Collider2D otro)
    {
        PlayerController jugador = otro.GetComponent<PlayerController>();

        if (jugador != null)
        {
            Vector2 direccionDanio = transform.position;

            jugador.RecibeDanio(direccionDanio, danio);
        }
    }
}
