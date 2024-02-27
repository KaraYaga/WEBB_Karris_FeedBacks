using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreStarGlow : MonoBehaviour
{
    public GameObject auraParticleSystemPrefab; // Reference to the aura particle system prefab

    // Method to instantiate the aura particle system on the star GameObject
    public void InstantiateAuraParticleSystem()
    {
        // Check if the aura particle system prefab is assigned
        if (auraParticleSystemPrefab != null)
        {
            // Instantiate the aura particle system at the position of this star GameObject
            Instantiate(auraParticleSystemPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("Aura Particle System Prefab is not assigned in the ScoreStarGlow script!");
        }
    }
}