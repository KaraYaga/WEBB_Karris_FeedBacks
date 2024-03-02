using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ParticleAuraScript : MonoBehaviour
{

    private void Start()
    {
        StartCoroutine(DestroyAfterTime());
    }

    // Destroy after a certain amount of time
    private IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(3f); // Wait for 5 seconds
        DestroyAura();
    }

    // Destroy function
    private void DestroyAura()
    {
        Destroy(gameObject);
    }

}