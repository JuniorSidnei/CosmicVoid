using System.Collections.Generic;
using DG.Tweening;
using ProjectA.Controllers;
using ProjectA.Modals;
using ProjectA.Singletons.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ProjectA.Managers {
    
    public class HUDManager : MonoBehaviour {
        
        [Header("life settings")]
        public GameObject LifePrefab;
        public Transform LifeContainer;
        public List<GameObject> PlayerLife = new List<GameObject>();

        public TextMeshProUGUI HitCount;

        [Header("player settings")]
        public List<GameObject> PlayerSettings = new List<GameObject>();

        [Header("cut scene layers")]
        public Image UpLayer;
        public Image DownLayer;

        [Header("modal")]
        public HighScoreModal HighScoreModal;

        private int m_playerMaxHealth;
    
        
        private void Awake() {
            TransitionModal.Instance.DoTransitionOut();
            
            HitCount.text = "0";
            GameManager.Instance.Dispatcher.Subscribe<OnPlayerLifeUpdate>(OnPlayerLifeUpdate);
            GameManager.Instance.Dispatcher.Subscribe<OnPlayerRechargeLife>(OnPlayerRechargeLife);
            GameManager.Instance.Dispatcher.Subscribe<OnHitCountUpdate>(OnHitCountUpdate);
            GameManager.Instance.Dispatcher.Subscribe<OnCutsceneStarted>(OnInitialCutsceneStarted);
            GameManager.Instance.Dispatcher.Subscribe<OnCutSceneFinished>(OnInitialCutSceneFinished);
            GameManager.Instance.Dispatcher.Subscribe<OnShowStageScore>(OnShowStageScore);

            m_playerMaxHealth = GameManager.Instance.GameSettings.GameDifficulty == GameDifficulty.HARD ? 1 : GameManager.Instance.GameSettings.PlayerMaxLife;

            for (var i = 0; i < m_playerMaxHealth; i++) {
                var life = Instantiate(LifePrefab, LifeContainer);
                PlayerLife.Add(life.transform.GetChild(0).gameObject);
            }
        }

        private void OnShowStageScore(OnShowStageScore arg0) {
            PauseModal.Instance.IsHighScoreModalOn = true;
            HighScoreModal.Show(arg0.StageScore, arg0.NextSceneIndex, () => {
                TransitionModal.Instance.DoTransitionIn(() => {
                    SceneManager.LoadScene("GameScene_" + arg0.NextSceneIndex);
                });
            });
        }

        private void OnDisable() {
            if (!GameManager.Instance) {
                return;
            }
            
            GameManager.Instance.Dispatcher.Unsubscribe<OnPlayerLifeUpdate>(OnPlayerLifeUpdate);
            GameManager.Instance.Dispatcher.Unsubscribe<OnPlayerRechargeLife>(OnPlayerRechargeLife);
            GameManager.Instance.Dispatcher.Unsubscribe<OnHitCountUpdate>(OnHitCountUpdate);
            GameManager.Instance.Dispatcher.Unsubscribe<OnCutsceneStarted>(OnInitialCutsceneStarted);
            GameManager.Instance.Dispatcher.Unsubscribe<OnCutSceneFinished>(OnInitialCutSceneFinished);
        }
  
        private void OnInitialCutsceneStarted(OnCutsceneStarted arg0) {
            foreach (var settings in PlayerSettings)  {
                settings.SetActive(false);
            }
            
            UpLayer.rectTransform.DOAnchorPosY(261f, 1f);
            DownLayer.rectTransform.DOAnchorPosY(-555f, 1f);
        }

        private void OnInitialCutSceneFinished(OnCutSceneFinished ev) {
            UpLayer.rectTransform.DOAnchorPosY(541f, 1f);
            DownLayer.rectTransform.DOAnchorPosY(-830f, 1f);
            
            if(!GameManager.Instance.GameSettings.HasTutorialFinished) return;
            
            foreach (var settings in PlayerSettings) {
                settings.SetActive(true);
            }
        }

        private void OnHitCountUpdate(OnHitCountUpdate ev) {
            HitCount.text = ev.Count.ToString();
        }

        private void OnPlayerLifeUpdate(OnPlayerLifeUpdate ev) {
            for (var i = ev.CurrentLife; i < PlayerLife.Count; i++) {
                PlayerLife[i].SetActive(false);
            }
        }

        private void OnPlayerRechargeLife(OnPlayerRechargeLife ev) {
            for (var i = 0; i < PlayerLife.Count; i++) {
                PlayerLife[i].SetActive(true);
            }
        }
    }
}