using UnityEngine;
using UnityEngine.Playables;

public class CutsceneManager : MonoBehaviour
{
    public PlayableDirector director;
    public GameObject playerController;
    public MouseCamera mouseCameraScript;

    private PlayerBehavior playerBehavior;
    private SnailMovementSprite snailSprite;

    void Start()
    {
        // Get  scripts we want to disable
        playerBehavior = playerController.GetComponent<PlayerBehavior>();
        snailSprite = playerController.GetComponent<SnailMovementSprite>();

        // Disable input and sprite logic, keeping snail visible
        if (playerBehavior != null) playerBehavior.enabled = false;
        if (snailSprite != null) snailSprite.enabled = false;
        if (mouseCameraScript != null) mouseCameraScript.enabled = false;

        // Wait for timeline end
        director.stopped += OnCutsceneFinished;
        director.Play();
    }

    void OnCutsceneFinished(PlayableDirector pd)
    {
        // Re-enable everything
        if (playerBehavior != null) playerBehavior.enabled = true;
        if (snailSprite != null) snailSprite.enabled = true;
        if (mouseCameraScript != null) mouseCameraScript.enabled = true;

        director.stopped -= OnCutsceneFinished;
        gameObject.SetActive(false);
    }

    void OnDestroy()
    {
        director.stopped -= OnCutsceneFinished;
    }
}