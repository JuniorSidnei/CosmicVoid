using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ProjectA.Managers {
    
    public class MainMenuManager : MonoBehaviour {

        public Image Transition;
        
        public void PlayGame() {
            Transition.DOFade(1, .5f).OnComplete(() => {
                SceneManager.LoadScene("SampleScene");
            });
        }

        public void QuitGame() {
            Application.Quit();
        }
    }
}