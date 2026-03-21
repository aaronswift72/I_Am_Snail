using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;


public class PauseMenu : MonoBehaviour
{
    [Header("UI")]
    public GameObject pausePanel;
    public Button resumeButton;
    public Button quitToMenuButton;

    [Header("Scene")]
    public string mainMenuSceneName = "MainMenu";

    private bool isPaused = false;

    void Start()
    {
        pausePanel.SetActive(false);

        if (resumeButton != null)
            resumeButton.onClick.AddListener(Resume);

        if (quitToMenuButton != null)
            quitToMenuButton.onClick.AddListener(QuitToMenu);
    }

    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }

    public void Pause()
    {
        // Shows pause menu and stops game
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        isPaused = true;
    }

    public void Resume()
    {
        //removes pause menu and resumes game
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isPaused = false;
    }

    public void QuitToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName);
    }
}