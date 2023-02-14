using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using ProjectA.Singletons.Managers;
using ProjectA.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectA.Modals {
    
    public class PauseModal : Singleton<PauseModal> {

        public Image PanelBg;
        public RectTransform BgRect;

        public bool IsGamePaused { get; set; }
        
        public void PauseGame() {
            PanelBg.DOFade(0.8f, 0.25f);
            BgRect.DOLocalMoveY(0, .35f).OnComplete(() => {
                Time.timeScale = 0;
                IsGamePaused = true;
            });
        }

        public void ResumeGame() {
            Time.timeScale = 1;
            PanelBg.DOFade(0, 0.15f);
            BgRect.DOLocalMoveY(-740f, .35f).OnComplete(() => {
                IsGamePaused = false;
            });
        }
    }
}