using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public AudioClip winSound;
    public string nextSceneName; // Name of the scene to load after winning
    public float countdownDuration = 30f; // Duration of the countdown timer
    public TextMeshProUGUI countdownText; // Text to display the countdown timer

    private float countdownTimer; // Timer for the countdown

    private void Start()
    {
        // Start the countdown timer
        countdownTimer = countdownDuration;
        UpdateCountdownText();
    }

    private void Update()
    {
        // Update the countdown timer
        countdownTimer -= Time.deltaTime;
        UpdateCountdownText();

        // Check if the countdown has reached zero
        if (countdownTimer <= 0)
        {
            countdownTimer = 0; // Ensure the timer doesn't go negative
            LoadGameOverScene();
        }
    }

    private void LoadGameOverScene()
    {
        // Save the final score before loading the GameOver scene
        ScoreManager.finalScore = FindObjectOfType<ScoreManager>().GetTotalScore();
        Debug.Log(ScoreManager.finalScore);

        // Load the GameOver scene
        SceneManager.LoadScene("GameOver");
    }

    private void UpdateCountdownText()
    {
        // Update the countdown text
        int seconds = Mathf.CeilToInt(countdownTimer);
        countdownText.text = "Time Left: " + seconds.ToString();
    }
}