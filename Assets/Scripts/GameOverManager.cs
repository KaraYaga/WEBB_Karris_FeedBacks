using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    public TextMeshProUGUI finalScoreText; // Text to display the final score

    // Parameters for pulsation
    public float minScale = 0.9f;
    public float maxScale = 1.1f;
    public float pulsateSpeed = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        // Retrieve the final score from player prefs and display it
        DisplayFinalScore();

        // Start the pulsation coroutine
        StartCoroutine(PulsateText());
    }

    // Coroutine for pulsating text
    IEnumerator PulsateText()
    {
        while (true)
        {
            // Pulsate from minScale to maxScale and back
            float t = Mathf.PingPong(Time.time * pulsateSpeed, 1.0f);
            float scale = Mathf.Lerp(minScale, maxScale, t);
            finalScoreText.transform.localScale = new Vector3(scale, scale, scale);

            yield return null;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void DisplayFinalScore()
    {
        // Retrieve the final score from player prefs
        int finalScore = ScoreManager.finalScore;

        // Display the final score
        finalScoreText.text = finalScore.ToString();
    }
}