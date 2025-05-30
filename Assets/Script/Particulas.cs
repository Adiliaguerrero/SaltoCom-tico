using UnityEngine;

public class DestruirParticulasAlColisionar : MonoBehaviour
{
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
