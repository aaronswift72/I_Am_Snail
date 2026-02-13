using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Audio Clips")]
    public AudioClip birdsAmbience;
    public AudioClip windAmbience;
    public AudioClip coin1;
    public AudioClip coin2;
    public AudioClip coin3;
    public AudioClip jumpCharge;
    public AudioClip jump;
    //public AudioClip shimmer;
    public AudioClip snailTheme;
    public AudioClip splat1;
    public AudioClip splat2;
    public AudioClip splat3;
    public AudioClip splat4;




    [Header("Audio Sources")]
    public AudioSource musicSource;         // For Snail Theme
    public AudioSource windSource;          // For wind ambience
    public AudioSource birdsSource;         // For birds ambience
    public AudioSource jumpSource;          // For jump and jump charge
    public AudioSource coinSource;          // For coin collect

    [Header("Volume Settings")]
    [Range(0f, 1f)] public float sfxVolume = 1.3f;
    [Range(0f, 1f)] public float musicVolume = 0.7f;
    [Range(0f, 1f)] public float ambienceVolume = 0.1f;

    [Header("Start Delays")]
    public float musicStartDelay = 1f;
    public float ambienceStartDelay = 1f;

    void Awake()
    {
        // Singleton pattern
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Setup volumes
        musicSource.volume = musicVolume;
        windSource.volume = ambienceVolume;
        birdsSource.volume = ambienceVolume;
        jumpSource.volume = 0.35f;
        coinSource.volume = sfxVolume * 0.4f;
    

        // Start sounds with delays
        Invoke(nameof(StartMusic), musicStartDelay);
        Invoke(nameof(StartAmbience), ambienceStartDelay);
    }


    void StartMusic()
    {
        // Start Snail Theme music
        if (snailTheme != null)
        {
            musicSource.clip = snailTheme;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    void StartAmbience()
    {
        // Start wind ambience
        if (windAmbience != null)
        {
            windSource.clip = windAmbience;
            windSource.loop = true;
            windSource.Play();
        }

        // Start birds ambience
        if (birdsAmbience != null)
        {
            birdsSource.clip = birdsAmbience;
            birdsSource.loop = true;
            birdsSource.Play();
        }
    }
    public void PlayCoinCollect()
    {
        // Randomly pick one of the three coin sounds
        int randomIndex = Random.Range(0, 3);
        AudioClip coinSound = null;

        switch (randomIndex)
        {
            case 0:
                coinSound = coin1;
                break;
            case 1:
                coinSound = coin2;
                break;
            case 2:
                coinSound = coin3;
                break;
        }

        if (coinSound!= null)
        {
            coinSource.PlayOneShot(coinSound, sfxVolume);
        }
    }

    public void PlayJump()
    {
        if (jump != null)
        {
            jumpSource.PlayOneShot(jump, sfxVolume);
        }
    }

    // Jump charge looping sound
    public void StartJumpCharge()
    {
        if (jumpCharge != null && !jumpSource.isPlaying)
        {
            jumpSource.clip = jumpCharge;
            jumpSource.loop = false;
            jumpSource.Play();
        }
    }

    public void StopJumpCharge()
    {
        if (jumpSource.isPlaying && jumpSource.clip == jumpCharge)
        {
            jumpSource.Stop();
        }
    }

    public void PlaySplat()
{
    int randomIndex = Random.Range(0, 4);
    AudioClip splatSound = null;
    switch (randomIndex)
        {
            case 0:
                splatSound = splat1;
                break;
            case 1:
                splatSound = splat2;
                break;
            case 2:
                splatSound = splat3;
                break;
            case 3:
                splatSound = splat4;
                break;
        }
    if (splatSound != null)
    {
        jumpSource.PlayOneShot(splatSound, sfxVolume);
    }
}
}