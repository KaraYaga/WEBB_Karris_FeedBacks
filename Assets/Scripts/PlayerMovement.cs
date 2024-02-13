using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Movement
    [SerializeField] private float speed = 5f;
    [SerializeField] private float sensitivity = 2f;

    private Rigidbody rb;
    private Camera playerCamera;
    private Vector2 rotation = Vector2.zero;

    //Shoot
    [SerializeField] private GameObject projectilePrefab;
    public Transform firePoint;
    public float shootForce = 10f;
    public float upForce = 1f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerCamera = Camera.main;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Rotation based on mouse movement
        rotation.y += Input.GetAxis("Mouse X") * sensitivity;
        rotation.x += -Input.GetAxis("Mouse Y") * sensitivity;
        rotation.x = Mathf.Clamp(rotation.x, -90f, 90f); // Clamp vertical rotation

        // Apply rotation to the player and the camera
        transform.eulerAngles = new Vector2(0, rotation.y);
        playerCamera.transform.localRotation = Quaternion.Euler(rotation.x, 0, 0);

        // Movement
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 rightMovement = transform.right * horizontalInput;
        Vector3 forwardMovement = transform.forward * verticalInput;
        Vector3 movement = (rightMovement + forwardMovement).normalized;
        rb.MovePosition(transform.position + movement * speed * Time.deltaTime);

        // Shooting
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    public void Shoot()
    {
        // Instantiate arrow prefab at the fire point position and rotation
        GameObject arrow = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            // Calculate throw direction
            Vector3 throwDirection = (hit.point - arrow.transform.position).normalized * shootForce;

            // Add extra upward force
            throwDirection += Vector3.up * upForce;

            // Apply force to the arrow
            arrow.GetComponent<Rigidbody>().velocity = throwDirection;
        }
    }
}