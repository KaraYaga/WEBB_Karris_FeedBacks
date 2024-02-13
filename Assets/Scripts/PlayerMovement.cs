using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header ("Movement")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float sensitivity = 2f;
    private Rigidbody rb;
    private Camera playerCamera;
    private Vector2 rotation = Vector2.zero;

    [Header("References")]
    public Transform cam;
    public Transform attackPoint;
    public GameObject objectToThrow;

    [Header("Throwing")]
    public KeyCode throwkey = KeyCode.Mouse0;
    public float throwForce;
    public float throwUpwardForce;
    public float throwCooldown;

    bool readyToThrow;

    private void Start()
    {
        //For Movement
        rb = GetComponent<Rigidbody>();
        playerCamera = Camera.main;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //For Shooting
        readyToThrow = true;
    }

    //PLAYER MOVEMENT
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
        if (Input.GetKeyDown(throwkey) && readyToThrow)
        {
            Throw();
        }
    }

    public void Throw()
    {
        readyToThrow = false;

        //Instantiate object to throw
        GameObject projectile = Instantiate(objectToThrow, attackPoint.position, cam.rotation);

        //Get Rigidbody Component
        Rigidbody projectileRB = projectile.GetComponent<Rigidbody>();

        //Add Force
        Vector3 forceToAdd = cam.transform.forward * throwForce + transform.up * throwUpwardForce;

        projectileRB.AddForce(forceToAdd, ForceMode.Impulse);

        //Implement throwCooldown
        Invoke(nameof(ResetThrow), throwCooldown);
    }

    private void ResetThrow()
    {
        readyToThrow = true;
    }
}