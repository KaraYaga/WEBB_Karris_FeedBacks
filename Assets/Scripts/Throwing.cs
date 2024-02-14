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

    private bool readyToThrow = true;

    //Ready to Throw
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

        //Instantiate object to throw
        GameObject projectile = Instantiate(objectToThrow, attackPoint.position, cam.rotation);

        //Add Rigidbody Component if not already attached
        Rigidbody projectileRB = projectile.GetComponent<Rigidbody>();
        if (projectileRB == null)
            projectileRB = projectile.AddComponent<Rigidbody>();

        //Add Force
        Vector3 forceToAdd = cam.forward * throwForce + cam.up * throwUpwardForce;
        projectileRB.AddForce(forceToAdd, ForceMode.Impulse);

        //Implement throwCooldown
        StartCoroutine(ResetThrow());
    }
    //Throw Cooldown
    private IEnumerator ResetThrow()
    {
        yield return new WaitForSeconds(throwCooldown);
        readyToThrow = true;
    }
}