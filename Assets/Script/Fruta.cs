using UnityEngine;


    /// <summary>
    /// Representa una fruta recolectable que restaura vida al jugador al entrar en contacto.
    /// </summary>
    /// <remarks>
    /// Este script debe estar adjunto a un objeto con un Collider2D marcado como Trigger.
    /// Se espera que el jugador tenga un componente <see cref="PlayerController"/> con el método <c>RestaurarVidaParcial</c>.
    /// </remarks>
    /// <example>
    /// Asigna este script a una fruta y asegúrate de configurar <see cref="cantidadVida"/> desde el Inspector.
    /// </example>
public class Fruta : MonoBehaviour
{

        /// <summary>
        /// Cantidad de vida que esta fruta restaurará al jugador.
        /// </summary>
        /// <value>
        /// Valor en puntos de vida, por defecto 1.
        /// </value>
    public float cantidadVida = 1f;


        /// <summary>
        /// Se ejecuta automáticamente cuando otro objeto con Collider2D entra en el trigger de la fruta.
        /// </summary>
        /// <param name="collision">El objeto que colisionó con la fruta.</param>
        /// <remarks>
        /// Si el objeto colisionado tiene la etiqueta "Player" y contiene un <see cref="PlayerController"/>, restaurará su vida y se destruirá esta fruta.
        /// </remarks>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Colisión detectada con: " + collision.name);

        if (collision.CompareTag("Player"))
        {
            Debug.Log("El objeto es el jugador");

            PlayerController player = collision.GetComponent<PlayerController>();

            if (player != null)
            {
                Debug.Log("PlayerController encontrado");

                player.RestaurarVidaParcial(cantidadVida);

                Destroy(gameObject);
            }
            else
            {
                Debug.LogWarning("No se encontró el componente PlayerController en el jugador.");
            }
        }
    }
}
