using UnityEngine;

public class Paneles1 : MonoBehaviour
{
    public GameObject panel1;
    public GameObject panel2;
    public GameObject panel3;

    public GameObject joystick;
    public GameObject imagen1;
    public GameObject imagen2;

    private int triviaActual = 0;

    private Rigidbody2D rbJugador;
    private PlayerController playerController;

    void Start()
    {
        
        panel1.SetActive(false);
        panel2.SetActive(false);
        panel3.SetActive(false);

      
        joystick.SetActive(true);
        imagen1.SetActive(true);
        imagen2.SetActive(true);

        
        GameObject jugador = GameObject.FindGameObjectWithTag("Player");
        if (jugador != null)
        {
            rbJugador = jugador.GetComponent<Rigidbody2D>();
            playerController = jugador.GetComponent<PlayerController>();
        }
    }

  public void SiguienteTrivia()
{
    
    GameObject jugador = GameObject.FindGameObjectWithTag("Player");
    if (jugador != null)
    {
        rbJugador = jugador.GetComponent<Rigidbody2D>();
        playerController = jugador.GetComponent<PlayerController>();
    }

    if (triviaActual == 0)
        ActivarPanel(0);
    else if (triviaActual == 1)
        ActivarPanel(1);
    else if (triviaActual == 2)
        ActivarPanel(2);
    else
    {
        Debug.Log("Todas las trivias han terminado.");

        panel1.SetActive(false);
        panel2.SetActive(false);
        panel3.SetActive(false);

        
        joystick.SetActive(true);
        FixedJoystick joyComponent = joystick.GetComponent<FixedJoystick>();
        if (joyComponent != null)
        {
            joyComponent.enabled = true;
        }

        imagen1.SetActive(true);
        imagen2.SetActive(true);

     
        if (rbJugador != null)
            rbJugador.bodyType = RigidbodyType2D.Dynamic;

        if (playerController != null)
            playerController.puedeMoverse = true;

        return;
    }

    triviaActual++;
}


    private void ActivarPanel(int index)
    {
        panel1.SetActive(index == 0);
        panel2.SetActive(index == 1);
        panel3.SetActive(index == 2);

        bool panelActivo = panel1.activeSelf || panel2.activeSelf || panel3.activeSelf;

        joystick.SetActive(!panelActivo);
        imagen1.SetActive(!panelActivo);
        imagen2.SetActive(!panelActivo);

      
        if (rbJugador != null)
            rbJugador.bodyType = panelActivo ? RigidbodyType2D.Static : RigidbodyType2D.Dynamic;

        if (playerController != null)
            playerController.puedeMoverse = !panelActivo;
    }
}
