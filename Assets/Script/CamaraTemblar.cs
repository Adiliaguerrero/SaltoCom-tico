 using UnityEngine;
using System.Collections;

    /// <summary>
    /// Controla un efecto de sacudida en la cámara para simular impactosinfligidos por enemigos u objetos. 
    /// Este efecto mejora la inmersion visual del jugador.
    /// </summary>
    /// 
    /// <remmark>
    /// Para que este efecto funcione correctamente, el GameObject que contiene este script debe ser la cámara.
    /// Puede ser llamado desde otros scripts cuando se detecten eventos como recibir daño por enemigos o trampas.
    /// </remmark>
    public class CameraShake : MonoBehaviour
{
        /// <summary>
        ///Duración total del efecto de sacudida en segundos.
        /// </summary>
    public float duracion = 0.2f;

        /// <summary>
        /// Intensidad máxima del movimiento durante la sacudida
        /// </summary> 
    public float magnitud = 0.3f;

        /// <summary>
        /// Inicia el efecto de sacudida de la cámara.
        /// </summary>
    public void Sacudir()
    {
        StartCoroutine(Shake());
    }

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
