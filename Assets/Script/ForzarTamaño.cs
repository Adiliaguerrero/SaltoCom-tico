using UnityEngine;


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

    /// <summary>
    /// Escala correcta que debe aplicarse al objeto en los ejes X e Y.
    /// </summary>
    /// <value>
    /// Vector2 con la escala deseada. El valor X puede invertirse internamente para reflejar la dirección.
    /// </value>
    public Vector2 escalaCorrecta = new Vector2(0.48f, 0.45f);



        /// <summary>
        /// Se llama automáticamente después de todos los métodos Update.
        /// Aplica la escala fija y ajusta la orientación según la dirección.
        /// </summary>
        /// <remarks>
        /// Si el objeto está volteado horizontalmente, se conserva esa inversión al aplicar la escala.
        /// </remarks>
    private void LateUpdate()
    {
        float direccion = transform.localScale.x >= 0 ? 1 : -1;

        transform.localScale = new Vector3(escalaCorrecta.x * direccion, escalaCorrecta.y, 1f);
    }
}
