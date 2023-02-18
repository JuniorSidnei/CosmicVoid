using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using LustyGod.Controllers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ProjectA.Managers {
    
    public class MainMenuManager : MonoBehaviour {
        
        public void PlayGame() {
            TransitionModal.DoTransitionIn(() => {
                SceneManager.LoadScene("GameScene");
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