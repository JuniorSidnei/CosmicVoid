using System;
using System.Collections;
using System.Collections.Generic;
using ProjectA.Singletons.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectA.Managers {
    
    public class HUDManager : MonoBehaviour {
        
        public List<GameObject> PlayerLife = new List<GameObject>();

        private void Awake() {
            GameManager.Instance.Dispatcher.Subscribe<OnPlayerLifeUpdate>(OnPlayerLifeUpdate);
        }

        private void OnPlayerLifeUpdate(OnPlayerLifeUpdate ev) {
            PlayerLife[ev.CurrentLife].SetActive(false);
        }
    }
}