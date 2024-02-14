using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    private bool hasCollided = false;
    private Renderer starRenderer; // Reference to the renderer component
    private Color startColor;
    public Material[] materials; // Array of materials for color cycling
    public float changeInterval = 1.0f; // Interval between material changes
    public GameObject groundCollisionParticles; // Reference to the ground collision particle system prefab

    // Check for collision
    private void Start()
    {
        StartCoroutine(DestroyIfNoCollision());
        starRenderer = GetComponent<Renderer>(); // Get the renderer component
        startColor = starRenderer.material.color; // Get the initial color

        // Start coroutine to change materials
        StartCoroutine(ChangeMaterials());
    }

    // Destroy on Collision with black hole
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BlackHole"))
        {
            hasCollided = true;
            DestroyStar();
        }
    }

    // Destroy if no collision after Delay
    private IEnumerator DestroyIfNoCollision()
    {
        yield return new WaitForSeconds(7f); // Wait for 7 seconds

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

    // Coroutine to change materials
    private IEnumerator ChangeMaterials()
    {
        while (true)
        {
            // Change material/color
            if (materials.Length > 0)
            {
                int randomIndex = Random.Range(0, materials.Length);
                starRenderer.material = materials[randomIndex];
            }

            yield return new WaitForSeconds(changeInterval); // Wait for changeInterval seconds
        }
    }

    // Detect collision with ground
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            // Create ground collision particles
            if (groundCollisionParticles != null)
            {
                Instantiate(groundCollisionParticles, transform.position, Quaternion.identity);
            }

            Destroy(gameObject); // Destroy the star object
        }
    }
}