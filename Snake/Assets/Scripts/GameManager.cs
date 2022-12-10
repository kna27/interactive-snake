using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverPanel;
    public TextMeshProUGUI scoreText;
    public int score;

    void Start()
    {
        gameOverPanel.SetActive(false);
    }

    private void Update()
    {
        scoreText.text = score.ToString();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Die();
        }
    }
    public void Die()
    {
        Time.timeScale = 0;
        gameOverPanel.SetActive(true);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}
