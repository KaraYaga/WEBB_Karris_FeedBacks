using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    // Collision Tracker
    private bool hasCollided = false;

    // Material Change
    private Renderer starRenderer; // Reference to the renderer component
    private Color startColor;
    public Material[] materials; // Array of materials for color cycling
    public float changeInterval = 1.0f; // Interval between material changes

    // Particle System Effects
    public GameObject groundCollisionParticles; // Reference to the ground collision particle system prefab
    public GameObject destructionParticles; // Reference to the destruction particle system prefab
    public GameObject secondStar; // Instantiate another star

    // Particle Second Star Cooldown
    private bool auraCooldown = false; // Flag to track cooldown for aura collision
    public float auraCooldownDuration = 2.0f; // Duration of the cooldown period for aura collision

    // Sound
    private AudioClip destructionSound; // Sound to play when destroyed
    private AudioSource audioSource; // Reference to the AudioSource component

    // Check for collision
    private void Start()
    {
        StartCoroutine(DestroyIfNoCollision());
        starRenderer = GetComponent<Renderer>(); // Get the renderer component
        startColor = starRenderer.material.color; // Get the initial color

        // Start coroutine to change materials
        StartCoroutine(ChangeMaterials());

        // Get the AudioSource component
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Destroy on Collision with black hole
        if (other.CompareTag("BlackHole"))
        {
            hasCollided = true;
            DestroyStar();
        }
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

    // Destroy function
    private void DestroyStar()
    {
        // Get the AudioSource component dynamically
        AudioSource audioSource = GetComponent<AudioSource>();

        // Log whether AudioSource component is found
        if (audioSource == null)
        {
            Debug.Log("AudioSource component not found on the star GameObject.");
        }
        else
        {
            Debug.Log("AudioSource component found on the star GameObject.");
        }

        // Play destruction sound
        if (destructionSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(destructionSound);
        }

        // Instantiate destruction particles
        if (destructionParticles != null)
        {
            Instantiate(destructionParticles, transform.position, Quaternion.identity);
        }

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