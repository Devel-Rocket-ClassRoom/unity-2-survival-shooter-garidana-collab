using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public CanvasGroup gameOverUI;
    public PauseMenu pauseMenu;

    private int score = 0;
    public bool IsGameOver {get; private set;}

    void Start()
    {
        scoreText.text = $"Score :{score}";
        gameOverUI.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenu.TogglePause();
        }
    }

    public void AddScore (int AddScore)
    {
        if (IsGameOver)
        {
            return;
        }

        score += AddScore;
        scoreText.text = $"Score: {score}";
    }

    public void SetActiveGameOverUI(bool SetActive)
    {
        if (SetActive)
        {
            gameOverUI.enabled = true;
            StartCoroutine(FadeIn());
        }
        else
        {
            gameOverUI.gameObject.SetActive(false);
        }
    }

    private IEnumerator FadeIn()
    {
        gameOverUI.alpha = 0f;
        gameOverUI.gameObject.SetActive(true);

        while (gameOverUI.alpha < 1f)
        {
            gameOverUI.alpha += Time.deltaTime/2;
            yield return null;
        }

        gameOverUI.alpha = 1f;
    }
}
