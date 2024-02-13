using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Check if the colliding GameObject has the "Star" tag
        if (other.CompareTag("Star"))
        {
            Destroy(); // Destroy the BlackHole object
        }
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
