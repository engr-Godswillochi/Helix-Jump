using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassCheck : MonoBehaviour
{
    private bool hasFallen = false;
    private bool hasScored = false;
    private bool triggered = false;

    // public AudioClip breakSound; // Assign the break sound effect here

    public void BreakLevel()
    {
        if (hasFallen) return;
        hasFallen = true;

        // Play break sound at center of the level
        // if (breakSound != null)
        // {
        //     AudioSource.PlayClipAtPoint(breakSound, transform.position);
        // }

        transform.SetParent(null);

        foreach (Transform part in transform)
        {
            part.gameObject.SetActive(true);

            // Remove mesh collider
            MeshCollider meshCol = part.GetComponent<MeshCollider>();
            if (meshCol)
            {
                Destroy(meshCol);
            }

            // Add box collider instead
            BoxCollider boxCol = part.gameObject.AddComponent<BoxCollider>();
            boxCol.size *= 0.9f;

            // Add rigidbody
            Rigidbody rb = part.gameObject.AddComponent<Rigidbody>();
            rb.mass = 0.5f;
            rb.useGravity = true;

            Vector3 force = (part.position - transform.position).normalized;
            rb.AddForce(force * Random.Range(1.5f, 3f), ForceMode.Impulse);

            // Remove the collider after a short delay so it doesn't trigger anything
            Destroy(boxCol, 0.1f);
        }

        Destroy(gameObject, .7f);
    }

    void OnTriggerEnter(Collider other)
    {
        if (triggered) return;
        BallController ball = other.GetComponent<BallController>();
        if (!ball) return;
        triggered = true;
        BreakLevel();
        // Only add score once per level
        if (!hasScored)
        {
            VibrationManager.VibrateLight();
            hasScored = true;
            GameManager.singleton.AddScore(2);
            FindObjectOfType<BallController>().perfectPass++;
            Debug.Log("Increased Perfect Pass");
        }

    }
}