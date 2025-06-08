using UnityEngine;

    /// <summary>
    /// Controla el comportamiento de un objeto tipo "espina" que inflige daño al jugador 
    /// al colisionar desde arriba.
    /// </summary>
public class Espina : MonoBehaviour
{
    /// <summary>
    /// Cantidad de daño que esta espina inflige al jugador al ser tocada desde arriba.
    /// </summary>
    public int danio = 1;

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
