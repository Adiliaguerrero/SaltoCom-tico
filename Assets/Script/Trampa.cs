using UnityEngine;

    /// <summary>
    /// Controla el comportamiento de un objeto tipo "espina" que inflige daño al jugador 
    /// al colisionar desde arriba.
    /// </summary>
public class Espina : MonoBehaviour
{
    // Cantidad de daño que la espina inflige al jugador al colisionar
    public int danio = 1;

    // Método que se ejecuta automáticamente cuando otro objeto colisiona con este objeto (con Rigidbody2D y Collider2D)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Intentamos obtener el componente PlayerController del objeto que colisionó, para verificar si es el jugador
        PlayerController jugador = collision.gameObject.GetComponent<PlayerController>();

        // Si el objeto colisionado tiene el componente PlayerController (es el jugador)
        if (jugador != null)
        {
            // Recorremos todos los puntos de contacto que tuvo la colisión
            foreach (ContactPoint2D punto in collision.contacts)
            {
                // Verificamos si la normal del punto de contacto tiene un componente Y menor a -0.5,
                // lo que indica que la colisión ocurrió desde arriba (el jugador cayó sobre la espina)
                if (punto.normal.y < -0.5f)
                {
                    // Preparamos la posición actual de la espina para usarla como referencia de dirección de daño
                    Vector2 direccionDanio = transform.position;

                    // Llamamos al método del jugador para que reciba daño, pasando la dirección y el valor de daño
                    jugador.RecibeDanio(direccionDanio, danio);

                    // Rompemos el ciclo para no procesar más puntos de contacto una vez aplicado el daño
                    break;
                }
            }
        }
    }
}
