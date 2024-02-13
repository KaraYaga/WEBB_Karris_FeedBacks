using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    private bool hasCollided = false;

    //Check for collision
    private void Start()
    {
        StartCoroutine(DestroyIfNoCollision());
    }

    //Destroy on Collision
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BlackHole"))
        {
            hasCollided = true;
            DestroyStar();
        }
    }

    //Destroy if no collision after Delay
    private IEnumerator DestroyIfNoCollision()
    {
        yield return new WaitForSeconds(2f); // Wait for 3 seconds

        // Check if there has been no collision
        if (!hasCollided)
        {
            DestroyStar();
        }
    }

    private void DestroyStar()
    {
        Destroy(gameObject);
    }
}