using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    //Collision Tracker
    private bool hasCollided = false;
    //Material Change
    private Renderer starRenderer; // Reference to the renderer component
    private Color startColor;
    public Material[] materials; // Array of materials for color cycling
    public float changeInterval = 1.0f; // Interval between material changes
    //Particle System Effects
    public GameObject groundCollisionParticles; // Reference to the ground collision particle system prefab
    public GameObject secondStar; //Instantiate another star
    //Particle Second Star Cooldown
    private bool auraCooldown = false; // Flag to track cooldown for aura collision
    public float auraCooldownDuration = 2.0f; // Duration of the cooldown period for aura collision

    // Check for collision
    private void Start()
    {
        StartCoroutine(DestroyIfNoCollision());
        starRenderer = GetComponent<Renderer>(); // Get the renderer component
        startColor = starRenderer.material.color; // Get the initial color

        // Start coroutine to change materials
        StartCoroutine(ChangeMaterials());
    }

    private void OnTriggerEnter(Collider other)
    {
        // Destroy on Collision with black hole
        if (other.CompareTag("BlackHole"))
        {
            hasCollided = true;
            DestroyStar();
        }
        //Effect on Collision with Aura
        if (other.CompareTag("Aura") && !auraCooldown && other.gameObject.GetInstanceID() != gameObject.GetInstanceID())
        {
            //InstantiateSecondStar();
        }

    }

    // Instantiate the second star and start cooldown
    private void InstantiateSecondStar()
    {
        Instantiate(secondStar, transform.position, Quaternion.identity);
        StartCoroutine(AuraCooldown());
    }

    // Cooldown for aura collision
    private IEnumerator AuraCooldown()
    {
        auraCooldown = true;
        yield return new WaitForSeconds(auraCooldownDuration);
        auraCooldown = false;
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
    //Destroy function
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
        }
    }
}