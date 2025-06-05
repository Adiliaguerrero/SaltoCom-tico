// Importamos los espacios de nombres necesarios para trabajar con escenas y texto en Unity
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

// Esta clase controla el cambio de escena cuando el jugador  entra en contacto con un portal,
// y muestra un mensaje emergente temporal al llegar a la nueva escena.
public class PortalCambioEscena : MonoBehaviour
{
    // Nombre de la escena a la que se cambiará al colisionar con el portal
    public string nombreEscenaDestino;

    // Nombre del objeto TextMeshPro que mostrará el mensaje de bienvenida en la nueva escena
    public string nombreObjetoTextoBienvenida;

    // Mensaje de bienvenida que se mostrará al llegar a la nueva escena
    public string mensajeBienvenida = "¡Bienvenido!";

    // Duración en segundos que el mensaje de bienvenida permanecerá visible
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
