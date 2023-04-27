using ProjectA.Controllers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjectA.Managers {
    
    public class MainMenuManager : MonoBehaviour {

        public GameObject OptionsPanel;
        
        public void PlayGame() {
            TransitionModal.Instance.DoTransitionIn(() => {
                SceneManager.LoadScene("SelectStageMenu"); 
                // var isTutorialFinished = PlayerPrefs.GetInt("tutorial_finished");
                //
                // switch (isTutorialFinished) {
                //     case 0:
                //         SceneManager.LoadScene("TutorialGameScene");        
                //         break;
                //     case 1:
                //         SceneManager.LoadScene("GameScene_1");
                //         break;
                // }
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
        
        private void Start() {
            TransitionModal.Instance.DoTransitionOut();
        }
    }
}