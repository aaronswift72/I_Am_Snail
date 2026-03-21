using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Audio Clips")]
    public AudioClip birdsAmbience;
    public AudioClip windAmbience;
    public AudioClip rainAmbience;
    public AudioClip coin1;
    public AudioClip coin2;
    public AudioClip coin3;
    public AudioClip jumpCharge;
    public AudioClip jump;
    public AudioClip snailTheme;
    public AudioClip splat1;
    public AudioClip splat2;
    public AudioClip splat3;
    public AudioClip splat4;

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource windSource;
    public AudioSource birdsSource;
    public AudioSource jumpSource;
    public AudioSource coinSource;
    public AudioSource rainSource;

    [Header("Volume Settings")]
    [Range(0f, 1f)] public float sfxVolume = 1.3f;
    [Range(0f, 1f)] public float musicVolume = 0.7f;
    [Range(0f, 1f)] public float ambienceVolume = 0.1f;

    [Header("Start Delays")]
    public float musicStartDelay = 1f;
    public float ambienceStartDelay = 1f;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        musicSource.volume = musicVolume;
        windSource.volume = ambienceVolume;
        birdsSource.volume = ambienceVolume;
        jumpSource.volume = 0.35f;
        coinSource.volume = sfxVolume * 0.6f;
        rainSource.volume = 0.15f;

        // Start sounds with delays
        Invoke(nameof(StartMusic), musicStartDelay);
        Invoke(nameof(StartAmbience), ambienceStartDelay);
    }

    void StartMusic()
    {
        // Start snail theme
        if (snailTheme != null)
        {
            musicSource.clip = snailTheme;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    void StartAmbience()
    {
        // Start wind
        if (windAmbience != null)
        {
            windSource.clip = windAmbience;
            windSource.loop = true;
            windSource.Play();
        }
        // Start birds
        if (birdsAmbience != null)
        {
            birdsSource.clip = birdsAmbience;
            birdsSource.loop = true;
            birdsSource.Play();
        }
    }

    public void PlayCoinCollect()
    {
        // Plays random coin noise
        int randomIndex = Random.Range(0, 3);
        AudioClip coinSound = randomIndex == 0 ? coin1 : randomIndex == 1 ? coin2 : coin3;
        if (coinSound != null) coinSource.PlayOneShot(coinSound, sfxVolume);
    }

    public void PlayJump()
    {
        // Plays jump noise (non-existent as of know)
        if (jump != null) jumpSource.PlayOneShot(jump, sfxVolume);
    }

    public void StartJumpCharge()
    {
        // Starts jump charge
        if (jumpCharge != null && !jumpSource.isPlaying)
        {
            jumpSource.clip = jumpCharge;
            jumpSource.loop = false;
            jumpSource.Play();
        }
    }

    public void StopJumpCharge()
    {
        // Stops jump charge
        if (jumpSource.isPlaying && jumpSource.clip == jumpCharge)
            jumpSource.Stop();
    }

    public void PlaySplat()
    {
        // Plays splat noise
        int randomIndex = Random.Range(0, 4);
        AudioClip splatSound = randomIndex == 0 ? splat1 : randomIndex == 1 ? splat2 : randomIndex == 2 ? splat3 : splat4;
        if (splatSound != null) jumpSource.PlayOneShot(splatSound, sfxVolume);
    }

    public void StartRain()
    {
        // Plays rain and stops wind/birds
        windSource.Stop();
        birdsSource.Stop();
        if (rainAmbience != null)
        {
            rainSource.clip = rainAmbience;
            rainSource.loop = true;
            rainSource.Play();
        }
    }

    public void StopRain()
    {
        // Stops rain and plays wind/birds
        rainSource.Stop();
        windSource.Play();
        birdsSource.Play();
    }
}