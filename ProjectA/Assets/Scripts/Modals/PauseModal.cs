using System;
using DG.Tweening;
using GameToBeNamed.Utils.Sound;
using ProjectA.Controllers;
using ProjectA.Singletons.Managers;
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
        public AudioClip ClickUI;
        
        public bool IsGamePaused { get; set; }
        public bool IsGameEnded { get; set; }
        public bool IsHighScoreModalOn { get; set; }

        private Action m_onActionBtn;
        
        public void PauseGame(bool isEndGame = false) {
            if(IsHighScoreModalOn) return;
            
            AudioController.Instance.Pause();
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
            AudioController.Instance.Play(ClickUI, AudioController.SoundType.SoundEffect2D, GameManager.Instance.GameSettings.SfxVolume);
            AudioController.Instance.Pause();
            TransitionModal.Instance.DoTransitionIn(()=> SceneManager.LoadScene("Menu"));
        }

        public void LoadSelectStage() {
            Time.timeScale = 1;
            AudioController.Instance.Play(ClickUI, AudioController.SoundType.SoundEffect2D, GameManager.Instance.GameSettings.SfxVolume);
            AudioController.Instance.Pause();
            TransitionModal.Instance.DoTransitionIn(()=> SceneManager.LoadScene("SelectStageMenu"));
        }

        private void DoResume() {
            Time.timeScale = 1;
            AudioController.Instance.Resume();
            AudioController.Instance.Play(ClickUI, AudioController.SoundType.SoundEffect2D, GameManager.Instance.GameSettings.SfxVolume);
            PanelBg.DOFade(0, 0.15f);
            BgRect.DOLocalMoveY(-740f, .2f).OnComplete(() => {
                IsGamePaused = false;
            });
        }

        private void DoEnd() {
            IsGameEnded = true;
            Time.timeScale = 1;
            AudioController.Instance.Play(ClickUI, AudioController.SoundType.SoundEffect2D, GameManager.Instance.GameSettings.SfxVolume);
            TransitionModal.Instance.DoTransitionIn(()=> SceneManager.LoadScene($"GameScene_{GameManager.Instance.SceneIndex}"));
        }

        private void Awake() {
            ActionBtn.onClick.AddListener(()=> m_onActionBtn?.Invoke());
        }
    }
}