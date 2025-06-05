using UnityEngine;

// Esta clase permite eliminar las partículas de un ParticleSystem cuando colisionan con un objeto etiquetado como "Suelo"
public class DestruirParticulasAlColisionar : MonoBehaviour
{
    // Referencia al componente ParticleSystem del objeto
    private ParticleSystem ps;

    // Se llama al iniciar el script
    void Start()
    {
        // Obtiene y guarda el ParticleSystem adjunto a este GameObject
        ps = GetComponent<ParticleSystem>();
    }

    // Metodo OnParticleCollision Se llama automáticamente cuando una partícula del ParticleSystem colisiona con otro objeto
    void OnParticleCollision(GameObject other)
    {
        // Verifica si el objeto con el que colisiona tiene la etiqueta "Suelo"
        if (other.CompareTag("Suelo"))
        {
            // Crea un arreglo para almacenar todas las partículas activas actualmente
            ParticleSystem.Particle[] particles = new ParticleSystem.Particle[ps.main.maxParticles];

            // Obtiene todas las partículas activas y devuelve su cantidad en 'count'
            int count = ps.GetParticles(particles);

            // Recorre todas las partículas para modificar su tiempo de vida restante
            for (int i = 0; i < count; i++)
            {
                // Si la partícula todavía está viva (no ha terminado su vida)
                if (particles[i].remainingLifetime > 0)
                {
                    // Fuerza a que la partícula expire inmediatamente, lo que la eliminará visualmente
                    particles[i].remainingLifetime = 0;
                }
            }

            // Aplica los cambios hechos al arreglo de partículas de vuelta al ParticleSystem
            ps.SetParticles(particles, count);
        }
    }
}

