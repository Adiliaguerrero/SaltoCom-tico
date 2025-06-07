using UnityEngine;

// Esta clase gestiona la aparición o instanciacion del personaje elegido al iniciar la escena

    /// <summary>
    /// Gestiona la aparición del personaje seleccionado al iniciar la escena.
    /// </summary>
    /// <remarks>
    /// Instancia el prefab correspondiente desde un arreglo de personajes, asigna joystick y configura la cámara para seguir al jugador.
    /// </remarks>
    /// <example>
    /// Asigna este script a un GameObject vacío y configura los prefabs de personajes, punto de aparición, joystick y cámara desde el Inspector.
    /// </example>
public class AparicionPersonaje : MonoBehaviour
{

        /// <summary>
        /// Arreglo de prefabs disponibles para el jugador.
        /// </summary>
        /// <value>
        /// Cada elemento del arreglo representa un personaje seleccionable.
        /// </value>
    public GameObject[] personajes;


        /// <summary>
        /// Posición donde aparecerá el personaje instanciado.
        /// </summary>
    public Transform puntoAparicion;

        /// <summary>
        /// Referencia al joystick usado para controlar al personaje.
        /// </summary>
    public Joystick joystick;


        /// <summary>
        /// Objeto de cámara que seguirá al personaje instanciado.
        /// </summary>
    public GameObject camara;

    // Método Start que se ejecuta automáticamente al iniciar la escena
    void Start()
    {
        // Validación: Verifica si el arreglo de personajes está vacío o no fue asignado
        if (personajes == null || personajes.Length == 0)
        {
            Debug.LogError("El array de personajes está vacío o no está asignado en el Inspector.");
            return;
        }

        // Validación: Verifica si se asignó el punto de aparición
        if (puntoAparicion == null)
        {
            Debug.LogError("El punto de aparición no está asignado en el Inspector.");
            return;
        }

        // Obtiene el índice del personaje guardado con PlayerPrefs, por defecto 0 si no existe
        int indicePersonaje = PlayerPrefs.GetInt("PersonajeSeleccionado", 0);
        Debug.Log("Índice recuperado de PlayerPrefs: " + indicePersonaje);

        // Validación: Verifica que el índice esté dentro de los límites del arreglo
        if (indicePersonaje < 0 || indicePersonaje >= personajes.Length)
        {
            Debug.LogError("Índice del personaje fuera de rango: " + indicePersonaje);
            return;
        }

        // Instancia el personaje seleccionado en la posición y rotación definidas
        GameObject personajeInstanciado = Instantiate(personajes[indicePersonaje], puntoAparicion.position, Quaternion.identity);

        // Asegura que el personaje tenga una escala normal (evita deformaciones)
        personajeInstanciado.transform.localScale = Vector3.one;

        Debug.Log("Personaje instanciado en: " + personajeInstanciado.transform.position);

        // Asigna el joystick al personaje si tiene el componente PlayerController
        PlayerController playerController = personajeInstanciado.GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.joystick = joystick;
        }
        else
        {
            Debug.LogWarning("El personaje instanciado no tiene un PlayerController.");
        }

        // Configura la cámara para que siga al personaje instanciado en todo el juego
        if (camara != null)
        {
            Camara camaraScript = camara.GetComponent<Camara>();
            if (camaraScript != null)
            {
                camaraScript.Player2 = personajeInstanciado;
                Debug.Log("La cámara sigue al personaje correctamente.");
            }
            else
            {
                Debug.LogError("No se encontró el script de la cámara en el objeto asignado.");
            }
        }
        else
        {
            Debug.LogError("La referencia de la cámara no está asignada en el Inspector.");
        }
    }
}
