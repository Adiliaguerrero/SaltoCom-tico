using UnityEngine;
using System.Collections;

public class PajaroEnemigo : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidad = 2f;
    public float distanciaVuelo = 5f;
    public float tiempoEspera = 2f;

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

