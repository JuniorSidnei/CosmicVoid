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
 
        public void PlayGame() {
            TransitionModal.Instance.DoTransitionIn(() => {
                if(SaveLoadManager.Instance.GameSettings.HasTutorialFinished) {
                    SceneManager.LoadScene("SelectStageMenu"); 
                } else{
                    SceneManager.LoadScene("TutorialGameScene");
                }
            });
        }

        public void QuitGame() {
            Application.Quit();
        }

        public void ShowOptions() {
            OptionsPanel.SetActive(true);    
        }
        
        public void HideOptions() {
            OptionsPanel.SetActive(false);    
        }
        
        public void IncreaseMusicVolume() {
            GameSettings.MusicVolume += 1;

            if(GameSettings.MusicVolume >= 10) GameSettings.MusicVolume = 10;

            MusicVolume.text = GameSettings.MusicVolume.ToString();
        }

        public void DecreaseMusicVolume() {
            GameSettings.MusicVolume -= 1;

            if(GameSettings.MusicVolume <= 0) GameSettings.MusicVolume = 0;

            MusicVolume.text = GameSettings.MusicVolume.ToString();
        }

        public void IncreaseSfxVolume() {
            GameSettings.SfxVolume += 1;

            if(GameSettings.SfxVolume >= 10) GameSettings.SfxVolume = 10;

            SfxVolume.text = GameSettings.SfxVolume.ToString();
        }

        public void DecreaseSfxVolume() {
            GameSettings.SfxVolume -= 1;

            if(GameSettings.SfxVolume <= 0) GameSettings.SfxVolume = 0;

            SfxVolume.text = GameSettings.SfxVolume.ToString();
        }
    
        private void Start() {
            TransitionModal.Instance.DoTransitionOut();
            MusicVolume.text = GameSettings.MusicVolume.ToString();
            SfxVolume.text = GameSettings.SfxVolume.ToString();
        }
    }
}