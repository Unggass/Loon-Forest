using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI scoreText;
    int score = 0;

    public void AddScore(int value)
    {
        GameManager.Instance.AddScore(value);
    }

    public void ResetScore()
    {
        score = 0;
        scoreText.text = "Score = " + score;
    }
}