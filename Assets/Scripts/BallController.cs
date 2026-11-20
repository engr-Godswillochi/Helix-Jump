using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BallController : MonoBehaviour
{
    private bool ignoreNextCollision;
    private Vector3 startPos;
    private float lastY;
    private PassCheck lastBrokenLevel;
    private TrailRenderer fireTrail;
    public int superSpeedBreakCount = 0;



    public int maxSuperSpeedBreaks = 1;
    public Rigidbody rb;
    public float impulseForce = 5f;
    public int perfectPass = 0;
    public bool isSuperSpeedActive;

    [Header("Particle Effects")]
    public GameObject bounceSplashPrefab; // Assign the particle prefab here

    [Header("Audio")]
    public AudioClip bounceSound; // Assign the bounce sound effect here
    public float bounceVolume = 0.5f; // Adjust bounce sound volume (0-1)

    void Awake()
    {
        startPos = transform.position;
        rb.isKinematic = true;
        lastY = transform.position.y;

        fireTrail = GetComponent<TrailRenderer>();
        if (fireTrail != null)
        {
            fireTrail.enabled = false;
        }

    }

    public void ActivateBall()
    {
        rb.isKinematic = false;
    }


    void OnCollisionEnter(Collision collision)
    {
        if (ignoreNextCollision) return;
        Debug.Log("Ball");

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

        if (isSuperSpeedActive)
        {
            if (!collision.transform.GetComponent<Goal>())
            {
                // Break the level before destroying it
                PassCheck level = collision.transform.GetComponentInParent<PassCheck>();
                if (level && level != lastBrokenLevel)
                {
                    level.BreakLevel();
                    superSpeedBreakCount++;
                    lastBrokenLevel = level;
                }
                else
                {
                    // If no PassCheck found, just destroy normally
                    Destroy(collision.transform.parent.gameObject, 0.3f);
                }
                Debug.Log("Destroying Platform");
            }
        }
        else
        {
            // Adding reset level functionality via deathpart - initialized when death part is hit
            DeathPart deathPart = collision.transform.GetComponent<DeathPart>();
            if (deathPart)
            {
                deathPart.HitDeathPart();
            }
        }


        rb.velocity = Vector3.zero;
        rb.AddForce(Vector3.up * impulseForce, ForceMode.Impulse);

        // Play bounce sound at collision point
        if (bounceSound != null && collision.contacts.Length > 0)
        {
            AudioSource.PlayClipAtPoint(bounceSound, collision.contacts[0].point, bounceVolume);
        }

        ignoreNextCollision = true;
        Invoke("AllowCollision", .2f);

        perfectPass = 0;
        if (superSpeedBreakCount >= maxSuperSpeedBreaks)
        {
            isSuperSpeedActive = false;
            superSpeedBreakCount = 0;
            if (fireTrail != null)
                fireTrail.enabled = false;
        }
    }

    private void AllowCollision()
    {
        ignoreNextCollision = false;
    }

    public void RestartBall()
    {
        transform.position = startPos;
        lastBrokenLevel = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (perfectPass >= 3 && !isSuperSpeedActive)
        {
            isSuperSpeedActive = true;
            superSpeedBreakCount = 0;

            if (fireTrail != null)
                fireTrail.enabled = true;
            rb.AddForce(Vector3.down * 10, ForceMode.Impulse);
        }
        lastY = transform.position.y;
    }
}