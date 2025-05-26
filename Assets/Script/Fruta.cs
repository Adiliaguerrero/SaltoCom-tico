using UnityEngine;

public class Fruta : MonoBehaviour
{
    public float cantidadVida = 1f;

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

