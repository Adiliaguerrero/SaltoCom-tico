using UnityEngine;
using UnityEngine.UI;

public class BotonClipControl : MonoBehaviour
{
    public Button botonSonido;
    public AudioClip sonidoBoton;

    private void Awake()
    {
        if (AudioManager.instancia == null)
        {
            GameObject audioManagerObj = new GameObject("AudioManager");
            audioManagerObj.AddComponent<AudioManager>();
            DontDestroyOnLoad(audioManagerObj);
        }
    }

    private void Start()
    {
        if (botonSonido != null)
            botonSonido.onClick.AddListener(ToggleSonido);
        else
            Debug.LogWarning("Botón de sonido no asignado.");
    }

    private void ToggleSonido()
    {
        bool sonidoActivo = AudioManager.instancia.ObtenerEstadoSonido();

        AudioManager.instancia.CambiarEstadoSonido(!sonidoActivo);

        if (sonidoBoton != null)
        {
            AudioManager.instancia.ReproducirSonido(sonidoBoton);
        }
    }
}

