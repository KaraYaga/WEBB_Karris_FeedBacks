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

    private Queue<int> popupScores = new Queue<int>();
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
            int score = popupScores.Dequeue();
            totalScore += score; // Increment the total score
            UpdateScoreText(); // Update the score text
            ShowPopupScore(score);
        }
    }

    public void AddPopupScore(int score)
    {
        popupScores.Enqueue(score);
    }

    IEnumerator ShakePopupScoreText()
    {
        isShowingPopup = true;
        float elapsed = 0.0f;

        while (elapsed < shakeDuration)
        {
            float x = originalPopupScorePos.x + Random.Range(-1f, 1f) * shakeMagnitude;
            float y = originalPopupScorePos.y + Random.Range(-1f, 1f) * shakeMagnitude;

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
        Color randomColor = new Color(Random.value, Random.value, Random.value);
        popupScoreText.color = randomColor;
    }

    void UpdateScoreText()
    {
        scoreText.text = totalScore.ToString();

    }

    void ShowPopupScore(int score)
    {
        popupScoreText.text = score.ToString(); // Set the popup score text to the generated score
        StartCoroutine(ShakePopupScoreText());
        ChangePopupScoreColorRandomly();
        ShowPopupScoreObject();
    }

    void ShowPopupScoreObject()
    {
        popupScoreObject.SetActive(true);

        // Calculate the direction from the black hole to the camera
        Vector3 direction = mainCamera.transform.position - popupScoreObject.transform.position;
        direction.Normalize();

        // Set the position of the popup score text in the direction of the black hole
        popupScoreObject.transform.position += direction * popupDistance;
    }

    void HidePopupScore()
    {
        popupScoreObject.SetActive(false);
    }
}