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


        /// <summary>
        /// Factor de suavizado del movimiento de la cámara. Un valor mayor produce un movimiento más lento y suave.
        /// </summary>
    public float suavizado = 5f;

        /// <summary>
        /// Desplazamiento en X e Y respecto al jugador para que la cámara no esté centrada directamente sobre él.
        /// </summary>
    public Vector2 desplazamiento = new Vector2(2f, 1f);


        /// <summary>
        /// Velocidad utilizada internamente por SmoothDamp para interpolar posiciones.
        /// </summary>
    private Vector3 velocidadSuavizado = Vector3.zero;

        /// <summary>
        /// Última dirección horizontal conocida del jugador para que la cámara mire hacia adelante.
        /// </summary>
    private float ultimaDireccionX = 1f;


    /// <summary>
    /// Actualiza la posición de la cámara de forma suave tras actualizar todos los objetos de la escena.
    /// Ideal para seguir al jugador sin causar saltos o retrasos visuales.
    /// </summary>
    void LateUpdate()
    {
        if (Player2 != null)
        {
            float direccionX = Player2.transform.localScale.x;

            if (direccionX != 0)
            {
                ultimaDireccionX = direccionX;
            }

            Vector3 posicionObjetivo = Player2.transform.position + new Vector3(desplazamiento.x * ultimaDireccionX, desplazamiento.y, 0f);

            posicionObjetivo.z = transform.position.z;

            transform.position = Vector3.SmoothDamp(transform.position, posicionObjetivo, ref velocidadSuavizado, 2f / suavizado);
        }
    }
}
