using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : MonoBehaviour
{
    public float shakeDuration = 2f; // Duration of the shake
    public float shakeMagnitude = 0.1f; // Magnitude of the shake
    public float shakeSpeed = 1.0f; // Speed of the shake
    public ParticleSystem destructionParticles; // Reference to the destruction particle system

    private bool isDestroyed = false;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the colliding GameObject has the "Star" tag and if the black hole hasn't been destroyed yet
        if (other.CompareTag("Star") && !isDestroyed)
        {
            isDestroyed = true;
            StartCoroutine(ShakeAndDestroy()); // Shake the BlackHole object and destroy it after shaking
        }
    }

    private IEnumerator ShakeAndDestroy()
    {
        // Shake the black hole
        float elapsed = 0.0f;
        Vector3 originalPos = transform.localPosition;
        while (elapsed < shakeDuration)
        {
            float x = originalPos.x + Random.Range(-1f, 1f) * shakeMagnitude;
            float y = originalPos.y + Random.Range(-1f, 1f) * shakeMagnitude;
            float z = originalPos.z + Random.Range(-1f, 1f) * shakeMagnitude;

            transform.localPosition = new Vector3(x, y, z);

            elapsed += Time.deltaTime * shakeSpeed;

            yield return null;
        }

        // Reset position
        transform.localPosition = originalPos;

        // Instantiate destruction particles immediately after shaking
        if (destructionParticles != null)
        {
            Instantiate(destructionParticles, transform.position, Quaternion.identity);
        }

        // Destroy the black hole
        Destroy(gameObject);
    }
}