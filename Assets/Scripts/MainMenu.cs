using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    [Header("Scene")]
    public string gameSceneName = "I am Snail";

    [Header("UI References")]
    public CanvasGroup menuCanvasGroup;   // The whole menu panel
    public Button playButton;
    public Button quitButton; 

    [Header("Fade Settings")]
    public float fadeOutDuration = 1f;

    void Start()
    {
        
        if (menuCanvasGroup != null)
            menuCanvasGroup.alpha = 1f;

        // Hook up buttons
        if (playButton != null)
            playButton.onClick.AddListener(OnPlayPressed);

        if (quitButton != null)
            quitButton.onClick.AddListener(OnQuitPressed);

        // Unlock cursor for menu
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void OnPlayPressed()
    {
        StartCoroutine(FadeAndLoad());
    }

    public void OnQuitPressed()
    {
        Application.Quit();
        Debug.Log("Quit pressed (only works in a built game)");
    }

    IEnumerator FadeAndLoad()
    {
        // Disable button so it can't be clicked twice
        if (playButton != null)
            playButton.interactable = false;

        // Fade out the menu
        if (menuCanvasGroup != null)
        {
            float elapsed = 0f;
            while (elapsed < fadeOutDuration)
            {
                elapsed += Time.deltaTime;
                menuCanvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsed / fadeOutDuration);
                yield return null;
            }
            menuCanvasGroup.alpha = 0f;
        }

        // Load the game scene
        SceneManager.LoadScene(gameSceneName);
    }
}