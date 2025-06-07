using UnityEngine;

// Esta clase permite dar una Cantidad de vida que esta fruta  puede restaurará al jugador cuando la recoja


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
    // Referencia publica para determinar cuanta vida restaura 
    public float cantidadVida = 1f;

    // Método OnTriggerEnter2 se activa automáticamente cuando otro collider entra en contacto con la fruta (marcado como Trigger)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Muestra en la consola con qué objeto colisionó la fruta (útil para depuración)
        Debug.Log("Colisión detectada con: " + collision.name);

        // Verifica si el objeto que colisionó tiene la etiqueta "Player"
        if (collision.CompareTag("Player"))
        {
            Debug.Log("El objeto es el jugador");

            // Intenta obtener el componente PlayerController del objeto que colisionó
            PlayerController player = collision.GetComponent<PlayerController>();

            // Verifica si encontró correctamente el script del jugador
            if (player != null)
            {
                Debug.Log("PlayerController encontrado");

                // Llama al método RestaurarVidaParcial del jugador y le pasa la cantidad de vida a restaurar
                player.RestaurarVidaParcial(cantidadVida);

                // Destruye el objeto fruta después de ser recolectado
                Destroy(gameObject);
            }
            else
            {
                // Si no se encontró el PlayerController, muestra una advertencia
                Debug.LogWarning("No se encontró el componente PlayerController en el jugador.");
            }
        }
    }
}
