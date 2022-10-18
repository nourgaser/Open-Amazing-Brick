using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

namespace MainMenu
{
    public class UIManager : MonoBehaviour
    {

        [SerializeField]
        private Slider sfxSlider;

        [SerializeField]
        private Slider musicSlider;

        [SerializeField]
        private TMP_Text highscore;

        [SerializeField]
        private TMP_Text lastscore;

        void Awake()
        {
            musicSlider.onValueChanged.AddListener(AudioManager.Instance.SetBackgoundMusicVolume);
            sfxSlider.onValueChanged.AddListener(AudioManager.Instance.SetSFXVolume);

            GameManager.gameEnded += () =>
            {
                UpdateScores();
            };

            if (!PlayerPrefs.HasKey("highscore")) PlayerPrefs.SetInt("highscore", 0);
            if (!PlayerPrefs.HasKey("lastscore")) PlayerPrefs.SetInt("lastscore", 0);

            if (!PlayerPrefs.HasKey("musicVolume")) PlayerPrefs.SetFloat("musicVolume", 1f);
            if (!PlayerPrefs.HasKey("SFXVolume")) PlayerPrefs.SetFloat("SFXVolume", 0.5f);


        }

        private void Start()
        {
            UpdateScores();
            UpdateVolumes();
        }

        public void EnterPlayScene()
        {
            SceneManager.LoadScene("Play");
        }

        public void EnterMainMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }

        void UpdateScores()
        {
            lastscore.text = PlayerPrefs.GetInt("lastscore").ToString();
            highscore.text = PlayerPrefs.GetInt("highscore").ToString();
        }


        void UpdateVolumes()
        {
            musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
            sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume");
        }
    }
}