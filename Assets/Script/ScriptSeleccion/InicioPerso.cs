using UnityEngine;

public class AparicionPersonaje : MonoBehaviour
{
    public GameObject[] personajes; 
    public Transform puntoAparicion; 
    public Joystick joystick; 
    public GameObject camara; 

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

        // Instanciar el personaje en la posición de aparición
        GameObject personajeInstanciado = Instantiate(personajes[indicePersonaje], puntoAparicion.position, Quaternion.identity);
        
        // Asegurar que la escala sea correcta
        personajeInstanciado.transform.localScale = Vector3.one;

        // Verificar posición después de instanciar
        Debug.Log("Personaje instanciado en: " + personajeInstanciado.transform.position);

        // Verificar si el personaje tiene PlayerController
        PlayerController playerController = personajeInstanciado.GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.joystick = joystick;
        }
        else
        {
            Debug.LogWarning("El personaje instanciado no tiene un PlayerController.");
        }

        // Configurar la cámara para seguir al personaje
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
