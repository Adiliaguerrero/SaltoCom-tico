using UnityEngine;

public class MusicaManager : MonoBehaviour
{
    public static MusicaManager instancia; 
    public AudioSource musicaFondo;  
    private bool musicaActiva;  

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

       
        musicaActiva = PlayerPrefs.GetInt("MusicaActiva", 1) == 1;  
        AplicarEstadoMusica();
    }

  
    public void AlternarMusica()
    {
        musicaActiva = !musicaActiva;
        PlayerPrefs.SetInt("MusicaActiva", musicaActiva ? 1 : 0);  
        PlayerPrefs.Save();
        AplicarEstadoMusica();
    }

   
    private void AplicarEstadoMusica()
    {
        if (musicaFondo != null)
        {
            if (musicaActiva)
            {
                if (!musicaFondo.isPlaying) musicaFondo.Play();
                musicaFondo.volume = 1f; 
            }
            else
            {
                musicaFondo.volume = 0f; 
            }
        }
    }
}
