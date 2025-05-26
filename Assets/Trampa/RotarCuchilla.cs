using UnityEngine;

public class RotarCuchilla : MonoBehaviour
{
    public float velocidadRotacion = 180f; // Grados por segundo

    void Update()
    {
        transform.Rotate(0f, 0f, velocidadRotacion * Time.deltaTime);
    }
}

