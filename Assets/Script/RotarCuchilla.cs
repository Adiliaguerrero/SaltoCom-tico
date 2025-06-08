using UnityEngine;

    /// <summary>
    /// Hace que un objeto gire continuamente sobre su eje Z, simulando una cuchilla giratoria dañina.
    /// Ideal para trampas en niveles de juego.
    /// </summary>
public class RotarCuchilla : MonoBehaviour
{
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
        transform.Rotate(0f, 0f, velocidadRotacion * Time.deltaTime);
    }
}
