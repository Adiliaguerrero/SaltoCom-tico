using UnityEngine;

public class Camara : MonoBehaviour
{
    public GameObject Player2;
    public float suavizado = 5f;
    public Vector2 desplazamiento = new Vector2(2f, 1f);

    private Vector3 velocidadSuavizado = Vector3.zero;
    private float ultimaDireccionX = 1f;

    void LateUpdate()
    {
        if (Player2 != null)
        {
            // Obtener dirección del personaje
            float direccionX = Player2.transform.localScale.x;
            if (direccionX != 0)
            {
                ultimaDireccionX = direccionX;
            }

          
            Vector3 posicionObjetivo = Player2.transform.position + new Vector3(desplazamiento.x * ultimaDireccionX, desplazamiento.y, 0f);
            posicionObjetivo.z = transform.position.z;

            // Suavizar movimiento de cámara
            transform.position = Vector3.SmoothDamp(transform.position, posicionObjetivo, ref velocidadSuavizado, 2f / suavizado);
        }
    }
}
