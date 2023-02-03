using System;
using System.Collections;
using System.Collections.Generic;
using ProjectA.Entity.ProcessDamage;
using ProjectA.Singletons.Managers;
using UnityEngine;

namespace ProjectA.Entity.Boss {
    
    public class BossEntityHealth : MonoBehaviour {

        public int HitsHealth;

        private EntityProcessDamage m_entityProcessDamage;
        
        private void Awake() {
            GameManager.Instance.Dispatcher.Subscribe<OnHitBoss>(OnHitBoss);
            m_entityProcessDamage = GetComponent<EntityProcessDamage>();
        }

        private void OnHitBoss(OnHitBoss ev) {
            if (ev.Entity != m_entityProcessDamage) return;

            HitsHealth -= ev.Damage;

            if (HitsHealth <= 0) {
                Debug.Log("Morri feiao");
            }
        }
    }
}