using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleAuraScript : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(DestroyAfterTime());
    }
    private IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(7f); // Wait for 7 seconds
        DestroyAura();
        
    }

    private void DestroyAura()
    {
        Destroy(gameObject);
    }

}
