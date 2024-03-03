using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    //SCORE AND POPUP
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI popupScoreText;
    public GameObject popupScoreObject; // Reference to the GameObject containing popupScoreText

    //POPUP SHAKE
    public Camera mainCamera; // Reference to the main camera
    public float popupDistance = 5f; // Distance from the camera
    public float shakeDuration = 0.5f;
    public float shakeMagnitude = 1.0f;
    public float pulseDuration = 1.0f; // Duration of one pulse cycle
    public float pulseMagnitude = 0.1f; // Magnitude of pulse effect

    //AUDIO FOR SCORE INCREASE
    public AudioClip scoreIncreaseSound; // Sound effect for score increase

    private int totalScore = 0;
    private Vector3 originalScorePos;
    private Vector3 originalPopupScorePos;
    private AudioSource audioSource; // Reference to the AudioSource component

    //DOUBLES POPUP
    private Queue<ScorePopupData> popupScores = new Queue<ScorePopupData>(); // Define a queue to hold score and position data
    private bool isShowingPopup = false;

    //SCORE BETWEEN SCENES
    public static int finalScore;

    //START SET TEXTS
    private void Start()
    {
        originalScorePos = scoreText.transform.localPosition;
        originalPopupScorePos = popupScoreText.transform.localPosition;
        UpdateScoreText();
        HidePopupScore(); // Hide the popup score text initially

        // Get the AudioSource component attached to the ScoreManager GameObject
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            // If AudioSource component is not found, add one
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Start pulsing the score text
        StartCoroutine(PulseScoreText());
    }

    //SHOW POPUP SCORE
    private void Update()
    {
        if (!isShowingPopup && popupScores.Count > 0)
        {
            var scoreData = popupScores.Dequeue();
            int score = scoreData.score;
            Vector3 blackHoleWorldPos = scoreData.position;
            totalScore += score; // Add the popup score to the total score
            ShowPopupScore(score, blackHoleWorldPos);
        }
    }
    //UPDATE MAIN SCORE
    void UpdateScoreText()
    {
        scoreText.text = totalScore.ToString();
        // Play the score increase sound effect
        if (scoreIncreaseSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(scoreIncreaseSound);
        }
    }
    //FINAL SCORE
    public int GetTotalScore()
    {
        return totalScore;
    }
    //ADDING POPUP
    public void AddPopupScore(int score, Vector3 blackHoleWorldPos)
    {
        popupScores.Enqueue(new ScorePopupData(score, blackHoleWorldPos));
    }
    //SHAKE POPUP
    IEnumerator ShakePopupScoreText()
    {
        isShowingPopup = true;
        float elapsed = 0.0f;

        while (elapsed < shakeDuration)
        {
            float x = originalPopupScorePos.x + UnityEngine.Random.Range(-1f, 1f) * shakeMagnitude;
            float y = originalPopupScorePos.y + UnityEngine.Random.Range(-1f, 1f) * shakeMagnitude;

            popupScoreText.transform.localPosition = new Vector3(x, y, originalPopupScorePos.z);

            elapsed += Time.deltaTime;
            yield return null;
        }

        popupScoreText.transform.localPosition = originalPopupScorePos;
        HidePopupScore();
        isShowingPopup = false;
    }
    //CHANGE POPUP COLOR
    void ChangePopupScoreColorRandomly()
    {
        // Generate random hue value in the range [0, 1]
        float randomHue = UnityEngine.Random.value;

        // Set saturation and value to high values to create neon colors
        float saturation = UnityEngine.Random.Range(0.8f, 1.0f); // Adjust saturation range as needed
        float value = UnityEngine.Random.Range(0.8f, 1.0f); // Adjust value range as needed

        // Convert HSV color to RGB
        Color randomColor = Color.HSVToRGB(randomHue, saturation, value);

        // Set the popup score text color to the generated random color
        popupScoreText.color = randomColor;
    }
    //SHOW POPUP
    void ShowPopupScore(int score, Vector3 blackHoleWorldPos)
    {
        popupScoreText.text = score.ToString(); // Set the popup score text to the generated score
        StartCoroutine(ShakePopupScoreText());
        ChangePopupScoreColorRandomly();
        ShowPopupScoreObject(blackHoleWorldPos);
        UpdateScoreText(); // Update the total score text
    }
    void ShowPopupScoreObject(Vector3 blackHoleWorldPos)
    {
        popupScoreText.gameObject.SetActive(true);

        // Convert black hole world position to screen coordinates
        Vector3 screenPos = mainCamera.WorldToScreenPoint(blackHoleWorldPos);

        // Set the position of the popup score text directly
        popupScoreText.rectTransform.position = screenPos + new Vector3(0f, popupDistance, 0f);
    }
    //HIDE POPUP
    void HidePopupScore()
    {
        popupScoreObject.SetActive(false);
    }
    //FIND BLACKHOLE POSITION
    Vector3 BlackHolePosition()
    {
        // Get the position of the black hole in world space
        GameObject blackHole = GameObject.FindGameObjectWithTag("BlackHole");
        if (blackHole != null)
        {
            return blackHole.transform.position;
        }
        else
        {
            return Vector3.zero;
        }
    }
    //SCORE POPUP DATA
    private class ScorePopupData
    {
        public int score;
        public Vector3 position;

        public ScorePopupData(int score, Vector3 position)
        {
            this.score = score;
            this.position = position;
        }
    }

    IEnumerator PulseScoreText()
    {
        Vector3 originalScale = scoreText.transform.localScale;
        float timer = 0f;

        while (true)
        {
            timer += Time.deltaTime;
            float pulse = Mathf.Sin(timer / pulseDuration * Mathf.PI * 2f) * pulseMagnitude;
            scoreText.transform.localScale = originalScale + new Vector3(pulse, pulse, 0f);

            yield return null;
        }
    }
}