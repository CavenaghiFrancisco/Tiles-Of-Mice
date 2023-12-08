using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardCalculator : MonoBehaviour
{
    public TMPro.TextMeshProUGUI scoreText;
    public int totalScore = 100;
    public float durationInSeconds = 5f;

    private int currentScore = 0;
    private float elapsedTime = 0f;

    void Start()
    {
        currentScore = 0;
        UpdateScoreText();

        StartCoroutine(AnimateScore());
    }


    void UpdateScoreText()
    {
        scoreText.text = currentScore.ToString();
    }

    IEnumerator AnimateScore()
    {
        while (currentScore < totalScore)
        {
            elapsedTime += Time.deltaTime;

            currentScore = (int)Mathf.Lerp(0, totalScore, elapsedTime / durationInSeconds);

            UpdateScoreText();

            yield return null;
        }

        currentScore = totalScore;
        UpdateScoreText();
    }
}
