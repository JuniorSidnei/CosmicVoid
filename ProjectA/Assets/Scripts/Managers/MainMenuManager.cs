using GameToBeNamed.Utils.Sound;
using ProjectA.Controllers;
using ProjectA.Singletons.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ProjectA.Managers {
    
    public class MainMenuManager : MonoBehaviour {

        public GameSettings GameSettings;
        public SaveLoadManager SaveLoadManager;
        public GameObject OptionsPanel;
        
        [Header("options settings")]
        public TextMeshProUGUI MusicVolume;
        public TextMeshProUGUI SfxVolume;

        [Header("audio settings")]
        public AudioClip MenuTheme;
        public AudioClip UIClick;
            
        public void PlayGame() {
            AudioController.Instance.Play(UIClick, AudioController.SoundType.SoundEffect2D, GameSettings.SfxVolume);
            TransitionModal.Instance.DoTransitionIn(() => {
                if(SaveLoadManager.Instance.GameSettings.HasTutorialFinished) {
                    SceneManager.LoadScene("SelectStageMenu"); 
                } else{
                    SceneManager.LoadScene("TutorialGameScene");
                }
            });
        }

        public void QuitGame() {
            AudioController.Instance.Play(UIClick, AudioController.SoundType.SoundEffect2D, GameSettings.SfxVolume);
            Application.Quit();
        }

        public void ShowOptions() {
            AudioController.Instance.Play(UIClick, AudioController.SoundType.SoundEffect2D, GameSettings.SfxVolume);
            OptionsPanel.SetActive(true);    
        }
        
        public void HideOptions() {
            AudioController.Instance.Play(UIClick, AudioController.SoundType.SoundEffect2D, GameSettings.SfxVolume);
            OptionsPanel.SetActive(false);    
        }
        
        public void IncreaseMusicVolume() {
            GameSettings.MusicVolume += 1;
            AudioController.Instance.Play(UIClick, AudioController.SoundType.SoundEffect2D, GameSettings.SfxVolume);
            if(GameSettings.MusicVolume >= 10) GameSettings.MusicVolume = 10;

            MusicVolume.text = GameSettings.MusicVolume.ToString();
        }

        public void DecreaseMusicVolume() {
            GameSettings.MusicVolume -= 1;
            AudioController.Instance.Play(UIClick, AudioController.SoundType.SoundEffect2D, GameSettings.SfxVolume);
            if(GameSettings.MusicVolume <= 0) GameSettings.MusicVolume = 0;

            MusicVolume.text = GameSettings.MusicVolume.ToString();
        }

        public void IncreaseSfxVolume() {
            GameSettings.SfxVolume += 1;
            AudioController.Instance.Play(UIClick, AudioController.SoundType.SoundEffect2D, GameSettings.SfxVolume);
            if(GameSettings.SfxVolume >= 10) GameSettings.SfxVolume = 10;

            SfxVolume.text = GameSettings.SfxVolume.ToString();
        }

        public void DecreaseSfxVolume() {
            GameSettings.SfxVolume -= 1;
            AudioController.Instance.Play(UIClick, AudioController.SoundType.SoundEffect2D, GameSettings.SfxVolume);
            if(GameSettings.SfxVolume <= 0) GameSettings.SfxVolume = 0;

            SfxVolume.text = GameSettings.SfxVolume.ToString();
        }
    
        private void Start() {
            TransitionModal.Instance.DoTransitionOut();
            MusicVolume.text = GameSettings.MusicVolume.ToString();
            SfxVolume.text = GameSettings.SfxVolume.ToString();
            
            AudioController.Instance.Play(MenuTheme, AudioController.SoundType.Music, GameSettings.MusicVolume, true);
        }
    }
}