using UnityEngine;
using UnityEngine.UI;


    /// <summary>
    /// Esta clase gestiona una instrucción visual en el manejo de controles en pantalla usando imágenes animadas de manos.
    /// </summary>
public class InstruccionesVisuales : MonoBehaviour
{
    /// <summary>
    /// Imagen de la mano izquierda mostrada en pantalla.
    /// </summary>
    public Image manoIzquierda;

    
    /// <summary>
    /// Imagen de la mano derecha mostrada en pantalla.
    /// </summary>
    public Image manoDerecha;

    /// <summary>
    /// Imagen de la mano que indica el toque en pantalla.
    /// </summary>
    public Image manoTocar;


    /// <summary>
    /// Duración de la animación para cada mano.
    /// </summary>
    public float duracionAnimacion = 1f;

    /// <summary>
    /// Cantidad de desplazamiento que realiza cada mano en su animación.
    /// </summary>
    public float desplazamiento = 50f;

    /// <summary>
    /// Referencia al joystick utilizado para detectar movimientos.
    /// </summary>
    public Joystick joystick;

    /// <summary>
    /// Indica si la mano izquierda sigue activa en pantalla.
    /// </summary>
    private bool izquierdaMostrada = true;

    
    /// <summary>
    /// Indica si la mano derecha sigue activa en pantalla.
    /// </summary>
    private bool derechaMostrada = true;

    /// <summary>
    /// Indica si la mano de toque sigue activa en pantalla.
    /// </summary>
    private bool tocarMostrado = true;

    /// <summary>
    /// Inicializa las animaciones de manos al comenzar la escena.
    /// </summary>
    void Start()
    {
        AnimarMovimiento(manoIzquierda.rectTransform, -desplazamiento);

        AnimarMovimiento(manoDerecha.rectTransform, desplazamiento);

        AnimarToque(manoTocar.rectTransform);
    }

    /// <summary>
    /// Verifica la interacción del jugador y elimina las instrucciones visuales correspondientes.
    /// </summary>
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
    
    /// <summary>
    /// Anima una mano con movimiento horizontal hacia una dirección específica.
    /// </summary>
    /// <param name="mano">RectTransform de la mano a animar.</param>
    /// <param name="direccionX">Desplazamiento horizontal.</param>
    void AnimarMovimiento(RectTransform mano, float direccionX)
    {
        Vector3 inicio = mano.anchoredPosition;

        Vector3 destino = inicio + new Vector3(direccionX, 0, 0);

        StartCoroutine(MoverMano(mano, inicio, destino));
    }

    
    /// <summary>
    /// Anima la mano de toque con un movimiento vertical hacia abajo.
    /// </summary>
    /// <param name="mano">RectTransform de la mano de toque.</param>
    void AnimarToque(RectTransform mano)
    {
        Vector3 inicio = mano.anchoredPosition;

        Vector3 destino = inicio + new Vector3(0, -desplazamiento, 0);

        StartCoroutine(MoverMano(mano, inicio, destino));
    }

    /// <summary>
    /// Corrutina que mueve la mano entre dos posiciones y repite la animación en bucle.
    /// </summary>
    /// <param name="mano">RectTransform de la mano a animar.</param>
    /// <param name="desde">Posición inicial.</param>
    /// <param name="hasta">Posición final.</param>
    /// <returns>IEnumerator para control de corrutina.</returns>
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
