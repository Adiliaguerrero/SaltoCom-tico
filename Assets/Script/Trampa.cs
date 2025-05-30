using UnityEngine;

    /// <summary>
    /// Controla el comportamiento de una trampa con espinas que daña al jugador al colisionar desde arriba.
    /// </summary>
    /// 
    /// <remarks>
    /// Este script debe colocarse en GameObjects con un <see cref="Collider2D"/> configurado como sólido (no como trigger).
    /// El objeto del jugador debe tener un componente <see cref="PlayerController"/> para recibir daño correctamente.
    /// </remarks>
public class Espina : MonoBehaviour
{
    /// <summary>
    /// Cantidad de daño que inflige la espina al jugador.
    /// </summary>

    public int danio = 1;

    /// <summary>
    /// Se ejecuta automáticamente cuando otro objeto colisiona con este GameObject que contiene un <see cref="Collider2D"/>.
    /// Si el objeto que colisiona tiene un componente <see cref="PlayerController"/> y el contacto se produce desde arriba,
    /// el jugador recibe daño.
    /// </summary>
    /// <param name="collision"></param>

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController jugador = collision.gameObject.GetComponent<PlayerController>();

        if (jugador != null)
        {

            foreach (ContactPoint2D punto in collision.contacts)
            {

                if (punto.normal.y < -0.5f)
                {
                    Vector2 direccionDanio = transform.position;
                    jugador.RecibeDanio(direccionDanio, danio);
                    break;
                }
            }
        }
    }
}
