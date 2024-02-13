using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwing : MonoBehaviour
{
  
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

    //Ready to Throw
    private void Start()
    {
  
        readyToThrow = true;
    }
   
    //THROWING
    void Update()
    { 
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

    //Collide with BlackHole
    private void OnTriggerEnter(Collider other)
    {
        // Check if the colliding GameObject has the "BlackHole" tag
        if (other.CompareTag("BlackHole"))
        {
            Destroy(); // Destroy the Star object
        }
    }

    //Reset Throw
    private void ResetThrow()
    {
        readyToThrow = true;
    }

    //Destroy Star
    public void Destroy()
    {
        Destroy(gameObject);
    }
}