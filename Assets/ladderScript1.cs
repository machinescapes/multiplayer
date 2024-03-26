using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder1 : MonoBehaviour
{
    public Rigidbody rb;
    bool inside = false;
    public float climbSpeed = 3.2f;

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            rb = col.gameObject.GetComponent<Rigidbody>();
            inside = true;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            rb = null;
            inside = false;
        }
    }

    void FixedUpdate()
    {
        if (inside && rb != null)
        {
            // Calculate climb direction based on the ladder's up vector
            Vector3 climbDirection = transform.up;

            // Apply climb velocity based on the climb direction and speed
            Vector3 climbVelocity = climbDirection * climbSpeed;
            rb.velocity = climbVelocity;
        }
    }
}
