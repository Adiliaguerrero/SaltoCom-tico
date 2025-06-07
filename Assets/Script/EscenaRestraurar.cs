using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

    /// <summary>
    /// Permite recargar una escena actual o cambiar a otra escena específica mediante botones.
    /// </summary>
    /// <remarks>
    /// Usa <see cref="SceneManager"/> para cambiar entre escenas y <see cref="AudioManager"/> para reproducir efectos de sonido al hacer clic.
    /// </remarks>
    /// <example>
    /// Este script puede estar vinculado a botones de UI:
    /// <code>
    /// public EscenaRestraurar escenaController;
    /// escenaController.CargarEscena1();
    /// </code>
    /// </example>
    /// <seealso cref="SceneManager"/>
    /// <seealso cref="AudioManager"/>
public class EscenaRestraurar : MonoBehaviour
{
   
        /// <summary>
        /// Nombre de la primera escena a cargar cuando se pulsa el primer botón.
        /// </summary>
        /// <value>
        /// Nombre exacto de la escena tal como aparece en el Build Settings.
        /// </value>
    public string escena1;

        
        /// <summary>
        /// Nombre de la segunda escena, normalmente el menú principal.
        /// </summary>
        /// <value>
        /// Debe coincidir con el nombre definido en el sistema de escenas.
        /// </value>
    public string escena2;

        /// <summary>
        /// Sonido que se reproducirá al pulsar un botón de cambio de escena.
        /// </summary>
        /// <remarks>
        /// Debe ser asignado en el Inspector
        /// </remarks>
    public AudioClip sonidoBoton;

        /// <summary>
        /// Carga la escena almacenada en <see cref="escena1"/> y reproduce el sonido de botón.
        /// </summary>
        /// <exception cref="System.ArgumentException">Se lanza si el nombre de la escena está vacío o nulo.
        /// </exception>
        
    public void CargarEscena1()
    {
        // Reproduce el sonido usando el sistema de AudioManager global
        AudioManager.instancia.ReproducirSonido(sonidoBoton);

        // Carga la escena cuyo nombre está escrito en "escena1"
        SceneManager.LoadScene(escena1);
    }

        /// <summary>
        /// Carga la escena almacenada en <see cref="escena2"/> y reproduce el sonido de botón.
        /// </summary>
        /// <exception cref="System.ArgumentException">Se lanza si el nombre de la escena está vacío o nulo.
        /// </exception>
    public void CargarEscena2()
    {
        // Reproduce el sonido usando el sistema de AudioManager global
        AudioManager.instancia.ReproducirSonido(sonidoBoton);

        // Carga la escena cuyo nombre está escrito en "escena2"
        SceneManager.LoadScene(escena2);
    }
}
