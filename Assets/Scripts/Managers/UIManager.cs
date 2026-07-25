using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("UI Canvas")]
    public GameObject pauseCanvas;
    public GameObject gameOverCanvas;
    public GameObject scoreCanvas;
    public GameObject guideCanvas;

    [Header("Panel")]
    public GameObject creditPanel;

    [Header("Score")]
    public TextMeshProUGUI scoreText;

    bool isShown = false;
    static bool hasShownGuide = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        if (Instance == null)
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
        if (!guideCanvas) { return; }
        if (hasShownGuide)
        {
            // udah pernah liat guide, langsung mulai game
            guideCanvas.SetActive(false);
            GameManager.Instance.GameStart();
        }
        else
        {
            // pertama kali, tampilin guide, game masih pause
            guideCanvas.SetActive(true);
        }
    }

    public void ScoreCount(int score)
    {
        scoreText.text = "Score = " + score;
    }

    public void TogglePause(bool isPause)
    {
        pauseCanvas.SetActive(isPause);
    }

    public void ResumeButton()
    {
        GameManager.Instance.PauseGame();
    }

    public void RestarButton()
    {
        GameManager.Instance.RestartGame();
    }

    public void GameOverUI()
    {
        gameOverCanvas.SetActive(true);
    }

    public void GuideUI()
    {
        guideCanvas.SetActive(false);
        GameManager.Instance.GameStart();
        hasShownGuide = true;
    }

    public void PlayButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Gameplay");
    }
    public void MainMenuButton()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void CreditButton(bool show)
    {
        isShown = show;

        creditPanel.SetActive(isShown);
    }

    public void ExitButton()
    {
        Debug.Log("Game Quit!");
        Application.Quit();
    }
}