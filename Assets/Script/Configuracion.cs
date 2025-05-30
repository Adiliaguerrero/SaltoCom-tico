using UnityEngine;
using UnityEngine.UI;

public class ConfiguracionUI : MonoBehaviour
{
    
    public GameObject panelConfiguracion;
    public Button botonAbrir;
    public Button botonCerrar;

    
    public GameObject panelCreditos;
    public Button botonCreditosAbrir;
    public Button botonCreditosCerrar;

    
    public AudioClip sonidoBoton; 

    void Start()
    {
        // Inicialización
        panelConfiguracion.SetActive(false);
        panelCreditos.SetActive(false);

        botonAbrir.onClick.AddListener(AbrirConfiguracion);
        botonCerrar.onClick.AddListener(CerrarConfiguracion);

        botonCreditosAbrir.onClick.AddListener(AbrirCreditos);
        botonCreditosCerrar.onClick.AddListener(CerrarCreditos);
    }

    void AbrirConfiguracion()
    {
        AudioManager.instancia.ReproducirSonido(sonidoBoton);
        panelConfiguracion.SetActive(true);
        Time.timeScale = 0f;  
    }

    void CerrarConfiguracion()
    {
        AudioManager.instancia.ReproducirSonido(sonidoBoton);
        panelConfiguracion.SetActive(false);
        Time.timeScale = 1f;  
    }

    void AbrirCreditos()
    {
        AudioManager.instancia.ReproducirSonido(sonidoBoton);
        panelCreditos.SetActive(true);
    }

    void CerrarCreditos()
    {
        AudioManager.instancia.ReproducirSonido(sonidoBoton);
        panelCreditos.SetActive(false);
    }
}


