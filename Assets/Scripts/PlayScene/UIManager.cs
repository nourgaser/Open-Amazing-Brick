using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;


public class UIManager : MonoBehaviour
{

    [SerializeField]
    private Button pauseButton;
    [SerializeField]
    private Button unPauseButton;
    [SerializeField]
    private Button backToMainMenuButton;

    [SerializeField]
    private GameObject pauseMenu;

    [SerializeField]
    private TMP_Text score;

    void Awake()
    {
        pauseButton.onClick.AddListener(PauseGame);
        PlayerController.playerPaused += PauseGame;
        unPauseButton.onClick.AddListener(UnPauseGame);
        backToMainMenuButton.onClick.AddListener(BackToMainMenu);

        GameManager.levelPassed += () => {
            score.text = GameManager.score.ToString();
        };
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
        pauseButton.gameObject.SetActive(false);
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().enabled = false;

    }

    public void UnPauseGame()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        pauseButton.gameObject.SetActive(true);
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().enabled = true;
    }

    public void BackToMainMenu() {
        UnPauseGame();
        SceneManager.LoadScene("MainMenu");
    }

    private void OnApplicationPause(bool pauseStatus) {
        
    }

}
