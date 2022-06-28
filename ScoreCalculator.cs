using UnityEngine;
using TMPro;

public class ScoreCalculator : MonoBehaviour
{
    [SerializeField] private TMP_Text currentScoreValue;
    [SerializeField] private TMP_Text highScoreValue;
    void Start()
    {
        int currentScore = PlayerPrefs.GetInt("CurrentScore", 0);
        int highScore = PlayerPrefs.GetInt("HighScore", 0);

        if (highScore < currentScore)
        {
            highScore = currentScore;
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
        }

        currentScoreValue.text = currentScore.ToString();
        highScoreValue.text = highScore.ToString();
    }
}