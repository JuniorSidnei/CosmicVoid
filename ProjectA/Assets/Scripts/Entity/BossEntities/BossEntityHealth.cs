using System;
using System.Collections;
using System.Collections.Generic;
using ProjectA.Entity.ProcessDamage;
using ProjectA.Singletons.Managers;
using UnityEngine;

namespace ProjectA.Entity.Boss {
    
    public class BossEntityHealth : MonoBehaviour {

        public int HitsHealth;
        public int RageHpActivation;
        
        private EntityProcessDamage m_entityProcessDamage;
        
        private void Awake() {
            GameManager.Instance.Dispatcher.Subscribe<OnHitBoss>(OnHitBoss);
            m_entityProcessDamage = GetComponent<EntityProcessDamage>();
        }

        private void OnHitBoss(OnHitBoss ev) {
            if (ev.Entity != m_entityProcessDamage) return;

            HitsHealth -= ev.Damage;
            
            GameManager.Instance.Dispatcher.Emit(new OnCameraScreenShake(ShakeForce.MEDIUM));
            
            if (HitsHealth <= RageHpActivation) {
                GameManager.Instance.Dispatcher.Emit(new OnBossRageMode());
            }

            if (HitsHealth <= 0) {
                //play boss death animation
                GameManager.Instance.OnBossDeath();
            }
        }
    }
}