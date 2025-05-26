 using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    public float duracion = 0.2f;
    public float magnitud = 0.3f;

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
