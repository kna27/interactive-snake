using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void StartGame(bool speedIncreases)
    {
        GameSettings.speedIncreases = speedIncreases;
        SceneManager.LoadScene(1);
    }
}
