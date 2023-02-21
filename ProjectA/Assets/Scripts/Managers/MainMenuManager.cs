using ProjectA.Controllers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ProjectA.Managers {
    
    public class MainMenuManager : MonoBehaviour {
        
        public void PlayGame() {
            TransitionModal.DoTransitionIn(() => {
                var isTutorialFinished = PlayerPrefs.GetInt("tutorial_finished");
                
                switch (isTutorialFinished) {
                    case 0:
                        SceneManager.LoadScene("TutorialGameScene");        
                        break;
                    case 1:
                        SceneManager.LoadScene("GameScene_1");
                        break;
                }
            });
        }

        public void QuitGame() {
            Application.Quit();
        }

        private void Start() {
            TransitionModal.DoTransitionOut();
        }
    }
}