using UnityEngine;

/// <summary>
/// Controla el comportamiento de una trampa con espinas que daña al jugador al colisionar desde arriba.
/// </summary>
public class Espina : MonoBehaviour
{
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
