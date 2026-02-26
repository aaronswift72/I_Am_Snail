using UnityEngine;
using UnityEngine.Playables;

public class CutsceneManager : MonoBehaviour
{
    public PlayableDirector director;
    public GameObject playerController;

    void Start()
    {
        playerController.SetActive(false);
        director.Play();
    }

    void OnDisable()
    {
        playerController.SetActive(true);
    }
}