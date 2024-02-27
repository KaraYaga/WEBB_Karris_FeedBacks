using System.Collections;
using UnityEngine;

public class Throwing : MonoBehaviour
{
    [Header("References")]
    public Transform cam;
    public Transform attackPoint;
    public GameObject objectToThrow;
    public ParticleSystem auraParticleSystem; // Reference to the aura particle system

    [Header("Throwing")]
    public KeyCode throwkey = KeyCode.Mouse0;
    public float throwForce;
    public float throwUpwardForce;
    public float throwCooldown;
    public float doubleShotDuration = 5f; // Duration for double shot when colliding with aura
    public int numberOfProjectiles = 1; // Number of projectiles to throw when not in double shot state

    private bool readyToThrow = true;
    private bool isDoubleShotActive = false;

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(throwkey) && readyToThrow)
        {
            Throw();
        }
    }

    private void Throw()
    {
        readyToThrow = false;

        // Determine the number of projectiles to throw based on the current state
        int projectilesToThrow = isDoubleShotActive ? numberOfProjectiles * 2 : numberOfProjectiles;

        for (int i = 0; i < projectilesToThrow; i++)
        {
            // Instantiate object to throw
            GameObject projectile = Instantiate(objectToThrow, attackPoint.position, cam.rotation);

            // Add Rigidbody Component if not already attached
            Rigidbody projectileRB = projectile.GetComponent<Rigidbody>();
            if (projectileRB == null)
                projectileRB = projectile.AddComponent<Rigidbody>();

            // Calculate the direction slightly left or right based on the loop iteration
            Vector3 direction = cam.forward + (i % 2 == 0 ? -cam.right : cam.right);
            direction.Normalize(); // Normalize the direction vector

            // Add Force
            Vector3 forceToAdd = direction * throwForce + cam.up * throwUpwardForce;
            projectileRB.AddForce(forceToAdd, ForceMode.Impulse);
        }

        // Implement throwCooldown
        StartCoroutine(ResetThrow());
    }

    // Throw Cooldown
    private IEnumerator ResetThrow()
    {
        yield return new WaitForSeconds(throwCooldown);
        readyToThrow = true;
    }

    // Enable double shot state for a specific duration
    public void ActivateDoubleShot()
    {
        isDoubleShotActive = true;
        StartCoroutine(DisableDoubleShotAfterDuration());
    }

    // Disable double shot state after the specified duration
    private IEnumerator DisableDoubleShotAfterDuration()
    {
        yield return new WaitForSeconds(doubleShotDuration);
        isDoubleShotActive = false;
    }

    // Check for collision with aura particle system
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Aura"))
        {
            // Activate double shot state
            ActivateDoubleShot();
        }
    }
}