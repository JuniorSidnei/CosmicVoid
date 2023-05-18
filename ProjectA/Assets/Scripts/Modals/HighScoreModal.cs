using System;
using DG.Tweening;
using GameToBeNamed.Utils.Sound;
using ProjectA.Controllers;
using ProjectA.Singletons.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ProjectA.Modals {
    
    public class HighScoreModal : MonoBehaviour {
        
        public Image PanelBg;
        public RectTransform BgRect;
        public TextMeshProUGUI HighScoreText;
        public TextMeshProUGUI StageHighScoreText;
        public TextMeshProUGUI NewHighScoreText;
        public Button NextBtn;

        
        public void Show(int currentScore, int nextSceneIndex, Action onClickNextBtnCallback) {
            var currentHighScoreStage = PlayerPrefs.GetInt("hit_count_" + (nextSceneIndex - 1));
            NewHighScoreText.gameObject.SetActive(currentScore > currentHighScoreStage);
            StageHighScoreText.text = "Stage Score: " + currentScore;
            HighScoreText.text = "High Score: " + currentHighScoreStage;
            
            PanelBg.DOFade(0.8f,  0.15f);
            BgRect.DOLocalMoveY(0, .5f).OnComplete(() => {
                Time.timeScale = 0;
            }); 
            
            PlayerPrefs.SetInt("hit_count_" + (nextSceneIndex - 1), currentScore);
            PlayerPrefs.Save();

            NextBtn.onClick.AddListener(() => {
                Time.timeScale = 1f;
                AudioController.Instance.Play(GameManager.Instance.GameSettings.ConfirmUI, AudioController.SoundType.SoundEffect2D, GameManager.Instance.GameSettings.GetSfxVolumeReduceScale());
                onClickNextBtnCallback?.Invoke();
            });
        }
    }
}