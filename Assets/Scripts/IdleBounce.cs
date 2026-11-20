using UnityEngine;

public class IdleBouncePhysics : MonoBehaviour
{
    public float bounceForce = 3f;
    public float minYVelocity = 0.5f;

    [Header("Particle Effects")]
    public GameObject bounceSplashPrefab; // Assign the particle prefab here
    

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision collision)
    {
        // Only bounce on helix
        if (collision.gameObject.CompareTag("Helix"))
        {
            rb.velocity = Vector3.zero;
            rb.AddForce(Vector3.up * bounceForce, ForceMode.Impulse);
        }

        // Spawn water splash effect at collision point
        if (bounceSplashPrefab != null && collision.contacts.Length > 0)
        {
            Vector3 collisionPoint = collision.contacts[0].point;
            Vector3 collisionNormal = collision.contacts[0].normal;
            
            GameObject splash = Instantiate(bounceSplashPrefab, collisionPoint, Quaternion.LookRotation(collisionNormal));
            
            // Get the particle system
            ParticleSystem ps = splash.GetComponent<ParticleSystem>();
            
            if (ps != null)
            {
                // Match splash color to BALL color
                Renderer ballRenderer = GetComponent<Renderer>();
                if (ballRenderer != null)
                {
                    ParticleSystem.MainModule mainModule = ps.main;
                    mainModule.startColor = ballRenderer.material.color;
                }
                
                // FORCE the particle system to play
                ps.Clear();
                ps.Play();
                
                Debug.Log("Splash effect spawned and playing!");
            }
            
            Destroy(splash, 2f);
        }
    }
}
