using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

/// <summary>
/// Controla el cambio de escena al entrar en contacto con un portal.
/// Muestra un mensaje de bienvenida temporal en la nueva escena.
/// </summary>
public class PortalCambioEscena : MonoBehaviour
{
    /// <summary>
    /// Nombre de la escena de destino a cargar al colisionar con el portal.
    /// </summary>
    public string nombreEscenaDestino;

    /// <summary>
    /// Nombre del objeto que contiene el texto de bienvenida en la nueva escena.
    /// </summary>
    public string nombreObjetoTextoBienvenida;

    /// <summary>
    /// Mensaje de bienvenida que se mostrará al llegar a la nueva escena.
    /// </summary>
    public string mensajeBienvenida = "¡Bienvenido!";

    /// <summary>
    /// Duración (en segundos) del mensaje de bienvenida.
    /// </summary>
    public float duracionMensaje = 3f;

    // Variables estáticas para transferir información entre escenas (ya que los datos públicos no persisten automáticamente)
    private static string textoNombreObj;
    private static string textoMensaje;
    private static float textoDuracion;
    private static bool mostrarBienvenida = false;

    // Método llamado automáticamente por Unity cuando otro objeto colisiona con este (en 2D)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Verificamos que el objeto que colisionó tiene la etiqueta "Player"
        if (collision.collider.CompareTag("Player"))
        {
            // Guardamos los datos del mensaje de bienvenida en variables estáticas
            textoNombreObj = nombreObjetoTextoBienvenida;
            textoMensaje = mensajeBienvenida;
            textoDuracion = duracionMensaje;
            mostrarBienvenida = true;

            // Evitamos que este objeto se destruya al cargar la nueva escena
            DontDestroyOnLoad(this.gameObject);

            // Nos suscribimos al evento que se ejecuta cuando la nueva escena se ha cargado completamente
            SceneManager.sceneLoaded += OnSceneLoaded;

            // Cargamos la nueva escena especificada
            SceneManager.LoadScene(nombreEscenaDestino);
        }
    }

    // Método que se ejecuta automáticamente cuando se ha cargado una nueva escena
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Verificamos si hay un mensaje de bienvenida por mostrar
        if (mostrarBienvenida)
        {
            // Buscamos el objeto que debe mostrar el mensaje por su nombre
            GameObject obj = GameObject.Find(textoNombreObj);

            // Si encontramos el objeto
            if (obj != null)
            {
                // Intentamos obtener su componente de texto (TextMeshProUGUI)
                TextMeshProUGUI texto = obj.GetComponent<TextMeshProUGUI>();
                if (texto != null)
                {
                    // Activamos el objeto y mostramos el mensaje
                    texto.gameObject.SetActive(true);
                    texto.text = textoMensaje;

                    // Iniciamos una corrutina para ocultar el mensaje después de un tiempo
                    obj.GetComponent<MonoBehaviour>()?.StartCoroutine(DesactivarTexto(obj, textoDuracion));
                }
            }

            // Reiniciamos la bandera para no volver a mostrar el mensaje
            mostrarBienvenida = false;

            // Nos desuscribimos del evento para evitar ejecuciones innecesarias
            SceneManager.sceneLoaded -= OnSceneLoaded;

            // Destruimos este objeto porque ya no lo necesitamos
            Destroy(this.gameObject);
        }
    }

    // Corrutina que espera unos segundos antes de ocultar el objeto del mensaje de bienvenida
    private System.Collections.IEnumerator DesactivarTexto(GameObject obj, float delay)
    {
        // Espera el tiempo indicado
        yield return new WaitForSeconds(delay);

        // Oculta el objeto de texto (mensaje)
        obj.SetActive(false);
    }
}
