using UnityEngine;
using UnityEngine.UI;

// Esta clase gestiona una instruccion visual en el manejo de controles en pantalla usando imágenes animadas de manos

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
        // Inicia animación para la mano izquierda hacia la izquierda
        AnimarMovimiento(manoIzquierda.rectTransform, -desplazamiento);

        // Inicia animación para la mano derecha hacia la derecha
        AnimarMovimiento(manoDerecha.rectTransform, desplazamiento);

        // Inicia animación para la mano de toque hacia abajo
        AnimarToque(manoTocar.rectTransform);
    }

    /// <summary>
    /// Verifica la interacción del jugador y elimina las instrucciones visuales correspondientes.
    /// </summary>
    void Update()
    {
        // Obtiene el valor horizontal del joystick (si existe)
        float horizontal = joystick != null ? joystick.Horizontal : 0f;

        // Si el joystick se mueve hacia la izquierda y la mano izquierda aún está visible
        if (horizontal < -0.5f && izquierdaMostrada)
        {
            // Marca la mano izquierda como eliminada y destruye su objeto
            izquierdaMostrada = false;
            Destroy(manoIzquierda.gameObject);
        }
        // Si el joystick se mueve hacia la derecha y la mano derecha aún está visible
        else if (horizontal > 0.5f && derechaMostrada)
        {
            // Marca la mano derecha como eliminada y destruye su objeto
            derechaMostrada = false;
            Destroy(manoDerecha.gameObject);
        }

        // Si se detecta un toque en pantalla y la mano de toque aún está visible
        if (Input.touchCount > 0 && tocarMostrado)
        {
            // Se obtiene el primer toque
            Touch toque = Input.GetTouch(0);

            // Si el toque ha comenzado y está en la parte derecha de la pantalla
            if (toque.phase == TouchPhase.Began && toque.position.x > Screen.width / 2)
            {
                // Marca la mano de toque como eliminada y destruye su objeto
                tocarMostrado = false;
                Destroy(manoTocar.gameObject);
            }
        }

#if UNITY_EDITOR
        // Simula el comportamiento táctil con el clic del mouse en el editor de Unity
        if (Input.GetMouseButtonDown(0) && tocarMostrado)
        {
            // Se obtiene la posición del clic
            Vector2 mousePos = Input.mousePosition;

            // Si el clic está en el lado derecho de la pantalla
            if (mousePos.x > Screen.width / 2)
            {
                // Marca la mano de toque como eliminada y destruye su objeto
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
        // Guarda la posición inicial
        Vector3 inicio = mano.anchoredPosition;

        // Calcula la posición de destino aplicando el desplazamiento en X
        Vector3 destino = inicio + new Vector3(direccionX, 0, 0);

        // Llama a la corrutina para mover la mano entre inicio y destino
        StartCoroutine(MoverMano(mano, inicio, destino));
    }

    
    /// <summary>
    /// Anima la mano de toque con un movimiento vertical hacia abajo.
    /// </summary>
    /// <param name="mano">RectTransform de la mano de toque.</param>
    void AnimarToque(RectTransform mano)
    {
        // Guarda la posición inicial
        Vector3 inicio = mano.anchoredPosition;

        // Calcula la posición de destino aplicando el desplazamiento en Y hacia abajo
        Vector3 destino = inicio + new Vector3(0, -desplazamiento, 0);

        // Llama a la corrutina para mover la mano entre inicio y destino
        StartCoroutine(MoverMano(mano, inicio, destino));
    }

    // Corrutina que mueve una mano de una posición a otra, y luego regresa, en bucle
    System.Collections.IEnumerator MoverMano(RectTransform mano, Vector3 desde, Vector3 hasta)
    {
        // Contador de tiempo
        float t = 0f;

        // Movimiento hacia el destino
        while (t < duracionAnimacion)
        {
            // Interpola la posición entre inicio y destino según el tiempo transcurrido
            mano.anchoredPosition = Vector3.Lerp(desde, hasta, t / duracionAnimacion);
            t += Time.deltaTime;
            yield return null;
        }

        // Asegura que la posición final sea exacta
        mano.anchoredPosition = hasta;

        // Reinicia el contador de tiempo
        t = 0f;

        // Movimiento de regreso hacia la posición original
        while (t < duracionAnimacion)
        {
            // Interpola la posición desde el destino de vuelta al origen
            mano.anchoredPosition = Vector3.Lerp(hasta, desde, t / duracionAnimacion);
            t += Time.deltaTime;
            yield return null;
        }

        // Asegura que la posición final sea exacta
        mano.anchoredPosition = desde;

        // Espera un breve momento antes de repetir la animación
        yield return new WaitForSeconds(0.5f);

        // Si la mano aún no ha sido destruida, repite la animación en bucle
        if (mano != null)
            StartCoroutine(MoverMano(mano, desde, hasta));
    }
}
