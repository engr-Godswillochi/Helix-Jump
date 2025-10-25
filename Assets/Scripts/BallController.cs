using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BallController : MonoBehaviour
{
    private bool ignoreNextCollision;
    private Vector3 startPos;

    public Rigidbody rb;
    public float impulseForce = 5f;
    public int perfectPass = 0;
    public bool isSuperSpeedActive;
    // Start is called before the first frame update
    void Awake()
    {
        startPos = transform.position;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (ignoreNextCollision) return;
        Debug.Log("Ball ");

        if (isSuperSpeedActive)
        {
            if (!collision.transform.GetComponent<Goal>())
            {
                Destroy(collision.transform.parent.gameObject, 0.3f);
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

        ignoreNextCollision = true;
        Invoke("AllowCollision", .2f);

        perfectPass = 0;
        isSuperSpeedActive = false;
    }

    private void AllowCollision()
    {
        ignoreNextCollision = false;
    }
    
    public void RestartBall()
    {
        transform.position = startPos;
    }

    // Update is called once per frame
    void Update()
    {
        if (perfectPass >= 3 && !isSuperSpeedActive)
        {
            isSuperSpeedActive = true;
            rb.AddForce(Vector3.down * 10, ForceMode.Impulse);
        }
    }
}
