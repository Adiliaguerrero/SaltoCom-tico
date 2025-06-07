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

   
        /// <summary>
        /// Nombre de la escena destino, asignado desde el Inspector.
        /// </summary>
    public string nombreEscena = "Escena";

    private static string textoMensaje = "¡Nivel Basico!";
    private static string nombreObjetoTextoBienvenida = "TextoBienvenida";     private static float duracionMensaje = 3f; 
    private static bool mostrarMensaje = false; 


    /// <summary>
    /// Detecta colisiones físicas con el trigger.
    /// </summary>
    /// <param name="collision">Datos de la colisión.
    /// </param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (haColisionado) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            haColisionado = true;

            if (joystickUI != null)
            {
                FixedJoystick joystick = joystickUI.GetComponent<FixedJoystick>();
                if (joystick != null)
                {
                    joystick.OnPointerUp(null); 
                    joystick.enabled = false;
                }
                joystickUI.SetActive(false);
            }

            rbJugador = collision.gameObject.GetComponent<Rigidbody2D>();
            if (rbJugador != null)
            {
                rbJugador.velocity = Vector2.zero;
                rbJugador.bodyType = RigidbodyType2D.Static;
            }

            playerController = collision.gameObject.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.puedeMoverse = false;
            }

            if (portal != null)
            {
                portal.SetActive(true);
                PortalTrigger trigger = portal.GetComponent<PortalTrigger>();
                if (trigger == null)
                {
                    trigger = portal.AddComponent<PortalTrigger>();
                }
                trigger.nombreEscena = nombreEscena;
            }

            StartCoroutine(EsperarYMostrarDialogo());
        }
    }

        /// <summary>
        /// Corrutina que espera unos segundos antes de mostrar el diálogo o continuar.
        /// </summary>
    private IEnumerator EsperarYMostrarDialogo()
    {
        yield return new WaitForSeconds(3f);

        DialogoController dialogo = FindObjectOfType<DialogoController>();
        if (dialogo != null)
        {
            dialogo.MostrarDialogo(() =>
            {
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

                Destroy(GetComponent<Collider2D>());
                Destroy(gameObject);
            });
        }
    }

        /// <summary>
        /// Clase interna que detecta la entrada al portal y realiza el cambio de escena.
        /// </summary>
    private class PortalTrigger : MonoBehaviour
    {
       
            /// <summary>
            /// Nombre de la escena destino.
            /// </summary>
        public string nombreEscena;


            /// <summary>
            /// Detecta si el jugador entra al área del portal.
            /// </summary>
            /// <param name="other">Collider del objeto entrante.
            /// </param>
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                textoMensaje = "Nivel Básico";
                nombreObjetoTextoBienvenida = "TextoBienvenida"; // Ajusta según tu UI en la escena destino
                duracionMensaje = 3f;
                mostrarMensaje = true;

                SceneManager.sceneLoaded += OnSceneLoaded;

                SceneManager.LoadScene(nombreEscena);
            }
        }
    }


    /// <summary>
    /// Método ejecutado al cargar una nueva escena.
    /// Busca y muestra el mensaje de bienvenida si está activado.
    /// </summary>
    /// <param name="scene">Escena cargada.
    /// </param>
    /// <param name="mode">Modo de carga.
    /// </param>
    private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (mostrarMensaje)
        {
            GameObject objTexto = GameObject.Find(nombreObjetoTextoBienvenida);
            if (objTexto != null)
            {
                var textoTMP = objTexto.GetComponent<TextMeshProUGUI>();
                if (textoTMP != null)
                {
                    textoTMP.gameObject.SetActive(true);
                    textoTMP.text = textoMensaje;

                    MonoBehaviour coroutineRunner = objTexto.GetComponent<MonoBehaviour>();
                    if (coroutineRunner == null)
                    {
                        coroutineRunner = objTexto.AddComponent<DummyMonoBehaviour>();
                    }
                    coroutineRunner.StartCoroutine(DesactivarTexto(objTexto, duracionMensaje));
                }
            }

            mostrarMensaje = false;
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

        /// <summary>
        /// Corrutina que desactiva el texto tras cierto tiempo.
        /// </summary>
        /// <param name="obj">Objeto de texto a desactivar.</param>
        /// <param name="delay">Tiempo en segundos antes de desactivarlo.</param>
        /// <returns>
        /// Enumerator para la corrutina.
        /// </returns>
    private static IEnumerator DesactivarTexto(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        obj.SetActive(false);
    }


        /// <summary>
        /// Clase auxiliar para correr corrutinas desde objetos que no sean MonoBehaviour.
        /// </summary>
    private class DummyMonoBehaviour : MonoBehaviour { }
}
