using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI popupScoreText;
    public GameObject popupScoreObject; // Reference to the GameObject containing popupScoreText
    public Camera mainCamera; // Reference to the main camera
    public float popupDistance = 5f; // Distance from the camera
    public float shakeDuration = 0.5f;
    public float shakeMagnitude = 1.0f;

    private int totalScore = 0;
    private Vector3 originalScorePos;
    private Vector3 originalPopupScorePos;

    private Queue<Tuple<int, Vector3>> popupScores = new Queue<Tuple<int, Vector3>>();
    private bool isShowingPopup = false;

    private void Start()
    {
        originalScorePos = scoreText.transform.localPosition;
        originalPopupScorePos = popupScoreText.transform.localPosition;
        UpdateScoreText();
        HidePopupScore(); // Hide the popup score text initially
    }

    private void Update()
    {
        if (!isShowingPopup && popupScores.Count > 0)
        {
            var scoreTuple = popupScores.Dequeue();
            int score = scoreTuple.Item1;
            Vector3 blackHoleWorldPos = scoreTuple.Item2;
            totalScore += score; // Add the popup score to the total score
            ShowPopupScore(score, blackHoleWorldPos);
        }
    }

    public void AddPopupScore(int score, Vector3 blackHoleWorldPos)
    {
        popupScores.Enqueue(new Tuple<int, Vector3>(score, blackHoleWorldPos));
    }

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

    void UpdateScoreText()
    {
        scoreText.text = totalScore.ToString();
        // Instantiate the aura particle system prefab when updating the score text
    }

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

    void HidePopupScore()
    {
        popupScoreObject.SetActive(false);
    }

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
}