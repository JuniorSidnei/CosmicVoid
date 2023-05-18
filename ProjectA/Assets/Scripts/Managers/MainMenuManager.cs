using GameToBeNamed.Utils.Sound;
using ProjectA.Controllers;
using ProjectA.Singletons.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            AudioController.Instance.Play(UIClick, AudioController.SoundType.SoundEffect2D, GameSettings.GetSfxVolumeReduceScale());
            TransitionModal.Instance.DoTransitionIn(() => {
                if(SaveLoadManager.Instance.GameSettings.HasTutorialFinished) {
                    SceneManager.LoadScene("SelectStageMenu"); 
                } else{
                    SceneManager.LoadScene("TutorialGameScene");
                }
            });
        }

        public void QuitGame() {
            AudioController.Instance.Play(UIClick, AudioController.SoundType.SoundEffect2D, GameSettings.GetSfxVolumeReduceScale());
            Application.Quit();
        }

        public void ShowOptions() {
            AudioController.Instance.Play(UIClick, AudioController.SoundType.SoundEffect2D, GameSettings.GetSfxVolumeReduceScale());
            OptionsPanel.SetActive(true);    
        }
        
        public void HideOptions() {
            AudioController.Instance.Play(UIClick, AudioController.SoundType.SoundEffect2D, GameSettings.GetSfxVolumeReduceScale());
            OptionsPanel.SetActive(false);    
        }
        
        public void IncreaseMusicVolume() {
            GameSettings.MusicVolume += 1;
            AudioController.Instance.Play(UIClick, AudioController.SoundType.SoundEffect2D, GameSettings.GetSfxVolumeReduceScale());
            if(GameSettings.MusicVolume >= 10) GameSettings.MusicVolume = 10;

            MusicVolume.text = GameSettings.MusicVolume.ToString();
            AudioController.Instance.ChangeVolume(AudioController.SoundType.Music, GameSettings.GetMusicVolumeReduceScale());
            SaveLoadManager.SaveGame();
        }

        public void DecreaseMusicVolume() {
            GameSettings.MusicVolume -= 1;
            AudioController.Instance.Play(UIClick, AudioController.SoundType.SoundEffect2D, GameSettings.GetSfxVolumeReduceScale());
            if(GameSettings.MusicVolume <= 0) GameSettings.MusicVolume = 0;

            MusicVolume.text = GameSettings.MusicVolume.ToString();
            AudioController.Instance.ChangeVolume(AudioController.SoundType.Music, GameSettings.GetMusicVolumeReduceScale());
            SaveLoadManager.SaveGame();
        }

        public void IncreaseSfxVolume() {
            GameSettings.SfxVolume += 1;
            AudioController.Instance.ChangeVolume(AudioController.SoundType.SoundEffect2D, GameSettings.GetSfxVolumeReduceScale());
            AudioController.Instance.Play(UIClick, AudioController.SoundType.SoundEffect2D, GameSettings.GetSfxVolumeReduceScale());
            if(GameSettings.SfxVolume >= 10) GameSettings.SfxVolume = 10;

            SfxVolume.text = GameSettings.SfxVolume.ToString();
            SaveLoadManager.SaveGame();
        }

        public void DecreaseSfxVolume() {
            GameSettings.SfxVolume -= 1;
            AudioController.Instance.ChangeVolume(AudioController.SoundType.SoundEffect2D, GameSettings.GetSfxVolumeReduceScale());
            AudioController.Instance.Play(UIClick, AudioController.SoundType.SoundEffect2D, GameSettings.GetSfxVolumeReduceScale());
            if(GameSettings.SfxVolume <= 0) GameSettings.SfxVolume = 0;

            SfxVolume.text = GameSettings.SfxVolume.ToString();
            SaveLoadManager.SaveGame();
        }
    
        private void Start() {
            SaveLoadManager.LoadGame();
            TransitionModal.Instance.DoTransitionOut();
            MusicVolume.text = GameSettings.MusicVolume.ToString();
            SfxVolume.text = GameSettings.SfxVolume.ToString();
            
            AudioController.Instance.ChangeVolume(AudioController.SoundType.Music, GameSettings.GetMusicVolumeReduceScale());
            AudioController.Instance.Play(MenuTheme, AudioController.SoundType.Music, GameSettings.GetMusicVolumeReduceScale(), true);
        }
    }
}