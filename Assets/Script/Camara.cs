using UnityEngine;  

    /// <summary>
    /// Controla el seguimiento suave de la cámara al jugador en una dirección ajustada según su movimiento.
    /// </summary>
public class Camara : MonoBehaviour
{

        /// <summary>
        /// Objeto del jugador que la cámara debe seguir.
        /// </summary>
    public GameObject Player2;

    // Controla la suavidad del movimiento de la cámara (valor más alto = movimiento más suave)
        /// <summary>
        /// Factor de suavizado del movimiento de la cámara. Un valor mayor produce un movimiento más lento y suave.
        /// </summary>
    public float suavizado = 5f;

    // Desplazamiento de la cámara respecto al jugador (eje X e Y)
    public Vector2 desplazamiento = new Vector2(2f, 1f);

    // Variable interna para almacenar la velocidad usada por SmoothDamp
    private Vector3 velocidadSuavizado = Vector3.zero;

    // Última dirección horizontal conocida del jugador para posicionar la cámara adelante
    private float ultimaDireccionX = 1f;

    // Metodo LateUpdate se llama después de Update cada frame o segundo, ideal para seguir al jugador sin retrasos visuales
    void LateUpdate()
    {
        // Comprobar que el jugador no sea null o no extista  para evitar errores
        if (Player2 != null)
        {
            // Obtener la dirección horizontal actual basada en la escala X del jugador (puede ser 1 o -1)
            float direccionX = Player2.transform.localScale.x;

            // Actualizar la última dirección sólo si no es cero (para evitar errores)
            if (direccionX != 0)
            {
                ultimaDireccionX = direccionX;
            }

            // Calcular la posición objetivo sumando el desplazamiento ajustado por la dirección del jugador
            Vector3 posicionObjetivo = Player2.transform.position + new Vector3(desplazamiento.x * ultimaDireccionX, desplazamiento.y, 0f);

            // Mantener la posición Z actual para no cambiar la profundidad de la cámara
            posicionObjetivo.z = transform.position.z;

            // Mover la cámara suavemente desde su posición actual hacia la posición objetivo
            transform.position = Vector3.SmoothDamp(transform.position, posicionObjetivo, ref velocidadSuavizado, 2f / suavizado);
        }
    }
}
