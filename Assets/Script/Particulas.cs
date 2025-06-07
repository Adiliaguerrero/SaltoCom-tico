using UnityEngine;

    /// <summary>
    /// Elimina visualmente las partículas de un <see cref="ParticleSystem"/> cuando colisionan con un objeto etiquetado como "Suelo".
    /// </summary>
    /// <remarks>
    /// Utiliza el evento <c>OnParticleCollision</c> para detectar colisiones y forzar la expiración de las partículas activas.
    /// Requiere que el <c>GameObject</c> tenga un componente <c>ParticleSystem</c> y colisionadores configurados.
    /// </remarks>
public class DestruirParticulasAlColisionar : MonoBehaviour
{
    /// <summary>
    /// Referencia al componente <see cref="ParticleSystem"/> asociado a este objeto.
    /// </summary>
    private ParticleSystem ps;

    void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Suelo"))
        {
            ParticleSystem.Particle[] particles = new ParticleSystem.Particle[ps.main.maxParticles];

            int count = ps.GetParticles(particles);

            for (int i = 0; i < count; i++)
            {
                if (particles[i].remainingLifetime > 0)
                {
                    particles[i].remainingLifetime = 0;
                }
            }

            ps.SetParticles(particles, count);
        }
    }
}

