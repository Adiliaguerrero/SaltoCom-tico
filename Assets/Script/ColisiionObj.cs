using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


    /// <summary>
    /// Maneja la colisión del jugador con un objeto trigger que activa un portal,
    /// detiene al jugador y cambia de escena mostrando un mensaje de bienvenida.
    /// </summary>
public class ColisionadorTrigger2D : MonoBehaviour
{
  
        /// <summary>
        /// Controla si ya ha ocurrido la colisión para evitar múltiples activaciones.
        /// </summary>
    private bool haColisionado = false;

        /// <summary>
        /// Portal que se activará para permitir el cambio de escena.
        /// </summary>
    public GameObject portal;


        /// <summary>
        /// Interfaz del joystick, que será deshabilitada al cambiar de escena.
        /// </summary>
    public GameObject joystickUI;

    // Referencias internas para manipular el Rigidbody y el controlador del jugador
        /// <summary>
        /// Referencia al Rigidbody2D del jugador.
        /// </summary>
    private Rigidbody2D rbJugador;

        /// <summary>
        /// Referencia al script de control del jugador.
        /// </summary>
    private PlayerController playerController;

    // Nombre de la escena a la que se desea cambiar, asignado en el Inspector
    public string nombreEscena = "Escena";

    // Variables estáticas para pasar el mensaje a mostrar en la escena destino
    private static string textoMensaje = "¡Nivel Basico!";
    private static string nombreObjetoTextoBienvenida = "TextoBienvenida"; // Nombre del objeto UI en la escena destino
    private static float duracionMensaje = 3f; // Duración que estará visible el mensaje
    private static bool mostrarMensaje = false; // Controla si se debe mostrar el mensaje

    // Detecta cuando un objeto colisiona con este trigger
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Si ya colisionó antes, no hacer nada
        if (haColisionado) return;

        // Verifica que el objeto que colisiona sea el jugador (por etiqueta)
        if (collision.gameObject.CompareTag("Player"))
        {
            haColisionado = true;

            // Deshabilita el joystick para que el jugador no pueda mover mientras se realiza la transición
            if (joystickUI != null)
            {
                FixedJoystick joystick = joystickUI.GetComponent<FixedJoystick>();
                if (joystick != null)
                {
                    joystick.OnPointerUp(null); // Simula levantar el dedo del joystick
                    joystick.enabled = false;
                }
                joystickUI.SetActive(false);
            }

            // Congela la física del jugador para detenerlo
            rbJugador = collision.gameObject.GetComponent<Rigidbody2D>();
            if (rbJugador != null)
            {
                rbJugador.velocity = Vector2.zero;
                rbJugador.bodyType = RigidbodyType2D.Static;
            }

            // Deshabilita el control de movimiento del jugador
            playerController = collision.gameObject.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.puedeMoverse = false;
            }

            // Activa el portal ya existente en la escena para permitir el cambio de escena
            if (portal != null)
            {
                portal.SetActive(true);
                // Asegura que el portal tenga el script PortalTrigger para detectar la entrada del jugador
                PortalTrigger trigger = portal.GetComponent<PortalTrigger>();
                if (trigger == null)
                {
                    trigger = portal.AddComponent<PortalTrigger>();
                }
                trigger.nombreEscena = nombreEscena;
            }

            // Inicia la espera y muestra un diálogo (o cualquier otro efecto antes de cargar la escena)
            StartCoroutine(EsperarYMostrarDialogo());
        }
    }

    // Corrutina que espera unos segundos antes de mostrar diálogo o continuar
    private IEnumerator EsperarYMostrarDialogo()
    {
        yield return new WaitForSeconds(3f);

        // Busca el controlador de diálogo en la escena actual y muestra el diálogo
        DialogoController dialogo = FindObjectOfType<DialogoController>();
        if (dialogo != null)
        {
            dialogo.MostrarDialogo(() =>
            {
                // Una vez terminado el diálogo, reactivamos el joystick y el movimiento del jugador
                if (joystickUI != null)
                {
                    joystickUI.SetActive(true);
                    FixedJoystick joystick = joystickUI.GetComponent<FixedJoystick>();
                    if (joystick != null)
                        joystick.enabled = true;
                }

                if (rbJugador != null)
                    rbJugador.bodyType = RigidbodyType2D.Dynamic;

                if (playerController != null)
                    playerController.puedeMoverse = true;

                // Eliminamos este objeto y su collider para que no se active de nuevo
                Destroy(GetComponent<Collider2D>());
                Destroy(gameObject);
            });
        }
    }

    // Clase interna que detecta la entrada del jugador en el portal y realiza el cambio de escena
    private class PortalTrigger : MonoBehaviour
    {
        // Nombre de la escena destino
        public string nombreEscena;

        // Detecta cuando el jugador entra en el portal
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                // Configura el mensaje a mostrar en la escena siguiente
                textoMensaje = "Nivel Básico";
                nombreObjetoTextoBienvenida = "TextoBienvenida"; // Ajusta según tu UI en la escena destino
                duracionMensaje = 3f;
                mostrarMensaje = true;

                // Se suscribe al evento que se dispara al cargar la escena para mostrar el mensaje
                SceneManager.sceneLoaded += OnSceneLoaded;

                // Cambia la escena a la especificada
                SceneManager.LoadScene(nombreEscena);
            }
        }
    }

    // Método estático que se ejecuta al cargar una nueva escena
    private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (mostrarMensaje)
        {
            // Busca el objeto de texto para mostrar la bienvenida
            GameObject objTexto = GameObject.Find(nombreObjetoTextoBienvenida);
            if (objTexto != null)
            {
                var textoTMP = objTexto.GetComponent<TextMeshProUGUI>();
                if (textoTMP != null)
                {
                    // Activa el texto y asigna el mensaje
                    textoTMP.gameObject.SetActive(true);
                    textoTMP.text = textoMensaje;

                    // Para arrancar una corrutina, se necesita un MonoBehaviour
                    MonoBehaviour coroutineRunner = objTexto.GetComponent<MonoBehaviour>();
                    if (coroutineRunner == null)
                    {
                        coroutineRunner = objTexto.AddComponent<DummyMonoBehaviour>();
                    }
                    // Inicia la corrutina que desactivará el texto después de la duración establecida
                    coroutineRunner.StartCoroutine(DesactivarTexto(objTexto, duracionMensaje));
                }
            }

            // Restablece la bandera y se desuscribe del evento para evitar llamadas futuras
            mostrarMensaje = false;
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    // Corrutina que desactiva el objeto de texto luego de un tiempo de espera
    private static IEnumerator DesactivarTexto(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        obj.SetActive(false);
    }

    // Clase auxiliar para poder arrancar corrutinas desde objetos sin MonoBehaviour
    private class DummyMonoBehaviour : MonoBehaviour { }
}
