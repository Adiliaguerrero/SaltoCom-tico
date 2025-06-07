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

    private static string textoNombreObj;
    private static string textoMensaje;
    private static float textoDuracion;
    private static bool mostrarBienvenida = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            textoNombreObj = nombreObjetoTextoBienvenida;
            textoMensaje = mensajeBienvenida;
            textoDuracion = duracionMensaje;
            mostrarBienvenida = true;

            DontDestroyOnLoad(this.gameObject);

            SceneManager.sceneLoaded += OnSceneLoaded;

            SceneManager.LoadScene(nombreEscenaDestino);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (mostrarBienvenida)
        {
            GameObject obj = GameObject.Find(textoNombreObj);

            if (obj != null)
            {
                TextMeshProUGUI texto = obj.GetComponent<TextMeshProUGUI>();
                if (texto != null)
                {
                    texto.gameObject.SetActive(true);
                    texto.text = textoMensaje;

                    obj.GetComponent<MonoBehaviour>()?.StartCoroutine(DesactivarTexto(obj, textoDuracion));
                }
            }

            mostrarBienvenida = false;

            SceneManager.sceneLoaded -= OnSceneLoaded;

            Destroy(this.gameObject);
        }
    }

    private System.Collections.IEnumerator DesactivarTexto(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);

        obj.SetActive(false);
    }
}
