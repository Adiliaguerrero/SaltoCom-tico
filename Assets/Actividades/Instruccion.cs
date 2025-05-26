 using UnityEngine;
using UnityEngine.UI;


public class InstruccionesVisuales : MonoBehaviour
{
    public Image manoIzquierda;
    public Image manoDerecha;
    public Image manoTocar;

    public float duracionAnimacion = 1f;
    public float desplazamiento = 50f;

    public Joystick joystick; 

    private bool izquierdaMostrada = true;
    private bool derechaMostrada = true;
    private bool tocarMostrado = true;

    void Start()
    {
        AnimarMovimiento(manoIzquierda.rectTransform, -desplazamiento);
        AnimarMovimiento(manoDerecha.rectTransform, desplazamiento);
        AnimarToque(manoTocar.rectTransform);
    }

    void Update()
    {
        float horizontal = joystick != null ? joystick.Horizontal : 0f;

        if (horizontal < -0.5f && izquierdaMostrada)
        {
            izquierdaMostrada = false;
            Destroy(manoIzquierda.gameObject);
        }
        else if (horizontal > 0.5f && derechaMostrada)
        {
            derechaMostrada = false;
            Destroy(manoDerecha.gameObject);
        }

        
        if (Input.touchCount > 0 && tocarMostrado)
        {
            Touch toque = Input.GetTouch(0);
            if (toque.phase == TouchPhase.Began && toque.position.x > Screen.width / 2)
            {
                tocarMostrado = false;
                Destroy(manoTocar.gameObject);
            }
        }

#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0) && tocarMostrado)
        {
            Vector2 mousePos = Input.mousePosition;
            if (mousePos.x > Screen.width / 2)
            {
                tocarMostrado = false;
                Destroy(manoTocar.gameObject);
            }
        }
#endif
    }

    void AnimarMovimiento(RectTransform mano, float direccionX)
    {
        Vector3 inicio = mano.anchoredPosition;
        Vector3 destino = inicio + new Vector3(direccionX, 0, 0);
        StartCoroutine(MoverMano(mano, inicio, destino));
    }

  void AnimarToque(RectTransform mano)
{
    Vector3 inicio = mano.anchoredPosition;
    Vector3 destino = inicio + new Vector3(0, -desplazamiento, 0);
    StartCoroutine(MoverMano(mano, inicio, destino));
}


    System.Collections.IEnumerator MoverMano(RectTransform mano, Vector3 desde, Vector3 hasta)
    {
        float t = 0f;
        while (t < duracionAnimacion)
        {
            mano.anchoredPosition = Vector3.Lerp(desde, hasta, t / duracionAnimacion);
            t += Time.deltaTime;
            yield return null;
        }

        mano.anchoredPosition = hasta;
 
        t = 0f;
        while (t < duracionAnimacion)
        {
            mano.anchoredPosition = Vector3.Lerp(hasta, desde, t / duracionAnimacion);
            t += Time.deltaTime;
            yield return null;
        }

        mano.anchoredPosition = desde;

        yield return new WaitForSeconds(0.5f);

        if (mano != null)
            StartCoroutine(MoverMano(mano, desde, hasta));
    }
}