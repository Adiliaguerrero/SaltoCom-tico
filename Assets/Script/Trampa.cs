using UnityEngine;

    /// <summary>
    /// Controla el comportamiento de una trampa con espinas que daña al jugador al colisionar desde arriba.
    /// </summary>
    /// 
    /// <remarks>
    /// Requiere que el jugador tenga un componente <see cref="PlayerController"/> con el método 
    /// <see cref="PlayerController.RecibeDanio(Vector2, int)"/> implementado.
    /// </remarks>
public class Espina : MonoBehaviour
{
    /// <summary>
    /// Cantidad de daño que inflige la espina al jugador.
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
