using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using ProjectA.Controllers;
using ProjectA.Modals;
using ProjectA.Movement;
using ProjectA.Singletons.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectA.Managers {
    
    public class HUDManager : MonoBehaviour {
        
        public List<GameObject> PlayerLife = new List<GameObject>();
        public TextMeshProUGUI HitCount;

        [Header("player settings")]
        public List<GameObject> PlayerSettings = new List<GameObject>();

        [Header("cut scene layers")]
        public Image UpLayer;
        public Image DownLayer;
        
        private void Awake() {
            TransitionModal.DoTransitionOut();
            
            HitCount.text = "0";
            GameManager.Instance.Dispatcher.Subscribe<OnPlayerLifeUpdate>(OnPlayerLifeUpdate);
            GameManager.Instance.Dispatcher.Subscribe<OnHitCountUpdate>(OnHitCountUpdate);
            GameManager.Instance.Dispatcher.Subscribe<OnCutsceneStarted>(OnInitialCutsceneStarted);
            GameManager.Instance.Dispatcher.Subscribe<OnCutSceneFinished>(OnInitialCutSceneFinished);
        }

        private void OnInitialCutsceneStarted(OnCutsceneStarted arg0) {
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