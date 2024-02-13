using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Character Movement
    [SerializeField] private float speed = 5f;
    [SerializeField] private float sensitivity = 2f;

    private Rigidbody rb;
    private Camera playerCamera;
    private Vector2 rotation = Vector2.zero;

    //Objects needed for Shooting
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
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

        //Shoot Function
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        // Instantiate projectile at the fire point position and rotation
        Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        // Add shooting logic here (e.g., apply force to the bullet, play shooting sound, etc.)
    }
}