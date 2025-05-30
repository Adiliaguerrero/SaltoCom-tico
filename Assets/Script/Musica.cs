using UnityEngine;
using UnityEngine.UI;

public class BotonMusica : MonoBehaviour
{
    public GameObject musicaManagerPrefab;  
    private MusicaManager musicaManager;  
    public Button boton; 

    private void Awake()
    {
     
        if (MusicaManager.instancia == null && musicaManagerPrefab != null)
        {
            Instantiate(musicaManagerPrefab);  
        }

     
        musicaManager = MusicaManager.instancia;
    }

    private void Start()
    {
     
        if (boton == null)
        {
            boton = GetComponent<Button>();
        }

        boton.onClick.AddListener(OnClick);
    }

    public void OnClick()
    {
        if (musicaManager != null)
        {
         
            musicaManager.AlternarMusica();
        }
    }
}







