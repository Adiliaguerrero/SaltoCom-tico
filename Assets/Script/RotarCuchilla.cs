// Importamos el espacio de nombres necesario para trabajar con Unity
using UnityEngine;

// Esta clase trampa hace que un objeto gire continuamente para hacer daño sobre su eje Z con una velocidad configurable 
public class RotarCuchilla : MonoBehaviour
{
    // Velocidad de rotación en grados por segundo (ajustable desde el Inspector)
    public float velocidadRotacion = 180f;  

    // Método llamado una vez por frame
    void Update()
    {
        // Rotamos el objeto alrededor del eje Z
        // velocidadRotacion * Time.deltaTime asegura que la rotación sea independiente del frame rate
        transform.Rotate(0f, 0f, velocidadRotacion * Time.deltaTime);
    }
}
