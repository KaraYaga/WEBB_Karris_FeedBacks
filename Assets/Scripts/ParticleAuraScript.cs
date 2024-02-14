using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleAuraScript : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(DestroyAfterTime());
    }
    //Destroy after a certain amount of time
    private IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(5f); // Wait for 7 seconds
        DestroyAura();
        
    }
    //Destroy function
    private void DestroyAura()
    {
        Destroy(gameObject);
    }

}
