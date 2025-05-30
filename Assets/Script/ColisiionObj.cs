using System.Collections;
using UnityEngine;

public class ColisionadorTrigger2D : MonoBehaviour
{
    private bool haColisionado = false;

    public GameObject portalPrefab;  
    public Transform puntoDeAparicionDelPortal;  

    public GameObject joystickUI;  

    private Rigidbody2D rbJugador;
    private PlayerController playerController;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (haColisionado) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            haColisionado = true;

           
            if (joystickUI != null)
            {
                FixedJoystick joystick = joystickUI.GetComponent<FixedJoystick>();
                if (joystick != null)
                {
                    joystick.OnPointerUp(null); 
                    joystick.enabled = false;   
                }
                joystickUI.SetActive(false);
            }

           
            rbJugador = collision.gameObject.GetComponent<Rigidbody2D>();
            if (rbJugador != null)
            {
                rbJugador.velocity = Vector2.zero;
                rbJugador.bodyType = RigidbodyType2D.Static;
            }

            
            playerController = collision.gameObject.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.puedeMoverse = false;
            }

           
            if (portalPrefab != null && puntoDeAparicionDelPortal != null)
            {
                Instantiate(portalPrefab, puntoDeAparicionDelPortal.position, Quaternion.identity);
            }

          
            StartCoroutine(EsperarYMostrarDialogo(collision.gameObject));
        }
    }

    private IEnumerator EsperarYMostrarDialogo(GameObject jugador)
    {
        yield return new WaitForSeconds(3f);

        DialogoController dialogo = FindObjectOfType<DialogoController>();
        if (dialogo != null)
        {
            dialogo.MostrarDialogo(() =>
            {
              
                if (joystickUI != null)
                {
                    joystickUI.SetActive(true);
                    FixedJoystick joystick = joystickUI.GetComponent<FixedJoystick>();
                    if (joystick != null)
                    {
                        joystick.enabled = true;
                    }
                }

               
                if (rbJugador != null)
                {
                    rbJugador.bodyType = RigidbodyType2D.Dynamic;
                }

                if (playerController != null)
                {
                    playerController.puedeMoverse = true;
                }

                Destroy(GetComponent<Collider2D>());
                Destroy(gameObject);
            });
        }
    }
}
