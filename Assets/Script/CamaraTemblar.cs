using UnityEngine;

using System.Collections;

    /// <summary>
    /// Permite aplicar un efecto de sacudida o "temblor" a la cámara, ideal para representar impactos o daños.
    /// </summary>
public class CameraShake : MonoBehaviour
{

        /// <summary>
        /// Duración total de la sacudida en segundos.
        /// </summary>
    public float duracion = 0.2f;

        /// <summary>
        /// Magnitud o intensidad del temblor. Controla qué tanto se moverá la cámara.
        /// </summary>
    public float magnitud = 0.3f;


        /// <summary>
        /// Método público que inicia el efecto de sacudida de la cámara.
        /// Puede ser llamado desde otros scripts o eventos del juego.
        /// </summary>
    public void Sacudir()
    {
        // Aquí empezamos una "corutina", que es como una función que se ejecuta poco a poco con el tiempo.
        StartCoroutine(Shake());
    }

    // Esta es la función que realmente hace que la cámara se sacuda.
    // IEnumerator permite que esta función se ejecute con pausas entre cada paso (no todo de golpe).

        /// <summary>
        /// Corrutina que implementa la sacudida de la cámara durante el tiempo definido.
        /// </summary>
        /// <returns>
        /// Devuelve un enumerador necesario para la ejecución como corrutina.
        /// </returns>
    private IEnumerator Shake()
    {
        Vector3 posicionOriginal = transform.localPosition;

        float tiempo = 0f;

        while (tiempo < duracion)
        {

            float offsetX = Random.Range(-1f, 1f) * magnitud;

            float offsetY = Random.Range(-1f, 1f) * magnitud;


            transform.localPosition = posicionOriginal + new Vector3(offsetX, offsetY, 0);

            tiempo += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = posicionOriginal;
    }
}
