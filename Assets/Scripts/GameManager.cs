using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject finalScoreText;
    public AudioClip winSound;
    public AudioSource backgroundMusic;

    private void Start()
    {
        // Subscribe to the event indicating all black holes are destroyed
        BlackHole.OnBlackHoleDestroyed += HandleAllBlackHolesDestroyed;
    }

    private void HandleAllBlackHolesDestroyed()
    {
        // Display final score text
        finalScoreText.SetActive(true);

        // Pause the game
        Time.timeScale = 0f;

        // Pause the music
        if (backgroundMusic != null)
        {
            backgroundMusic.Pause();
        }

        // Play win sound
        if (winSound != null)
        {
            AudioSource.PlayClipAtPoint(winSound, Camera.main.transform.position);
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe from the event
        BlackHole.OnBlackHoleDestroyed -= HandleAllBlackHolesDestroyed;
    }
}