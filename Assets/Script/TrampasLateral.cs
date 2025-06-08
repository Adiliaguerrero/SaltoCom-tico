using UnityEngine;

    /// <summary>
    /// Representa una cuchilla que inflige daño al jugador al colisionar con él.
    /// </summary>
public class Cuchilla : MonoBehaviour
{
    /// <summary>
    /// Cantidad de daño que la cuchilla inflige al jugador al tocarlo.
    /// </summary>
    public int danio = 1;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController jugador = collision.gameObject.GetComponent<PlayerController>();

        if (jugador != null)
        {
            Vector2 direccionDanio = transform.position;

            jugador.RecibeDanio(direccionDanio, danio);
        }
    }
}
