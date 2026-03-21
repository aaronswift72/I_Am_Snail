using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.InputSystem;


public class CutsceneManager : MonoBehaviour
{
    public PlayableDirector director;
    public GameObject playerController;
    public MouseCamera mouseCameraScript;
    public Canvas canvas;

    private static bool hasPlayed = false;

    private PlayerBehavior playerBehavior;
    private SnailMovementSprite snailSprite;

    void Start()
    {
        playerBehavior = playerController.GetComponent<PlayerBehavior>();
        snailSprite = playerController.GetComponent<SnailMovementSprite>();

        // Skip cutscene if it has already played
        if (hasPlayed)
        {
            if (playerBehavior != null) playerBehavior.enabled = true;
            if (snailSprite != null) snailSprite.enabled = true;
            if (mouseCameraScript != null) mouseCameraScript.enabled = true;
            gameObject.SetActive(false);
            return;
        }

        if (playerBehavior != null) playerBehavior.enabled = false;
        if (snailSprite != null) snailSprite.enabled = false;
        if (mouseCameraScript != null) mouseCameraScript.enabled = false;

        // Wait for  end
        director.stopped += OnCutsceneFinished;
        director.Play();

        // Disable UI
        foreach (Transform child in canvas.transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (Keyboard.current.mKey.wasPressedThisFrame)
        {
            director.Stop();
        }
    }

    void OnCutsceneFinished(PlayableDirector pd)
    {
        hasPlayed = true;

        // Re-enable everything
        if (playerBehavior != null) playerBehavior.enabled = true;
        if (snailSprite != null) snailSprite.enabled = true;
        if (mouseCameraScript != null) mouseCameraScript.enabled = true;

        director.stopped -= OnCutsceneFinished;
        gameObject.SetActive(false);

        // Enable UI
        foreach (Transform child in canvas.transform)
        {
            child.gameObject.SetActive(true);
        }
    }

    void OnDestroy()
    {
        director.stopped -= OnCutsceneFinished;
    }
}