using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance = null;
    public static AudioManager Instance
    {
        get { return instance; }
    }

    [SerializeField]
    private AudioSource backGroundMusic;
    [SerializeField]
    private AudioSource jumpSFX;
    [SerializeField]
    private AudioSource passSFX;
    [SerializeField]
    private AudioSource loseSFX;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;

            PlayerController.playerJumped += () =>
            {
                PlayJumpSFX();
            };

            GameManager.levelPassed += () =>
            {
                PlayPassSFX();
            };

            PlayerController.playerCollided += () =>
            {
                PlayLoseSFX();
            };
            DontDestroyOnLoad(this.gameObject);
        }
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey("musicVolume"))
            SetBackgoundMusicVolume(PlayerPrefs.GetFloat("musicVolume"));

        if (PlayerPrefs.HasKey("SFXVolume"))
            SetSFXVolume(PlayerPrefs.GetFloat("SFXVolume"));
    }

    private void PlayJumpSFX() { jumpSFX.Play(); }
    private void PlayPassSFX() { passSFX.Play(); }
    private void PlayLoseSFX() { loseSFX.Play(); }

    public void SetBackgoundMusicVolume(float val)
    {
        float v = Mathf.Clamp(val, 0f, 1f);
        backGroundMusic.volume = v;
        PlayerPrefs.SetFloat("musicVolume", v);
    }
    public void SetSFXVolume(float val)
    {
        float v = Mathf.Clamp(val, 0f, 1f);
        jumpSFX.volume = v;
        passSFX.volume = v;
        loseSFX.volume = v;
        PlayerPrefs.SetFloat("SFXVolume", v);
    }
}
