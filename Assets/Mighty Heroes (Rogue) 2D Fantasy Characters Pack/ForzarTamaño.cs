using UnityEngine;

public class ForzarEscala : MonoBehaviour
{
    public Vector2 escalaCorrecta = new Vector2(0.48f, 0.45f);

    private void LateUpdate()
    {
        float direccion = transform.localScale.x >= 0 ? 1 : -1;
        transform.localScale = new Vector3(escalaCorrecta.x * direccion, escalaCorrecta.y, 1f);
    }
}

