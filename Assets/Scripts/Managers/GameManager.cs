using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.DebugUI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Difficulty")]
    [SerializeField] private int scoreStep = 20;

    int scoreToAdd;

    bool isPause = false;
    bool isGameOver = false;

    public static event System.Action<int> DifficultyIncrease;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Time.timeScale = 0f;
    }

    public void GameStart()
    {
        Time.timeScale = 1f;
    }

    public void AddScore(int score)
    {
        int oldScore = scoreToAdd;
        scoreToAdd = Mathf.Max(0, scoreToAdd + score);

        UIManager.Instance.ScoreCount(scoreToAdd);

        GameDifficulty(oldScore, scoreToAdd);
    }

    public void GameDifficulty(int oldScore, int newScore)
    {
        int oldSteps = oldScore / scoreStep;
        int newSteps = newScore / scoreStep;

        if (newSteps > oldSteps)
        {
            int stepsPassed = newSteps - oldSteps;
            DifficultyIncrease?.Invoke(stepsPassed);

            Debug.Log($"Score kelipatan {scoreStep} tercapai! Steps: {stepsPassed}");
        }
    }

    public void PauseGame()
    {
        isPause = !isPause;

        Time.timeScale = isPause ? 0f : 1f;
        Debug.Log("Game Paused!");

        UIManager.Instance.TogglePause(isPause);
    }
    
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GameOver()
    {
        if (isGameOver) { return; }

        isGameOver = true;

        Time.timeScale = 0;

        UIManager.Instance.GameOverUI();
    }
}
