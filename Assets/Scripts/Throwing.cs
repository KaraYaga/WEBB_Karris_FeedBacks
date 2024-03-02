using System.Collections;
using UnityEngine;
using TMPro;

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
    public float doubleShotDuration = 5f; // Duration for double shot when colliding with aura
    public int numberOfProjectiles = 1; // Number of projectiles to throw when not in double shot state

    [Header("Double Shot")]
    [SerializeField] private TMP_Text doublesActivated; // Reference to the TextMeshPro text component
    private bool readyToThrow = true;
    private bool isDoubleShotActive = false;

    private AudioSource audioSource; // Reference to the AudioSource component
    public AudioClip throwSound; // Sound effect for throwing a star

    // Start is called before the first frame update
    void Start()
    {
        // Get the AudioSource component attached to the GameObject
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            // If AudioSource component is not found, add one
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

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
        // Play the throw sound effect
        if (throwSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(throwSound);
        }

        // Determine the number of projectiles to throw based on the current state
        int projectilesToThrow = isDoubleShotActive ? numberOfProjectiles * 2 : numberOfProjectiles;

        // Throw projectiles only if readyToThrow is true
        if (readyToThrow)
        {
            for (int i = 0; i < projectilesToThrow; i++)
            {
                // Instantiate object to throw
                GameObject projectile = Instantiate(objectToThrow, attackPoint.position, cam.rotation);

                // Add Rigidbody Component if not already attached
                Rigidbody projectileRB = projectile.GetComponent<Rigidbody>();
                if (projectileRB == null)
                    projectileRB = projectile.AddComponent<Rigidbody>();

                // Calculate the direction
                Vector3 direction;
                if (isDoubleShotActive)
                {
                    // For double shot, alternate between left and right
                    direction = cam.forward + (i % 2 == 0 ? -cam.right : cam.right);
                }
                else
                {
                    // For single shot, go straight
                    direction = cam.forward;
                }
                direction.Normalize(); // Normalize the direction vector

                // Add Force
                Vector3 forceToAdd = direction * throwForce + cam.up * throwUpwardForce;
                projectileRB.AddForce(forceToAdd, ForceMode.Impulse);
            }

            // Reset readyToThrow after throwing projectiles
            readyToThrow = true;
        }
    }

    //Check Aura to Activate Double Shot
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Aura"))
        {
            // Activate double shot state
            ActivateDoubleShot();
            DisplayPopup();
        }
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

    // Display TMP Pro popup
    private void DisplayPopup()
    {
        Debug.Log("Displaying popup...");

        // Check if the reference to doublesActivated is not null
        if (doublesActivated != null)
        {
            // Set the popup text
            doublesActivated.text = "Double Star Shot!";
            doublesActivated.gameObject.SetActive(true);

            // Start coroutine to shake the popup
            StartCoroutine(ShakePopupEffect(6, 3, 3));
            // Start coroutine to hide the popup after a delay
            StartCoroutine(HidePopupAfterDelay(2f)); // Hide the popup after 2 seconds (adjust as needed)
        }
        else
        {
            Debug.LogError("doublesActivated is null!");
        }
    }
    // Coroutine to shake the popup text
    private IEnumerator ShakePopupEffect(float duration, float magnitudeX, float magnitudeY)
    {
        Vector3 originalPos = doublesActivated.transform.localPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = originalPos.x + Random.Range(-magnitudeX, magnitudeX);
            float y = originalPos.y + Random.Range(-magnitudeY, magnitudeY);

            doublesActivated.transform.localPosition = new Vector3(x, y, originalPos.z);

            elapsed += Time.deltaTime;
            yield return null;
        }

        doublesActivated.transform.localPosition = originalPos;
    }


    // Hide TMP Pro popup after a delay
    private IEnumerator HidePopupAfterDelay(float delay)
    {
        Debug.Log("Starting popup hide coroutine...");

        yield return new WaitForSeconds(delay);

        if (doublesActivated != null)
        {
            // Deactivate the TMP Pro text component after the delay
            doublesActivated.gameObject.SetActive(false);
        }
    }
}