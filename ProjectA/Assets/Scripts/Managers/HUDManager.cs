using System;
using System.Collections;
using System.Collections.Generic;
using ProjectA.Movement;
using ProjectA.Singletons.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectA.Managers {
    
    public class HUDManager : MonoBehaviour {
        
        public List<GameObject> PlayerLife = new List<GameObject>();
        public TextMeshProUGUI HitCount;
        
        private void Awake() {
            HitCount.text = "0";
            GameManager.Instance.Dispatcher.Subscribe<OnPlayerLifeUpdate>(OnPlayerLifeUpdate);
            GameManager.Instance.Dispatcher.Subscribe<OnHitCountUpdate>(OnHitCountUpdate);
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