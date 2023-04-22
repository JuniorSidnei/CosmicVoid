using System.Collections.Generic;
using DG.Tweening;
using ProjectA.Controllers;
using ProjectA.Singletons.Managers;
using TMPro;
using UnityEngine;
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

        private int m_playerMaxHealth;
        
        private void Awake() {
            TransitionModal.DoTransitionOut();
            
            HitCount.text = "0";
            GameManager.Instance.Dispatcher.Subscribe<OnPlayerLifeUpdate>(OnPlayerLifeUpdate);
            GameManager.Instance.Dispatcher.Subscribe<OnHitCountUpdate>(OnHitCountUpdate);
            GameManager.Instance.Dispatcher.Subscribe<OnCutsceneStarted>(OnInitialCutsceneStarted);
            GameManager.Instance.Dispatcher.Subscribe<OnCutSceneFinished>(OnInitialCutSceneFinished);

            m_playerMaxHealth = PlayerPrefs.GetInt("player_max_life", 3);

            for (var i = 0; i < m_playerMaxHealth; i++) {
                var life = Instantiate(LifePrefab, LifeContainer);
                PlayerLife.Add(life.transform.GetChild(0).gameObject);
            }
        }

        private void OnDisable() {
            if (!GameManager.Instance) {
                return;
            }
            
            GameManager.Instance.Dispatcher.Unsubscribe<OnPlayerLifeUpdate>(OnPlayerLifeUpdate);
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
    }
}