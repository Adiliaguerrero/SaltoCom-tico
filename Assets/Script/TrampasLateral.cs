using UnityEngine;

public class Cuchilla: MonoBehaviour
{
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
