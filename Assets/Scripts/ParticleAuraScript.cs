using System.Collections;
using UnityEngine;

public class ParticleAuraScript : MonoBehaviour
{
    public AudioClip auraSound; // Sound to play while the aura exists
    private AudioSource audioSource; // Reference to the AudioSource component

    private void Start()
    {
        // Get the AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            // If AudioSource component is not found, add one
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Assign the aura sound clip
        if (auraSound != null)
        {
            audioSource.clip = auraSound;
            // Set the volume to 60%
            audioSource.volume = 0.4f;
            // Play the sound on a loop
            audioSource.loop = true;
            audioSource.Play();
        }

        StartCoroutine(DestroyAfterTime());
    }

    // Destroy after a certain amount of time
    private IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(3f); // Wait for 3 seconds
        DestroyAura();
    }

    // Destroy function
    private void DestroyAura()
    {
        // Stop playing the sound
        if (audioSource != null)
        {
            audioSource.Stop();
        }
        Destroy(gameObject);
    }
}