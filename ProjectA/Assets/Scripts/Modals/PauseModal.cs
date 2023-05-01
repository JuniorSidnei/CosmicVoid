using System;
using DG.Tweening;
using ProjectA.Controllers;
using ProjectA.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ProjectA.Modals {
    
    public class PauseModal : Singleton<PauseModal> {

        public Image PanelBg;
        public RectTransform BgRect;
        public TextMeshProUGUI ModalText;
        public Button ActionBtn;
        
        public bool IsGamePaused { get; set; }
        public bool IsGameEnded { get; set; }
        public bool IsHighScoreModalOn { get; set; }
        
        private Action m_onActionBtn;
        
        public void PauseGame(bool isEndGame = false) {
            if(IsHighScoreModalOn) return;
            
            ModalText.text = isEndGame ? "Retry" : "Resume";
            PanelBg.DOFade(0.8f, 0.15f);
            BgRect.DOLocalMoveY(0, .2f).OnComplete(() => {
                Time.timeScale = 0;
                IsGamePaused = true;
            });
            
            m_onActionBtn = isEndGame ? DoEnd : DoResume;
        }

        public void ResumeGame() {
            if (IsGameEnded) return;
            
            DoResume();
        }
        
        public void LoadMenu() {
            Time.timeScale = 1;
            TransitionModal.Instance.DoTransitionIn(()=> SceneManager.LoadScene("Menu"));
        }

        public void LoadSelectStage() {
            Time.timeScale = 1;
            TransitionModal.Instance.DoTransitionIn(()=> SceneManager.LoadScene("SelectStageMenu"));
        }

        private void DoResume() {
            Time.timeScale = 1;
            PanelBg.DOFade(0, 0.15f);
            BgRect.DOLocalMoveY(-740f, .2f).OnComplete(() => {
                IsGamePaused = false;
            });
        }

        private void DoEnd() {
            IsGameEnded = true;
            Time.timeScale = 1;
            TransitionModal.Instance.DoTransitionIn(()=> SceneManager.LoadScene("GameScene_1"));
        }

        private void Awake() {
            ActionBtn.onClick.AddListener(()=> m_onActionBtn?.Invoke());
        }
    }
}