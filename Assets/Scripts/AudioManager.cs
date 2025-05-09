using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static  AudioManager Instance { get; private set; }

    [Header("Audio Clip")]
    public AudioClip leaderboardMusic;
    public AudioClip menuMusic;
    public AudioClip leaderboardEntry;

    public AudioClip luckyDipPUsound;
    public AudioClip invincibilityPUsound;
    public AudioClip freezePUsound;

    public AudioClip powerUpPickupSound;
    public AudioClip leapSound;

    public AudioClip waterDeathSFX;
    public AudioClip gameOverSFX;
    public AudioClip carHitSFX;
    public AudioClip timeDeathSFX;
    public AudioClip respawnSFX;

    [Header("Audio Source")]
    private AudioSource musicSource;
    private AudioSource sfxSource;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        musicSource = gameObject.AddComponent<AudioSource>();
        sfxSource = gameObject.AddComponent<AudioSource>();

        musicSource.loop = true;
        musicSource.playOnAwake = false;

        if (SceneManager.GetActiveScene().name != "StartMenu" && menuMusic != null)
        {
            PlayMusic(menuMusic);
        }
    }

    public void PlayMusic(AudioClip clip)
    {
        Debug.Log($"PlayMusic called by {clip.name} in scene {UnityEngine.SceneManagement.SceneManager.GetActiveScene().name}");

        Debug.Log("Trying to play: " + clip.name);
        if (clip == null) return;

        if (musicSource.clip == clip && musicSource.isPlaying) return;

        musicSource.clip = clip;
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
        musicSource.clip = null;
    }


    public void PlaySFX(AudioClip clip)
    {
        if (clip != null) sfxSource.PlayOneShot(clip);
    }

    public void PlayPowerUpSound(PowerUpType type)
    {
        switch (type)
        {
            case PowerUpType.Freeze:
                PlaySFX(freezePUsound);
                break;
            case PowerUpType.Invincibility:
                PlaySFX(invincibilityPUsound);
                break;
            case PowerUpType.LuckyDip:
                PlaySFX(luckyDipPUsound);
                break;
        }
    }

    public string GetCurrentClipName()
    {
        return musicSource.clip != null ? musicSource.clip.name : "No clip";
    }
}
