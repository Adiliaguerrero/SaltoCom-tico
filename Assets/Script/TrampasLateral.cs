using UnityEngine;

    /// <summary>
    /// Representa una cuchilla que inflige daño al jugador al colisionar con él.
    /// </summary>
public class Cuchilla : MonoBehaviour
{
    // Cantidad de daño que la cuchilla inflige al jugador al tocarlo
    public int danio = 1;

    // Método que se ejecuta automáticamente cuando otro objeto colisiona con este objeto (usando Rigidbody2D y Collider2D)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Intentamos obtener el componente PlayerController del objeto que colisionó, para verificar si es el jugador
        PlayerController jugador = collision.gameObject.GetComponent<PlayerController>();

        // Si el objeto que colisionó es el jugador
        if (jugador != null)
        {
            // Definimos la posición actual de la cuchilla para usar como dirección del daño
            Vector2 direccionDanio = transform.position;

            // Llamamos al método del jugador para que reciba daño, pasando la dirección y la cantidad de daño
            jugador.RecibeDanio(direccionDanio, danio);
        }
    }
}
