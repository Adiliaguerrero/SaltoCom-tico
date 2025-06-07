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
        // Guardamos la posición original de la cámara (antes de moverse).
        Vector3 posicionOriginal = transform.localPosition;

        // Creamos una variable llamada "tiempo" que empieza en cero.
        float tiempo = 0f;

        // Esta parte se repite mientras el tiempo total sea menor que la duración de la sacudida.
        while (tiempo < duracion)
        {
            // Generamos un número al azar entre -1 y 1 para el movimiento horizontal (x).
            // Lo multiplicamos por la magnitud para controlar qué tan fuerte se mueve.
            float offsetX = Random.Range(-1f, 1f) * magnitud;

            // Lo mismo, pero para el movimiento vertical (y).
            float offsetY = Random.Range(-1f, 1f) * magnitud;

            // Movemos la cámara a una nueva posición sumando el movimiento al azar.
            // El valor "0" es para no mover la cámara hacia adelante o atrás (eje Z).
            transform.localPosition = posicionOriginal + new Vector3(offsetX, offsetY, 0);

            // Aumentamos el tiempo con el valor del tiempo que ha pasado desde el último cuadro.
            tiempo += Time.deltaTime;

            // Esta línea hace una pequeña pausa hasta el siguiente cuadro (frame).
            yield return null;
        }

        // Al final de la sacudida, regresamos la cámara a su posición original.
        transform.localPosition = posicionOriginal;
    }
}
