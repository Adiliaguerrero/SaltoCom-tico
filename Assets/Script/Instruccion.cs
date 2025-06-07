using UnityEngine;
using UnityEngine.UI;

// Esta clase gestiona una instruccion visual en el manejo de controles en pantalla usando imágenes animadas de manos

    /// <summary>
    /// Esta clase gestiona una instrucción visual en el manejo de controles en pantalla usando imágenes animadas de manos.
    /// </summary>
public class InstruccionesVisuales : MonoBehaviour
{
    // Referencia publica al asingnar en el isnpector la imagen de la mano izquierda
    public Image manoIzquierda;

    // Referencia publica al asingnar en el isnpector a la imagen de la mano derecha
    public Image manoDerecha;

    // Referencia publica al asingnar en el isnpector de la mano que indica toque
    public Image manoTocar;

    // Tiempo que tarda la animación de cada mano en completarse
    public float duracionAnimacion = 1f;

    // Cantidad de desplazamiento que realiza cada mano al animarse
    public float desplazamiento = 50f;

    // Referencia publica al asingnar en el isnpector para el joystick virtual utilizado para detectar movimiento
    public Joystick joystick;

    // Controla si la mano izquierda ya fue eliminada
    private bool izquierdaMostrada = true;

    // Controla si la mano derecha ya fue eliminada
    private bool derechaMostrada = true;

    // Controla si la mano de toque ya fue eliminada
    private bool tocarMostrado = true;

    // Método que se ejecuta al iniciar la escena
    void Start()
    {
        // Inicia animación para la mano izquierda hacia la izquierda
        AnimarMovimiento(manoIzquierda.rectTransform, -desplazamiento);

        // Inicia animación para la mano derecha hacia la derecha
        AnimarMovimiento(manoDerecha.rectTransform, desplazamiento);

        // Inicia animación para la mano de toque hacia abajo
        AnimarToque(manoTocar.rectTransform);
    }

    // Método que se ejecuta una vez por frame
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

    // Método que inicia la animación de movimiento horizontal de una mano
    void AnimarMovimiento(RectTransform mano, float direccionX)
    {
        // Guarda la posición inicial
        Vector3 inicio = mano.anchoredPosition;

        // Calcula la posición de destino aplicando el desplazamiento en X
        Vector3 destino = inicio + new Vector3(direccionX, 0, 0);

        // Llama a la corrutina para mover la mano entre inicio y destino
        StartCoroutine(MoverMano(mano, inicio, destino));
    }

    // Método que inicia la animación de movimiento vertical (toque) de una mano
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
