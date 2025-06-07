using UnityEngine;

// Esta clase se encarga de mantener una escala fija en el objeto al que está asignado,
// permitiendo que se voltee horizontalmente si es necesario (por ejemplo, al mirar hacia la izquierda o derecha)


    /// <summary>
    /// Mantiene una escala constante en el objeto y ajusta su orientación horizontal (mirando a la izquierda o derecha).
    /// </summary>
    /// <remarks>
    /// Se recomienda usar este script para personajes o sprites que deban conservar su tamaño original sin distorsión,
    /// pero aún puedan girar horizontalmente.
    /// </remarks>
    /// <example>
    /// Asigna este script a un GameObject y configura su <see cref="escalaCorrecta"/> desde el Inspector.
    /// </example>
public class ForzarEscala : MonoBehaviour
{
    // Variable pública que permite definir desde el Inspector la escala correcta del objeto (X, Y)
    public Vector2 escalaCorrecta = new Vector2(0.48f, 0.45f);

    // Este método se llama una vez por frame, pero después de que todos los demás métodos Update hayan sido ejecutados
    // Se utiliza para asegurar que la escala se aplique correctamente al final del frame
    private void LateUpdate()
    {
        // Determina si el objeto está mirando hacia la derecha (1) o hacia la izquierda (-1) según su escala en X
        float direccion = transform.localScale.x >= 0 ? 1 : -1;

        // Aplica la escala correcta, manteniendo la dirección original del objeto (positiva o negativa en X)
        transform.localScale = new Vector3(escalaCorrecta.x * direccion, escalaCorrecta.y, 1f);
    }
}
