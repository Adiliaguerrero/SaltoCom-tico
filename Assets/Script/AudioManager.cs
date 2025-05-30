using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private bool sonidosActivos = true;
    public static AudioManager instancia;

    private AudioSource audioSource;

    private void Awake()
    {
        if (instancia == null)
        {
            instancia = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        audioSource = gameObject.AddComponent<AudioSource>();
        CargarEstadoSonido();
    }

    private void CargarEstadoSonido()
    {
        if (PlayerPrefs.HasKey("SonidosActivos"))
        {
            sonidosActivos = PlayerPrefs.GetInt("SonidosActivos") == 1;
        }
        else
        {
            sonidosActivos = true;
        }
    }

    public void CambiarEstadoSonido(bool estado)
    {
        sonidosActivos = estado;
        PlayerPrefs.SetInt("SonidosActivos", sonidosActivos ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void ReproducirSonido(AudioClip clip)
    {
        if (sonidosActivos && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    public bool ObtenerEstadoSonido()
    {
        return sonidosActivos;
    }
}


