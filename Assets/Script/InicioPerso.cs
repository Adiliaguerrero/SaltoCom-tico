using UnityEngine;


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

    
        /// <summary>
        /// Instancia el personaje seleccionado y configura sus dependencias.
        /// </summary>
        /// <remarks>
        /// Recupera el índice del personaje desde PlayerPrefs, instancia el personaje, asigna joystick y configura la cámara.
        /// </remarks>
    void Start()
    {
        if (personajes == null || personajes.Length == 0)
        {
            Debug.LogError("El array de personajes está vacío o no está asignado en el Inspector.");
            return;
        }

        if (puntoAparicion == null)
        {
            Debug.LogError("El punto de aparición no está asignado en el Inspector.");
            return;
        }

        int indicePersonaje = PlayerPrefs.GetInt("PersonajeSeleccionado", 0);
        Debug.Log("Índice recuperado de PlayerPrefs: " + indicePersonaje);

        if (indicePersonaje < 0 || indicePersonaje >= personajes.Length)
        {
            Debug.LogError("Índice del personaje fuera de rango: " + indicePersonaje);
            return;
        }

        GameObject personajeInstanciado = Instantiate(personajes[indicePersonaje], puntoAparicion.position, Quaternion.identity);

        personajeInstanciado.transform.localScale = Vector3.one;

        Debug.Log("Personaje instanciado en: " + personajeInstanciado.transform.position);

        PlayerController playerController = personajeInstanciado.GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.joystick = joystick;
        }
        else
        {
            Debug.LogWarning("El personaje instanciado no tiene un PlayerController.");
        }

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
