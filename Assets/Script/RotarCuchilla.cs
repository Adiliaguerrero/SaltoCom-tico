using UnityEngine;

    /// <summary>
    /// Hace que un objeto gire continuamente sobre su eje Z, simulando una cuchilla giratoria dañina.
    /// Ideal para trampas en niveles de juego.
    /// </summary>
public class RotarCuchilla : MonoBehaviour
{
    // Velocidad de rotación en grados por segundo (ajustable desde el Inspector)
        /// <summary>
        /// Velocidad de rotación en grados por segundo.
        /// Puede configurarse desde el Inspector de Unity.
        /// </summary>
    public float velocidadRotacion = 180f;

    
    /// <summary>
    /// Se ejecuta una vez por fotograma.
    /// Rota el objeto en el eje Z, de forma suave y constante, independiente del frame rate.
    /// </summary>
    void Update()
    {
        // Rotamos el objeto alrededor del eje Z
        // velocidadRotacion * Time.deltaTime asegura que la rotación sea independiente del frame rate
        transform.Rotate(0f, 0f, velocidadRotacion * Time.deltaTime);
    }
}
